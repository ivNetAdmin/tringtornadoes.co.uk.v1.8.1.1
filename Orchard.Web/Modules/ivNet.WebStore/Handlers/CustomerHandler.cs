using System.Web.Routing;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Users.Models;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Handlers {
    public class CustomerHandler : ContentHandler {
        public CustomerHandler(IRepository<CustomerRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<UserPart>("Customer"));

            OnGetContentItemMetadata<CustomerPart>((context, part) => {

                if (context.Metadata.EditorRouteValues == null)
                {
                    context.Metadata.EditorRouteValues = new RouteValueDictionary {
                        {"Area", "ivNet.Webstore"},
                        {"Controller", "CustomerAdmin"},
                        {"Action", "Edit"},
                        {"Id", context.ContentItem.Id}
                    };
                }
                
                if (context.Metadata.RemoveRouteValues == null)
                {
                    context.Metadata.RemoveRouteValues = new RouteValueDictionary {
                        {"Area", "ivNet.Webstore"},
                        {"Controller", "CustomerAdmin"},
                        {"Action", "Edit"},
                        {"Id", context.ContentItem.Id}
                    };
                }
 
            });
        }
    }
}