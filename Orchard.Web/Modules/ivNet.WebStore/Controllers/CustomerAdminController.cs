using System;
using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard.Users.Models;
using ivNet.Webstore.Helpers;
using ivNet.Webstore.Models;
using ivNet.Webstore.Services;
using ivNet.Webstore.ViewModels;

namespace ivNet.Webstore.Controllers {

    [Admin]
    public class CustomerAdminController : Controller, IUpdateModel {
        private dynamic Shape { get; set; }
        private Localizer T { get; set; }
        private readonly ICustomerService _customerService;
        private readonly ISiteService _siteService;
        private readonly IContentManager _contentManager;
        private readonly INotifier _notifier;
        private readonly IOrderService _orderService;

        public CustomerAdminController
        (
            ICustomerService customerService, 
            IShapeFactory shapeFactory, 
            ISiteService siteService, 
            IContentManager contentManager, 
            INotifier notifier,
            IOrderService orderService
        ) 
        {
            Shape            = shapeFactory;
            T                = NullLocalizer.Instance;
            _customerService = customerService;
            _siteService     = siteService;
            _contentManager  = contentManager;
            _notifier        = notifier;
            _orderService = orderService;
        }

        public ActionResult Index(PagerParameters pagerParameters, CustomersSearchVM search) {

            // Create a basic query that selects all customer content items, joined with the UserPartRecord table
            var customerQuery = _customerService.GetCustomers().Join<UserPartRecord>().List();

            // If the user specified a search expression, update the query with a filter
            if (!string.IsNullOrWhiteSpace(search.Expression)) {
                
                var expression = search.Expression.Trim();

                customerQuery = from customer in customerQuery
                        where
                            customer.FirstName.Contains(expression, StringComparison.InvariantCultureIgnoreCase) ||
                            customer.LastName.Contains(expression, StringComparison.InvariantCultureIgnoreCase) ||
                            customer.As<UserPart>().Email.Contains(expression)
                        select customer;
            }

            // Project the query into a list of customer shapes
            var customersProjection = from customer in customerQuery
                                      select Shape.Customer
                                      (
                                        Id: customer.Id, 
                                        FirstName: customer.FirstName,
                                        LastName: customer.LastName,
                                        Email: customer.As<UserPart>().Email,
                                        CreatedAt: customer.CreatedAt
                                      );

            // The pager is used to apply paging on the query and to create a PagerShape
            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters.Page, pagerParameters.PageSize);

            // Apply paging
            var customers = customersProjection.Skip(pager.GetStartIndex()).Take(pager.PageSize);

            // Construct a Pager shape
            var pagerShape = Shape.Pager(pager).TotalItemCount(customerQuery.Count());

            // Create the viewmodel
            var model = new CustomersIndexVM(customers, search, pagerShape);

            return View(model);
        }

        public ActionResult Edit(int id) {
            var customer = _customerService.GetCustomer(id);
            var model = _contentManager.BuildEditor(customer);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPOST(int id)
        {
            var customer = _customerService.GetCustomer(id);
            var model = _contentManager.UpdateEditor(customer, this);

            if (!ModelState.IsValid)
                return View(model);

            _notifier.Add(NotifyType.Information, T("Your customer has been saved"));
            return RedirectToAction("Edit", new { id });
        }

        public ActionResult ListAddresses(int id) {
            var addresses = _customerService.GetAddresses(id).ToArray();
            return View(addresses);
        }

        public ActionResult ListOrders(int id)
        {
            var orders = _orderService.GetOrders(id).ToArray();
            return View(orders);
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.Text);
        }
    }
}