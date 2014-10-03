using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ivNet.WebStore.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Themes;
using ivNet.Webstore.Extensibility;
using ivNet.Webstore.Models;
using ivNet.Webstore.Services;

namespace ivNet.Webstore.Controllers {
    public class OrderController : Controller {
        private readonly dynamic _shapeFactory;
        private readonly IOrderService _orderService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IShoppingCart _shoppingCart;
        private readonly ICustomerService _customerService;
        private readonly IEnumerable<IPaymentServiceProvider> _paymentServiceProviders;
        private readonly Localizer _t;

        public OrderController(
            IShapeFactory shapeFactory, 
            IOrderService orderService, 
            IAuthenticationService authenticationService, 
            IShoppingCart shoppingCart, 
            ICustomerService customerService,
            IEnumerable<IPaymentServiceProvider> paymentServiceProviders
            ) {
            _shapeFactory                  = shapeFactory;
            _orderService                  = orderService;
            _authenticationService         = authenticationService;
            _shoppingCart                  = shoppingCart;
            _customerService               = customerService;
            _paymentServiceProviders       = paymentServiceProviders;
            _t                             = NullLocalizer.Instance;
        }

        [Themed, HttpPost]
        public ActionResult Create() {

            var user = _authenticationService.GetAuthenticatedUser();

            if(user == null)
                throw new OrchardSecurityException(_t("Login required"));

            var customer = user.ContentItem.As<CustomerPart>();

            if(customer == null)
                throw new InvalidOperationException("The current user is not a customer");

            var order = _orderService.CreateOrder(customer.Id, _shoppingCart.Items);

            // Fire the PaymentRequest event
            var paymentRequest = new PaymentRequest(order);
            var products = _shoppingCart.GetProducts();

            foreach (var productPart in products)
            {
                var cakes = productPart;
            }

            //paymentRequest.OrderDescription = _orderService.GetProducts(order.Details).ToArray();

            foreach (var handler in _paymentServiceProviders) {
                handler.RequestPayment(paymentRequest);

                // If the handler responded, it will set the action result
                if (paymentRequest.WillHandlePayment) {
                    return paymentRequest.ActionResult;
                }
            }

            // If we got here, no PSP handled the PaymentRequest event, so we'll just display the order.
            var shape = _shapeFactory.Order_Created(
                Order: order,
                Products: _orderService.GetProducts(order.Details).ToArray(),
                Customer: customer,
                InvoiceAddress: (dynamic)_customerService.GetAddress(user.Id, "InvoiceAddress"),
                ShippingAddress: (dynamic)_customerService.GetAddress(user.Id, "ShippingAddress")
            );
            return new ShapeResult(this, shape);
        }

        [Themed]
        public ActionResult PaymentResponse() {

            var args = new PaymentResponse(HttpContext);

            foreach (var handler in _paymentServiceProviders) {
                handler.ProcessResponse(args);

                if (args.WillHandleResponse)
                    break;
            }

            if(!args.WillHandleResponse)
                throw new OrchardException(_t("Such things mean trouble"));

            var order = _orderService.GetOrderByNumber(args.OrderReference);
            _orderService.UpdateOrderStatus(order, args);

            if (order.Status == OrderStatus.Paid) {
                // Send some notification mail message to the customer that the order was paid.
                // We may also initiate the shipping process from here
            }

            return new ShapeResult(this, _shapeFactory.Order_PaymentResponse(Order: order, PaymentResponse: args));
        }

        [HttpPost]
        public HttpStatusCodeResult IPN(FormCollection result)
        {
            try
            {
                var payPalPaymentInfo = new PayPalPaymentInfo();

                TryUpdateModel(payPalPaymentInfo, result.ToValueProvider());

                var model = new PayPalListenerModel();
                model.PayPalPaymentInfo = payPalPaymentInfo;

                var parameters = Request.BinaryRead(Request.ContentLength);

                if (parameters != null)
                {
                //    model.GetStatus(parameters);
                }


                var order = _orderService.GetOrderByNumber(1013.ToString(CultureInfo.InvariantCulture));
                _orderService.UpdateOrderStatus(order,"Paid");;

                return new HttpStatusCodeResult(200, "Success");
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500, "Error");
            }
        }


        [HttpPost]
        [Themed]
        public ActionResult ThankYou()
        {
            return View();
        }

        [HttpPost]
        [Themed]
        public ActionResult Error()
        {
            return View();
        }
    }
}