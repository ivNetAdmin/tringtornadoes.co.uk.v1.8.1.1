using Orchard.UI.Resources;

namespace ivNet.Webstore {
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineStyle("Webstore.Shoppingcart").SetUrl("shoppingcart.min.css");

            //manifest.DefineStyle("Webstore.Common").SetUrl("common.css");
            //manifest.DefineStyle("Webstore.ShoppingCart").SetUrl("shoppingcart.css").SetDependencies("Webstore.Common");
            //manifest.DefineStyle("Webstore.ShoppingCart.Widget").SetUrl("shoppingcart-widget.css").SetDependencies("Webstore.Common");
            //manifest.DefineStyle("Webstore.Checkout.Summary").SetUrl("checkout-summary.css").SetDependencies("Webstore.Common");
            //manifest.DefineStyle("Webstore.Order").SetUrl("order.css").SetDependencies("Webstore.Common");
            //manifest.DefineStyle("Webstore.SimulatedPSP").SetUrl("simulated-psp.css").SetDependencies("Webstore.Common");

            //manifest.DefineScript("KnockoutJS").SetUrl("knockout-2.0.0.js").SetVersion("2.0.0");
            //manifest.DefineScript("LinqJS").SetUrl("jquery.linq.min.js").SetVersion("2.2.0.2").SetDependencies("jQuery");
            //manifest.DefineScript("Globalize").SetUrl("globalize.js").SetDependencies("jQuery");
            //manifest.DefineScript("Globalize.Cultures").SetBasePath(manifest.BasePath + "scripts/cultures/").SetUrl("globalize.culture.js").SetCultures("en-US", "nl-NL").SetDependencies("Globalize", "jQuery");
            //manifest.DefineScript("Globalize.SetCulture").SetUrl("~/ivNet.Webstore/Resource/SetCultureScript").SetDependencies("Globalize.Cultures");
            ////manifest.DefineScript("Webstore.ShoppingCart").SetUrl("shoppingcart.js").SetDependencies("jQuery", "KnockoutJS", "LinqJS", "Globalize.SetCulture");
            /// 
            manifest.DefineScript("AngularJS").SetUrl("anjular.min.js").SetVersion("1.2.9").SetDependencies("jQuery");
            manifest.DefineScript("Webstore.ShoppingCart").SetUrl("shoppingcart.js").SetDependencies("AngularJS");
        }
    }
}