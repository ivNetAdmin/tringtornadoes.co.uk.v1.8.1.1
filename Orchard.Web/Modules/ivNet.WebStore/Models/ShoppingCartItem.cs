using System;

namespace ivNet.Webstore.Models {
    [Serializable]
    public sealed class ShoppingCartItem
    {
        public int ProductId { get; private set; }
        public String Size { get; private set; }

        private int _quantity;
        
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 0)
                    throw new IndexOutOfRangeException();

                _quantity = value;
            }
        }

        public ShoppingCartItem()
        {
        }

        public ShoppingCartItem(int productId, string size, int quantity = 1)
        {           
            ProductId = productId;
            Quantity = quantity;
            Size = size;
        }
    }
}