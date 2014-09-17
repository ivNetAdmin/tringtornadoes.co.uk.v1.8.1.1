using Orchard.ContentManagement.Records;

namespace ivNet.Webstore.Models {
    public class AddressRecord : ContentPartRecord {
        public virtual int CustomerId { get; set; }
        public virtual string Type { get; set; }
    }
}