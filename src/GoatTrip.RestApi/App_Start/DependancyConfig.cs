namespace GoatTrip.RestApi {
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;

    public static class DependancyConfig {

        public static void Configure() {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //builder.RegisterWebApiFilterProvider(config);

            builder.RegisterModule(new LocationControllerModule("app_data/test.db"));

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}