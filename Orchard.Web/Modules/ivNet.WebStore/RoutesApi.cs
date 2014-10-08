
using System.Collections.Generic;
using System.Web.Http;
using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;

namespace ivNet.WebStore
{
    public class RoutesApi : IHttpRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            var rdl = new List<RouteDescriptor>();
            rdl.AddRange(Routes());
            return rdl;
        }

        private IEnumerable<RouteDescriptor> Routes()
        {
            return new[]
            {
                #region default              

                new HttpRouteDescriptor
                {
                    RouteTemplate = "api/store/{controller}/{id}",
                    Defaults = new
                    {
                        area = "ivNet.WebStore",
                        id = RouteParameter.Optional
                    }
                }

                #endregion                             
            };
        }
    }
}