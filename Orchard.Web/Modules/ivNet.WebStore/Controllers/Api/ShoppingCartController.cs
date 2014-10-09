
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ivNet.Webstore.Models;
using ivNet.WebStore.Models;
using ivNet.Webstore.Services;
using Orchard.ContentManagement;
using Orchard.Core.Title.Models;
using Orchard.DisplayManagement;

namespace ivNet.WebStore.Controllers.Api
{
    public class ShoppingCartController : ApiController
    {
        private readonly IShoppingCart _shoppingCart;
     
        public ShoppingCartController(IShoppingCart shoppingCart, IShapeFactory shapeFactory)
        {
            _shoppingCart = shoppingCart;
        }
        
        public HttpResponseMessage Get()
        {
            var shoppingCartModel = new ShoppingCartModel();

            //var query = _shoppingCart.GetProducts().Select(tuple => _shapeFactory.ShoppingCartItem(
            //    Product: tuple.Item1,
            //    ContentItem: tuple.Item1.ContentItem,
            //    Quantity: tuple.Item2
            //));

            var query = _shoppingCart.GetProducts();
            var enumerable = query as IList<Tuple<ProductPart, int>> ?? query.ToList();

            var itemCount = 0;
            foreach (var tuple in enumerable)
            {                
                var contentItem = (ContentItem)tuple.Item1.ContentItem;
                var productId = contentItem.Id;
                var titlePart = contentItem.As<TitlePart>();
                var title = titlePart != null ? titlePart.Title : "(no titlePart attached)";
                itemCount += tuple.Item2;

                shoppingCartModel.ShopItems.Add(new ShoppingCartItemModel
                {
                    ProductId = productId,
                    Quantity = tuple.Item2,
                    Title = title,
                    Price = tuple.Item1.Price,
                    Sku = tuple.Item1.Sku
                });

            }

            shoppingCartModel.ItemCount = itemCount;
            //shoppingCartModel.Total = _shoppingCart.Total();
            //shoppingCartModel.Subtotal = _shoppingCart.Subtotal();
            //shoppingCartModel.Vat = _shoppingCart.Vat();

            return Request.CreateResponse(HttpStatusCode.OK,
                shoppingCartModel);
        }
    }
}