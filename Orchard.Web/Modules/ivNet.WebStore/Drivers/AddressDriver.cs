using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Drivers {
    public class AddressDriver : ContentPartDriver<AddressPart>
    {
        protected override DriverResult Editor(AddressPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Address_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Address", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(AddressPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}