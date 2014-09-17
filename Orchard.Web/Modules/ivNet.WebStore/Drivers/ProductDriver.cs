using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Drivers {
    public class ProductDriver : ContentPartDriver<ProductPart> {
        protected override DriverResult Editor(ProductPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Product_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Product", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(ProductPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        //protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper)
        //{
        //    var driverResults = new List<DriverResult> {
        //        ContentShape("Parts_Product", () => shapeHelper.Parts_Product(
        //            Price: part.Price,
        //            Sku: part.Sku
        //        ))
        //    };

        //    if (displayType != "SummaryAdmin")
        //    {
        //        driverResults.Add
        //        (
        //            ContentShape("Parts_Product_AddButton", () => 
        //                shapeHelper.Parts_Product_AddButton(ProductId: part.Id))
        //        );
        //    }

        //    return Combined(driverResults.ToArray());
        //}

        protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper)
        {
            return Combined(
                ContentShape("Parts_Product_SummaryAdmin", () => shapeHelper.Parts_Product_SummaryAdmin(
                    Price: part.Price,
                    Sku: part.Sku
                )),
                ContentShape("Parts_Product", () => shapeHelper.Parts_Product(
                    Price: part.Price,
                    Sku: part.Sku
                )),
                 ContentShape("Parts_Product_AddButton", () => shapeHelper.Parts_Product_AddButton(
                     ProductId: part.Id
                     ))
                );
        }
    }
}