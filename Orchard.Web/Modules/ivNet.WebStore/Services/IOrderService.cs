using System.Collections.Generic;
using System.Linq;
using ivNet.Webstore.Extensibility;
using ivNet.Webstore.Models;
using ivNet.WebStore.Models;
using Orchard;

namespace ivNet.Webstore.Services {
    public interface IOrderService : IDependency {
        /// <summary>
        /// Creates a new order based on the specified ShoppingCartItems
        /// </summary>
        OrderRecord CreateOrder(int customerId, IEnumerable<ShoppingCartItem> items);

        /// <summary>
        /// Gets a list of ProductParts from the specified list of OrderDetails. Useful if you need to use the product as a ProductPart (instead of just having access to the ProductRecord instance).
        /// </summary>
        IEnumerable<ProductPart> GetProducts(IEnumerable<OrderDetailRecord> orderDetails);

        OrderRecord GetOrderByNumber(string orderNumber);        
        IEnumerable<OrderRecord> GetOrders(int customerId);
        IQueryable<OrderRecord> GetOrders();
        OrderRecord GetOrder(int id);

        OrderRecord UpdateOrderStatus(PayPalPaymentInfo payPalPaymentInfo);
    }
}