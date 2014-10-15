using System;
using System.Collections.Generic;
using ivNet.Webstore.Models;
using Orchard;

namespace ivNet.Webstore.Services {
    public interface IShoppingCart : IDependency {
        IEnumerable<ShoppingCartItem> Items { get; }
        void Add(int productId, string size, int quantity = 1);
        void AddRange(IEnumerable<ShoppingCartItem> items);
        void Remove(int productId);
        ProductRecord GetProduct(int productId);
        IEnumerable<Tuple<ProductPart, int>> GetProducts();
        void UpdateItems();
        decimal Subtotal();
        decimal Vat();
        decimal Total();
        decimal ItemCount();
        void Clear();
    }
}