using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Handlers {
    public class AddressHandler : ContentHandler {
        public AddressHandler(IRepository<AddressRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}