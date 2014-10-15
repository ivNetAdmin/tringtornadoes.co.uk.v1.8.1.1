using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Environment.ShellBuilders.Models;
using Orchard.Localization;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard.Users.Models;
using ivNet.Webstore.Models;
using ivNet.Webstore.Services;
using ivNet.Webstore.ViewModels;

namespace ivNet.Webstore.Controllers
{
    [Admin]
    public class OrderAdminController : Controller
    {
        private dynamic Shape { get; set; }
        private Localizer T { get; set; }
        private readonly IOrderService _orderService;
        private readonly IRepository<ProductRecord> _productRepository;
        private readonly INotifier _notifier;
        private readonly ISiteService _siteService;

        public OrderAdminController(INotifier notifier, IOrderService orderService, IShapeFactory shapeFactory, IRepository<ProductRecord> productRepository, ISiteService siteService)
        {
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
            _notifier = notifier;
            _orderService = orderService;
            _productRepository = productRepository;
            _siteService = siteService;
        }

        public ActionResult Index(PagerParameters pagerParameters) {
            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters.Page, pagerParameters.PageSize);
            var ordersQuery = _orderService.GetOrders();
            var orders = ordersQuery.Skip(pager.GetStartIndex()).Take(pager.PageSize);
            var pagerShape = Shape.Pager(pager).TotalItemCount(ordersQuery.Count());
            var model = Shape.Orders(Orders: orders.ToArray(), Pager: pagerShape);
            return View((object)model);
        }

        public ActionResult Edit(int id) {
            var order = _orderService.GetOrder(id);
            var model = BuildModel(order, new EditOrderVM(order));
            return View((object)model);
        }

        [HttpPost]
        public ActionResult Edit(EditOrderVM model)
        {
            var order = _orderService.GetOrder(model.Id);

            if (!ModelState.IsValid)
                return BuildModel(order, model);

            order.Status = model.Status;

            _notifier.Add(NotifyType.Information, T("The order has been saved"));
            return RedirectToAction("ListOrders", "CustomerAdmin", new { id = order.CustomerId });
        }

        private dynamic BuildModel(OrderRecord order, EditOrderVM editModel) {
            return Shape.Order(
                Order: order,
                Details: order.Details.Join(_productRepository.Table, x => x.ProductId, x => x.Id, (record, productRecord) => 
                    Shape.Detail
                    (
                        Sku: productRecord.Sku,
                        Price: productRecord.Price,
                        Quantity: record.Quantity,
                        Total: record.Total,
                        Size: record.Size
                    )).ToArray(),
                EditModel: editModel
            );
        }
    }
}