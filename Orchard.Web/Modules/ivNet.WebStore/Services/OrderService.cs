using System;
using System.Collections.Generic;
using System.Linq;
using ivNet.WebStore.Helpers;
using ivNet.WebStore.Models;
using Newtonsoft.Json;
using Orchard.ContentManagement;
using Orchard.Data;
using ivNet.Webstore.Extensibility;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Services {
    public class OrderService : IOrderService {
        private readonly IDateTimeService _dateTimeService;
        private readonly IRepository<ProductRecord> _productRepository;
        private readonly IContentManager _contentManager;
        private readonly IRepository<OrderRecord> _orderRepository;
        private readonly IRepository<OrderDetailRecord> _orderDetailRepository;

        public OrderService(IDateTimeService dateTimeService, IRepository<ProductRecord> productRepository, IContentManager contentManager, IRepository<OrderRecord> orderRepository , IRepository<OrderDetailRecord> orderDetailRepository ) {
            _dateTimeService = dateTimeService;
            _productRepository = productRepository;
            _contentManager = contentManager;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public OrderRecord CreateOrder(int customerId, IEnumerable<ShoppingCartItem> items) {

            if(items == null)
                throw new ArgumentNullException("items");

            // Convert to an array to avoid re-running the enumerable
            var itemsArray = items.ToArray();

            if(!itemsArray.Any())
                throw new ArgumentException("Creating an order with 0 items is not supported", "items");

            var order = new OrderRecord {
                CreatedAt  = _dateTimeService.Now,
                CustomerId = customerId,
                Status     = OrderStatus.New
            };

            _orderRepository.Create(order);

            // Get all products in one shot, so we can add the product reference to each order detail
            var productIds = itemsArray.Select(x => x.ProductId).ToArray();
            var products = _productRepository.Fetch(x => productIds.Contains(x.Id)).ToArray();

            // Create an order detail for each item
            foreach (var item in itemsArray) {
                var product = products.Single(x => x.Id == item.ProductId);
                
                var detail      = new OrderDetailRecord {
                    OrderRecord_Id     = order.Id,
                    ProductId   = product.Id,
                    Quantity    = item.Quantity,
                    Size        = item.Size,
                    UnitPrice   = product.Price,
                    VatRate     = .19m
                };

                _orderDetailRepository.Create(detail);
                order.Details.Add(detail);
            }

            order.UpdateTotals();
            
            return order;
        }

        /// <summary>
        /// Gets a list of ProductParts from the specified list of OrderDetails. Useful if you need to use the product as a ProductPart (instead of just having access to the ProductRecord instance).
        /// </summary>
        public IEnumerable<ProductPart> GetProducts(IEnumerable<OrderDetailRecord> orderDetails) {
            var productIds = orderDetails.Select(x => x.ProductId).ToArray();
            return _contentManager.GetMany<ProductPart>(productIds, VersionOptions.Latest, QueryHints.Empty);
        }

        public OrderRecord GetOrderByNumber(string orderNumber) {
            var orderId = int.Parse(orderNumber) - 1000;
            return _orderRepository.Get(orderId);
        }

        public OrderRecord UpdateOrderStatus(PayPalPaymentInfo payPalPaymentInfo)
        {
            var order = GetOrderByNumber(payPalPaymentInfo.invoice);         
            
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
            
            //if (order.Status == orderStatus)
           //     return;

            order.Status = orderStatus;
            order.PaymentServiceProviderResponse = JsonConvert.SerializeObject(payPalPaymentInfo);
            order.PaymentReference = payPalPaymentInfo.txn_id;

            switch(order.Status) {
                case OrderStatus.Paid:
                    order.PaidAt = _dateTimeService.Now;
                    break;
                case OrderStatus.Completed:
                    order.CompletedAt = _dateTimeService.Now;
                    break;
                case OrderStatus.Cancelled:
                    order.CancelledAt = _dateTimeService.Now;
                    break;
            }

            //PayPalLog.Debug(JsonConvert.SerializeObject(order));

           // _orderRepository.Update(order);

            PayPalLog.Debug("Updated");
            return order;
        }        

        public IEnumerable<OrderRecord> GetOrders(int customerId) {
            return _orderRepository.Fetch(x => x.CustomerId == customerId);
        }

        public IQueryable<OrderRecord> GetOrders()
        {
            return _orderRepository.Table.OrderByDescending(o => o.Number);
        }

        public OrderRecord GetOrder(int id) {
            return _orderRepository.Get(id);
        }
    }
}