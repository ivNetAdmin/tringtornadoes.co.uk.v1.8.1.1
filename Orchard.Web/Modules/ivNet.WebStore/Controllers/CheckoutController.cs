using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Core.Common.Fields;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Themes;
using ivNet.Webstore.Helpers;
using ivNet.Webstore.Models;
using ivNet.Webstore.Services;
using ivNet.Webstore.ViewModels;

namespace ivNet.Webstore.Controllers {
    public class CheckoutController : Controller {
        private readonly dynamic _shapeFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerService _customerService;
        private readonly IShoppingCart _shoppingCart;
        private readonly IMembershipService _membershipService;
        private readonly Localizer _t;

        public CheckoutController(IShapeFactory shapeFactory, IAuthenticationService authenticationService, ICustomerService customerService, IShoppingCart shoppingCart, IMembershipService membershipService) {
            _shapeFactory = shapeFactory;
            _authenticationService = authenticationService;
            _customerService = customerService;
            _shoppingCart = shoppingCart;
            _membershipService = membershipService;
            _t = NullLocalizer.Instance;
        }

        [Themed]
        public ActionResult SignupOrLogin() {

            if (_authenticationService.GetAuthenticatedUser() != null)
                return RedirectToAction("SelectAddress");

            var shape = _shapeFactory.SignupOrLogin();
            return new ShapeResult(this, shape);
        }

        [Themed]
        public ActionResult Signup()
        {
            var shape = _shapeFactory.Checkout_Signup();
            return new ShapeResult(this, shape);
        }

        [Themed, HttpPost]
        public ActionResult Signup(SignupVM signup)
        {
            if(!ModelState.IsValid) {
                return new ShapeResult(this, _shapeFactory.Checkout_Signup(Signup: signup));
            }

            var customer       = _customerService.CreateCustomer(signup.Email, signup.Password);
            customer.FirstName = signup.FirstName.TrimSafe();
            customer.LastName  = signup.LastName.TrimSafe();
            customer.Title     = signup.Title.TrimSafe();

            _authenticationService.SignIn(customer.User, true);

            return RedirectToAction("SelectAddress");
        }

        [Themed]
        public ActionResult Login()
        {
            var shape = _shapeFactory.Checkout_Login();
            return new ShapeResult(this, shape);
        }

        [Themed, HttpPost]
        public ActionResult Login(LoginVM login) {

            // If there are any model errors, redisplay the login form
            if (!ModelState.IsValid)
            {
                var shape = _shapeFactory.Checkout_Login(Login: login);
                return new ShapeResult(this, shape);
            }

            // Validate the specified credentials
            var user = _membershipService.ValidateUser(login.Email, login.Password);

            // If no user was found, add a model error
            if (user == null) {
                ModelState.AddModelError("Email", _t("Incorrect username/password combination").ToString());
            }

            // If there are any model errors, redisplay the login form
            if (!ModelState.IsValid) {
                var shape = _shapeFactory.Checkout_Login(Login: login);
                return new ShapeResult(this, shape);
            }

            // Create a forms ticket for the user
            _authenticationService.SignIn(user, login.CreatePersistentCookie);

            // Redirect to the next step
            return RedirectToAction("SelectAddress");
        }

        [Themed]
        public ActionResult SelectAddress() {
            var currentUser = _authenticationService.GetAuthenticatedUser();
            
            if(currentUser == null) 
                throw new OrchardSecurityException(_t("Login required"));

            var customer = currentUser.ContentItem.As<CustomerPart>();
            var invoiceAddress = _customerService.GetAddress(customer.Id, "InvoiceAddress");
            var shippingAddress = _customerService.GetAddress(customer.Id, "ShippingAddress");

            var addressesVM = new AddressesVM {
                InvoiceAddress = MapAddress(invoiceAddress),
                ShippingAddress = MapAddress(shippingAddress)
            };

            var shape = _shapeFactory.Checkout_SelectAddress(Addresses: addressesVM);

            if (string.IsNullOrWhiteSpace(addressesVM.InvoiceAddress.Name))
                addressesVM.InvoiceAddress.Name = string.Format("{0} {1} {2}", customer.Title, customer.FirstName, customer.LastName);

            return new ShapeResult(this, shape);
        }

        [Themed, HttpPost]
        public ActionResult SelectAddress(AddressesVM addresses) {
            var currentUser = _authenticationService.GetAuthenticatedUser();

            if (currentUser == null)
                throw new OrchardSecurityException(_t("Login required"));

            if(!ModelState.IsValid) {
                return new ShapeResult(this, _shapeFactory.Checkout_SelectAddress(Addresses: addresses));
            }

            var customer = currentUser.ContentItem.As<CustomerPart>();
            MapAddress(addresses.InvoiceAddress, "InvoiceAddress", customer);
            MapAddress(addresses.ShippingAddress, "ShippingAddress", customer);

            return RedirectToAction("Summary");
        }

        [Themed]
        public ActionResult Summary() {
            var user = _authenticationService.GetAuthenticatedUser();

            if(user == null)
                throw new OrchardSecurityException(_t("Login required"));

            dynamic invoiceAddress = _customerService.GetAddress(user.Id, "InvoiceAddress");
            dynamic shippingAddress = _customerService.GetAddress(user.Id, "ShippingAddress");
            dynamic shoppingCartShape = _shapeFactory.ShoppingCart();

            var query = _shoppingCart.GetProducts().Select(tuple => _shapeFactory.ShoppingCartItem(
                Product: tuple.Item1,
                ContentItem: tuple.Item1.ContentItem,
                Quantity: tuple.Item2
            ));

            shoppingCartShape.ShopItems = query.ToArray();
            shoppingCartShape.Total     = _shoppingCart.Total();
            shoppingCartShape.Subtotal  = _shoppingCart.Subtotal();
            shoppingCartShape.Vat       = _shoppingCart.Vat();

            return new ShapeResult(this, _shapeFactory.Checkout_Summary(
                ShoppingCart: shoppingCartShape,
                InvoiceAddress: invoiceAddress,
                ShippingAddress: shippingAddress
            ));
        }

        private AddressVM MapAddress(AddressPart addressPart) {
            dynamic address = addressPart;
            var addressVM = new AddressVM();

            if (addressPart != null)
            {
                addressVM.Name         = address.Name.Value;
                addressVM.AddressLine1 = address.AddressLine1.Value;
                addressVM.AddressLine2 = address.AddressLine2.Value;
                addressVM.Zipcode      = address.Zipcode.Value;
                addressVM.City         = address.City.Value;
                addressVM.Country      = address.Country.Value;
            }

            return addressVM;
        }

        private AddressPart MapAddress(AddressVM source, string addressType, CustomerPart customerPart) {
            var addressPart = _customerService.GetAddress(customerPart.Id, addressType) ?? _customerService.CreateAddress(customerPart.Id, addressType);
            dynamic address = addressPart;

            address.Name.Value         = source.Name.TrimSafe();
            address.AddressLine1.Value = source.AddressLine1.TrimSafe();
            address.AddressLine2.Value = source.AddressLine2.TrimSafe();
            address.Zipcode.Value      = source.Zipcode.TrimSafe();
            address.City.Value         = source.City.TrimSafe();
            address.Country.Value      = source.Country.TrimSafe();

            return addressPart;
        }
    }
}