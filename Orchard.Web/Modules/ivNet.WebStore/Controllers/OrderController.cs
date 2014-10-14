using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ivNet.WebStore.Helpers;
using ivNet.WebStore.Models;
using Newtonsoft.Json;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Themes;
using ivNet.Webstore.Extensibility;
using ivNet.Webstore.Models;
using ivNet.Webstore.Services;
using Orchard.UI.Notify;

namespace ivNet.Webstore.Controllers {
    public class OrderController : Controller {
        private readonly dynamic _shapeFactory;
        private readonly IOrderService _orderService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IShoppingCart _shoppingCart;
        private readonly ICustomerService _customerService;
        private readonly IEnumerable<IPaymentServiceProvider> _paymentServiceProviders;
        private readonly INotifier _notifier;
        private readonly Localizer _t;

        public OrderController(
            IShapeFactory shapeFactory, 
            IOrderService orderService, 
            IAuthenticationService authenticationService, 
            IShoppingCart shoppingCart, 
            ICustomerService customerService,
            INotifier notifier,
            IEnumerable<IPaymentServiceProvider> paymentServiceProviders
            ) {
            _shapeFactory                  = shapeFactory;
            _orderService                  = orderService;
            _authenticationService         = authenticationService;
            _shoppingCart                  = shoppingCart;
            _customerService               = customerService;
            _paymentServiceProviders       = paymentServiceProviders;
            _t                             = NullLocalizer.Instance;
            _notifier = notifier;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        [Themed]
        public ActionResult Create() {

            var user = _authenticationService.GetAuthenticatedUser();

            if(user == null)
                throw new OrchardSecurityException(_t("Login required"));

            var customer = user.ContentItem.As<CustomerPart>();

            if(customer == null)
                throw new InvalidOperationException("The current user is not a customer");

            var order = _orderService.CreateOrder(customer.Id, _shoppingCart.Items);

            return Json(order.Number, JsonRequestBehavior.AllowGet);

           
        }

        [HttpPost]
        public HttpStatusCodeResult IPN(FormCollection result)
        {
            try
            {
                var payPalPaymentInfo = new PayPalPaymentInfo();

                TryUpdateModel(payPalPaymentInfo, result.ToValueProvider());

                var model = new PayPalListenerModel {PayPalPaymentInfo = payPalPaymentInfo};

                var parameters = Request.BinaryRead(Request.ContentLength);

                if (parameters.Length > 0)
                {
                    model.GetStatus(parameters);
                    PayPalLog.Debug(payPalPaymentInfo.invoice);
                    PayPalLog.Debug(payPalPaymentInfo.payment_status);

                    try
                    {
                        var order = _orderService.GetOrderByNumber(payPalPaymentInfo.invoice);

                        OrderStatus orderStatus;

                        switch (payPalPaymentInfo.payment_status.ToLower())
                        {
                            case "completed":
                                orderStatus = OrderStatus.Paid;
                                break;
                            default:
                                orderStatus = OrderStatus.Cancelled;
                                break;
                        }

                        order.Status = orderStatus;
                        order.PaymentServiceProviderResponse = JsonConvert.SerializeObject(payPalPaymentInfo);
                        order.PaymentReference = payPalPaymentInfo.txn_id;

                        switch (order.Status)
                        {
                            case OrderStatus.Paid:
                                order.PaidAt = DateTime.Now;
                                break;
                            case OrderStatus.Completed:
                                order.CompletedAt = DateTime.Now;
                                break;
                            case OrderStatus.Cancelled:
                                order.CancelledAt = DateTime.Now;
                                break;
                        }

                        _notifier.Add(NotifyType.Information, _t("The order has been saved"));

                    }
                    catch (Exception ex)
                    {
                        PayPalLog.Debug(string.Format("Error saving order [{0}] {1}", payPalPaymentInfo.invoice,
                            JsonConvert.SerializeObject(payPalPaymentInfo)));
                        PayPalLog.Error(ex);
                    }

                }
                else
                {
                    PayPalLog.Debug(string.Format("No PayPal return parameters [{0}]",
                        JsonConvert.SerializeObject(result)));
                }
            }
            catch (Exception ex)
            {
                PayPalLog.Debug(string.Format("Error unknown [{0}] {1}", ex.Message,
                    result));
                PayPalLog.Error(ex);
            }
            return new HttpStatusCodeResult(200, "Success");
        }

        public ActionResult Test()
        {
            

            //var order = _orderService.GetOrder(38);

            var payPalPaymentInfo = new PayPalPaymentInfo
            {
                invoice = "1035",
                payment_status = "completed",
                txn_id = "1234"
            };


            var order = _orderService.GetOrderByNumber(payPalPaymentInfo.invoice);

            OrderStatus orderStatus;

            switch (payPalPaymentInfo.payment_status.ToLower())
            {
                case "completed":
                    orderStatus = OrderStatus.Completed;
                    break;
                default:
                    orderStatus = OrderStatus.Cancelled;
                    break;
            }

            order.Status = orderStatus;
            order.PaymentServiceProviderResponse = JsonConvert.SerializeObject(payPalPaymentInfo);
            order.PaymentReference = payPalPaymentInfo.txn_id;

            switch (order.Status)
            {
                case OrderStatus.Paid:
                    order.PaidAt = DateTime.Now;
                    break;
                case OrderStatus.Completed:
                    order.CompletedAt = DateTime.Now;
                    break;
                case OrderStatus.Cancelled:
                    order.CancelledAt = DateTime.Now;
                    break;
            }

            _notifier.Add(NotifyType.Information, _t("The order has been saved"));


            //_orderService.UpdateOrderStatus(new PayPalPaymentInfo { invoice = "1038", payer_status = "Paid" , payment_status="completed"});
            return new HttpStatusCodeResult(200, "Success");
        }

        [Themed]
        public ActionResult ThankYou()
        {
            return View();
        }

        [Themed]
        public ActionResult Error()
        {
            return View();
        }
    }
}