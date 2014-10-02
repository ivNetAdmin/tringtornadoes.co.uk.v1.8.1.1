using System.Web.Mvc;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Extensibility
{
    public class PaymentRequest
    {
        public OrderRecord Order { get; private set; }
        public string OrderDescription { get; set; }
        public bool WillHandlePayment { get; set; }
        public ActionResult ActionResult { get; set; }

        public PaymentRequest(OrderRecord order) {
            Order = order;
        }
    }
}