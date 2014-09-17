using System;
using System.Web.Mvc;
using Orchard.DisplayManagement;
using Orchard.Themes;

namespace ivNet.Webstore.Controllers {
    public class SimulatedPaymentServiceProviderController : Controller {
        
        private readonly dynamic _shapeFactory;

        public SimulatedPaymentServiceProviderController(IShapeFactory shapeFactory) {
            _shapeFactory = shapeFactory;
        }

        [Themed]
        public ActionResult Index(string orderReference, int amount) {
            var model = _shapeFactory.PaymentRequest(
                OrderReference: orderReference,
                Amount: amount
                );

            return View(model);
        }

        [HttpPost]
        public ActionResult Command(string command, string orderReference) {

            // Generate a fake payment ID
            var paymentId = new Random(Guid.NewGuid().GetHashCode()).Next(1000, 9999);

            // Redirect back to the webstore
            return RedirectToAction("PaymentResponse", "Order", new {area = "ivNet.Webstore", paymentId = paymentId, result = command, orderReference});
        }
    }
}