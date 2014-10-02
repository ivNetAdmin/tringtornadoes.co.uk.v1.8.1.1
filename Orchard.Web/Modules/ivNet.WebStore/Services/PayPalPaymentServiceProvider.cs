
using System.Web.Mvc;
using System.Web.Routing;
using ivNet.Webstore.Extensibility;
using Orchard.Environment.Extensions;

namespace ivNet.WebStore.Services
{
     [OrchardFeature("ivNet.Webstore.PayPalPSP")]
    public class PayPalPaymentServiceProvider : IPaymentServiceProvider
    {
        public void RequestPayment(PaymentRequest e)
        {

            e.ActionResult = new RedirectToRouteResult(new RouteValueDictionary {
                {"action", "Index"},
                {"controller", "PayPalPaymentServiceProvider"},
                {"area", "ivNet.Webstore"},
                {"orderReference", e.Order.Number},
                {"amount", (int)(e.Order.Total * 100)}
            });

            e.WillHandlePayment = true;
        }

        public void ProcessResponse(PaymentResponse e)
        {
            var result = e.HttpContext.Request.QueryString["result"];

            //e.OrderReference = e.HttpContext.Request.QueryString["orderReference"];
            //e.PaymentReference = e.HttpContext.Request.QueryString["paymentId"];
            //e.ResponseText = e.HttpContext.Request.QueryString.ToString();

            //switch (result)
            //{
            //    case "Success":
            //        e.Status = PaymentResponseStatus.Success;
            //        break;
            //    case "Failure":
            //        e.Status = PaymentResponseStatus.Failed;
            //        break;
            //    case "Cancelled":
            //        e.Status = PaymentResponseStatus.Cancelled;
            //        break;
            //    default:
            //        e.Status = PaymentResponseStatus.Exception;
            //        break;
            //}

            //e.WillHandleResponse = true;
        }
    }
 }