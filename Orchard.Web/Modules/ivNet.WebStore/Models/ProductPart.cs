using Orchard.ContentManagement;

namespace ivNet.Webstore.Models {
    public class ProductPart : ContentPart<ProductRecord>
    {
        public decimal Price {
            get { return Record.Price; }
            set { Record.Price = value; }
        }

        public string Sku {
            get { return Record.Sku; }
            set { Record.Sku = value; }
        }
    }
}