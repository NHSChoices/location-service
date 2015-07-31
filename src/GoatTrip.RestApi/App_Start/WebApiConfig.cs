
namespace GoatTrip.RestApi {
    using System.Net.Http.Headers;
    using System.Web.Http;

    public static class WebApiConfig {

        public static void Register(HttpConfiguration config) {

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Formatters.Add(new BrowserJsonFormatter(true));
        }
    }
}
