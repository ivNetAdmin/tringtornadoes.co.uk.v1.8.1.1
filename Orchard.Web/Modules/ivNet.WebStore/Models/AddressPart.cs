using Orchard.ContentManagement;

namespace ivNet.Webstore.Models {
    public class AddressPart : ContentPart<AddressRecord> {
        public int CustomerId {
            get { return Record.CustomerId; }
            set { Record.CustomerId = value; }
        }

        public string Type {
            get { return Record.Type; }
            set { Record.Type = value; }
        }
    }
}