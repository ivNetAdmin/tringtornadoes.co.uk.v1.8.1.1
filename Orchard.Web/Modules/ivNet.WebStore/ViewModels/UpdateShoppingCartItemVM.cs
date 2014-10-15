namespace ivNet.Webstore.ViewModels
{
    public class UpdateShoppingCartItemVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public bool IsRemoved { get; set; }
    }
}