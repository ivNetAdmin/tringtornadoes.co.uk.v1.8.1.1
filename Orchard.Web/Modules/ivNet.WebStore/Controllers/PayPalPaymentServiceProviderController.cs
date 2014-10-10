
using System;
using System.Linq;
using System.Web.Mvc;
using ivNet.Webstore.Services;
using Orchard.DisplayManagement;
using Orchard.Themes;

namespace ivNet.WebStore.Controllers
{
    public class PayPalPaymentServiceProviderController : Controller
    {
         private readonly dynamic _shapeFactory;
         private readonly IOrderService _orderService;

         public PayPalPaymentServiceProviderController(IShapeFactory shapeFactory,IOrderService orderService)
         {
            _shapeFactory = shapeFactory;
            _orderService = orderService;
        }

        [Themed]
        public ActionResult Index(string orderReference, int amount)
        {

            //var order = _orderService.GetOrderByNumber(orderReference);
            //var products = _orderService.GetProducts(order.Details).ToArray();

            var model = _shapeFactory.PaymentRequest(
                OrderReference: orderReference,
                Amount: amount
                );

            return View(model);
        }

        //[HttpPost]
        //public ActionResult Command(string command, string orderReference) {

        //    // Generate a fake payment ID
        //    var paymentId = new Random(Guid.NewGuid().GetHashCode()).Next(1000, 9999);

        //    // Redirect back to the webstore
        //    return RedirectToAction("PaymentResponse", "Order", new {area = "ivNet.Webstore", paymentId = paymentId, result = command, orderReference});
        //}
    }
}