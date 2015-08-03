
using System.Web.Http;

namespace GoatTrip.RestApi {
    public static class WebApiConfig {

        public static void Register(HttpConfiguration config) {

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Formatters.Add(new BrowserJsonFormatter(true));
        }
    }
}
