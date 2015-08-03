using System;
using System.Web.Http.Routing;

namespace GoatTrip.RestApi.IntegrationTests {
    public class RouteInfo {
        public Type Controller { get; set; }

        public string Action { get; set; }
        public IHttpRouteData RouteData { get; set; }
    }
}
