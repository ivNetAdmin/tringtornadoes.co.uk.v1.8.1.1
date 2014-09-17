using Orchard.ContentManagement.Records;

namespace ivNet.Webstore.Models
{
    public class ProductRecord : ContentPartRecord
    {
        public virtual decimal Price { get; set; }
        public virtual string Sku { get; set; }
    }
}
