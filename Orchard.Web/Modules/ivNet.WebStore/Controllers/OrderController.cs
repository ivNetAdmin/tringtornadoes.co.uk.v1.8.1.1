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
                        _orderService.UpdateOrderStatus(payPalPaymentInfo);
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