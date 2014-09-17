using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Handlers {
    public class ProductHandler : ContentHandler {
        public ProductHandler(IRepository<ProductRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}