namespace GoatTrip.RestApi.IntegrationTests {

    using System;
    using System.Web.Http.Routing;

    public class RouteInfo {
        public Type Controller { get; set; }

        public string Action { get; set; }
        public IHttpRouteData RouteData { get; set; }
    }
}
