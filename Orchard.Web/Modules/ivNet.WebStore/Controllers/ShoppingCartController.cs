using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Core.Title.Models;
using Orchard.DisplayManagement;
using Orchard.Mvc;
using Orchard.Themes;
using ivNet.Webstore.Models;
using ivNet.Webstore.Services;
using ivNet.Webstore.ViewModels;

namespace ivNet.Webstore.Controllers {
    public class ShoppingCartController : Controller {
        private readonly IShoppingCart _shoppingCart;
        private readonly dynamic _shapeFactory;

        public ShoppingCartController(IShoppingCart shoppingCart, IShapeFactory shapeFactory) {
            _shoppingCart = shoppingCart;
            _shapeFactory = shapeFactory;
        }

        [HttpPost]
        public ActionResult Add(int id, string size) {
            _shoppingCart.Add(id, size);
            return RedirectToAction("Index");
        }

        [Themed]
        public ActionResult oldIndex() {
            dynamic shape = _shapeFactory.ShoppingCart();

            var query = _shoppingCart.GetProducts().Select(tuple => _shapeFactory.ShoppingCartItem(
                Product: tuple.Item1,
                ContentItem: tuple.Item1.ContentItem,
                Quantity: tuple.Item2
            ));

            shape.ShopItems  = query.ToArray();
            shape.Total      = _shoppingCart.Total();
            shape.Subtotal   = _shoppingCart.Subtotal();
            shape.Vat        = _shoppingCart.Vat();

            return new ShapeResult(this, shape);
        }

        [Themed]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(string command, FormCollection form)
        {
            UpdateShoppingCart(form);

            switch (command)
            {
                case "Checkout":
                    return RedirectToAction("SignupOrLogin", "Checkout");
                case "ContinueShopping":
                    return Redirect("/shop");
            }

            return RedirectToAction("Index");
        }

        //[AjaxOnly]
        public ActionResult GetItems() {
            var products = _shoppingCart.GetProducts();

            var json = new
            {
                items = (from item in products
                         let titlePart = item.Item1.As<TitlePart>()
                         select new
                         {
                             id = item.Item1.Id,
                             title = titlePart != null ? titlePart.Title : "(No TitlePart attached)",
                             unitPrice = item.Item1.Price,
                             quantity  = item.Item2
                         }).ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private void UpdateShoppingCart(FormCollection form)
        {
            var formKeys = form.AllKeys;
            var items = (from key in formKeys
                where key.Length > 3
                where key.Substring(0, 3) == "prd"
                select new UpdateShoppingCartItemVM
                {
                    ProductId = Convert.ToInt32(key.Substring(3)),
                    Quantity = Convert.ToInt32(form[key]),
                    Size = form["size" + key.Substring(3)]
                }).ToList();

            if (items.Count == 0) return;

            _shoppingCart.Clear();

            _shoppingCart.AddRange(items
                .Where(item => !item.IsRemoved)
                .Select(item => new ShoppingCartItem(item.ProductId, item.Size, item.Quantity < 0 ? 0 : item.Quantity))
            );

            _shoppingCart.UpdateItems();
        }
    }
}