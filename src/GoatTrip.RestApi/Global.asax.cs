
using System.Web;
using System.Web.Http;

namespace GoatTrip.RestApi {
    public class WebApiApplication : HttpApplication {

        protected void Application_Start() {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            DependancyConfig.Configure();
        }
    }
}
