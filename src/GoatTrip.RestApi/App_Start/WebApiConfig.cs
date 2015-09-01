using System.Web.Http;

namespace GoatTrip.RestApi {
//    using log4net;
    //using log4net.Config;

    public static class WebApiConfig {

        public static void Register(HttpConfiguration config) {

            // Web API configuration and services
      //      XmlConfigurator.Configure();
        //    var log = LogManager.GetLogger(typeof (LoggingHandler));
          //  config.MessageHandlers.Add(new LoggingHandler(log));

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Formatters.Add(new BrowserJsonFormatter(true));
        }
    }
}
