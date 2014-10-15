using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Services {
    public class ShoppingCart : IShoppingCart
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IRepository<ProductRecord> _productRepository;
        private readonly IOrchardServices _orchardServices;
        public IEnumerable<ShoppingCartItem> Items { get { return ItemsInternal.AsReadOnly(); } }

        private HttpContextBase HttpContext
        {
            get { return _workContextAccessor.GetContext().HttpContext; }
        }

        private List<ShoppingCartItem> ItemsInternal
        {
            get
            {
                var items = (List<ShoppingCartItem>)HttpContext.Session["ShoppingCart"];

                if (items == null)
                {
                    items = new List<ShoppingCartItem>();
                    HttpContext.Session["ShoppingCart"] = items;
                }

                return items;
            }
        }

        public ShoppingCart(IWorkContextAccessor workContextAccessor, IRepository<ProductRecord> productRepository, IOrchardServices orchardServices)
        {
            _workContextAccessor = workContextAccessor;
            _productRepository = productRepository;
            _orchardServices = orchardServices;
        }

        public void Add(int productId, string size, int quantity = 1)
        {
            var item = Items.SingleOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                item = new ShoppingCartItem(productId, size, quantity);
                ItemsInternal.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void AddRange(IEnumerable<ShoppingCartItem> items) {
            foreach (var item in items) {
                Add(item.ProductId, item.Size, item.Quantity);
            }
        }

        public void Remove(int productId)
        {
            var item = Items.SingleOrDefault(x => x.ProductId == productId);

            if (item == null)
                return;

            ItemsInternal.Remove(item);
        }

        public ProductRecord GetProduct(int productId)
        {
            return _productRepository.Get(productId);
        }

        public IEnumerable<Tuple<ProductPart, int>> GetProducts() {
            var ids = Items.Select(x => x.ProductId);

            var productParts = _orchardServices.ContentManager.GetMany<ProductPart>(ids, VersionOptions.Latest, QueryHints.Empty).ToArray();

            var query = from item in Items
                        from product in productParts
                        where product.Id == item.ProductId
                        select new Tuple<ProductPart, int>(product, item.Quantity);

            return query;
        }

        public void UpdateItems()
        {
            ItemsInternal.RemoveAll(x => x.Quantity <= 0);
        }

        public decimal Subtotal()
        {
            return Items.Select(x => GetProduct(x.ProductId).Price * x.Quantity).Sum();
        }

        public decimal Vat()
        {
            return Subtotal() * .20m;
        }

        public decimal Total()
        {
            return Subtotal() + Vat();
        }

        public decimal ItemCount() {
            return Items.Sum(x => x.Quantity);
        }

        public void Clear()
        {
            ItemsInternal.Clear();
            UpdateItems();
        }
    }
}