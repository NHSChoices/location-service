
namespace GoatTrip.RestApi {
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Controllers;
    using Models;
    using Services;

    public class DummyDataRetriever
        : ILocationDataRetriever {
        public IEnumerable<LocationModel> RetrieveAll() {
            return new List<LocationModel> {
                new LocationModel {
                    Postcode = "SO111XX"
                },
                new LocationModel {
                    Postcode = "SO222XX"
                },
                new LocationModel {
                    Postcode = "SO222XX"
                },
                new LocationModel {
                    Postcode = "SO222XX"
                }
            };
        }
    }

    public class WebApiApplication : System.Web.HttpApplication {

        protected void Application_Start() {

            var builder = new ContainerBuilder();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //builder.RegisterWebApiFilterProvider(config);

            builder.Register(c => new DummyDataRetriever()).As<ILocationDataRetriever>();
            builder.Register(c => new LocationQueryValidator()).As<ILocationQueryValidator>();
            builder.Register(c => new LocationQuerySanitiser()).As<ILocationQuerySanitiser>();

            builder.Register(c => new LocationService(c.Resolve<ILocationDataRetriever>(), c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationQuerySanitiser>())).As<ILocationService>();
            builder.Register(c => new LocationController(c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationService>()));

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}
