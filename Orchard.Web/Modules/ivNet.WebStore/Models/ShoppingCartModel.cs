
using System;
using System.Collections.Generic;

namespace ivNet.WebStore.Models
{
    public class ShoppingCartModel
    {
        public ShoppingCartModel()
        {
            ShopItems=new List<ShoppingCartItemModel>();
        }
        public List<ShoppingCartItemModel> ShopItems { get; set; }
        //public decimal Total { get; set; }
        //public decimal Subtotal { get; set; }
        //public decimal Vat { get; set; }
        public int ItemCount { get; set; }
    }
}