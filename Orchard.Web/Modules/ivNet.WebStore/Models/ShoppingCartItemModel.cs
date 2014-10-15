
namespace ivNet.WebStore.Models
{
    public class ShoppingCartItemModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }       
        public string Sku { get; set; }
        public string[] Sizes { get; set; }
    }
}