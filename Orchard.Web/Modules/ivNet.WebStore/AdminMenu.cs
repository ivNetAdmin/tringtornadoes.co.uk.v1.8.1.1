using System.Web.Routing;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace ivNet.Webstore {
    public class AdminMenu : INavigationProvider
    {
        private readonly Work<RequestContext> _requestContextAccessor;

        public string MenuName {
            get { return "admin"; }
        }

        public AdminMenu(Work<RequestContext> requestContextAccessor) {
            _requestContextAccessor = requestContextAccessor;
            T = NullLocalizer.Instance;
        }

        private Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder) {

            var requestContext = _requestContextAccessor.Value;
            var idValue = (string) requestContext.RouteData.Values["id"];
            var id = 0;

            if (!string.IsNullOrEmpty(idValue)) {
                int.TryParse(idValue, out id);
            }

            builder
                
                // Image set
                .AddImageSet("webstore")

                // "Webstore"
                .Add(item => item
 
                    .Caption(T("Webstore"))
                    .Position("2")
                    .LinkToFirstChild(true)
                
                    // "Customers"
                    .Add(subItem => subItem
                        .Caption(T("Customers"))
                        .Position("2.1")
                        .Action("Index", "CustomerAdmin", new { area = "ivNet.Webstore" })
                        
                        .Add(T("Details"), i => i.Action("Edit", "CustomerAdmin", new { id }).LocalNav())
                        .Add(T("Addresses"), i => i.Action("ListAddresses", "CustomerAdmin", new { id }).LocalNav())
                        .Add(T("Orders"), i => i.Action("ListOrders", "CustomerAdmin", new { id }).LocalNav())
                    )

                    // "Orders"
                    .Add(subItem => subItem
                        .Caption(T("Orders"))
                        .Position("2.2")
                        .Action("Index", "OrderAdmin", new { area = "ivNet.Webstore" })
                    )
                );
        }
    }
}