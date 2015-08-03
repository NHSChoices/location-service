using System;
using System.IO;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace GoatTrip.RestApi {
    public static class DependancyConfig {

        public static void Configure() {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //builder.RegisterWebApiFilterProvider(config);
            var dbPath = WebConfigurationManager.AppSettings["sqLiteDb"].Replace(@"~\", "");


            builder.RegisterModule(new ConnectionManagerModule(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbPath)));
            builder.RegisterModule(new LocationControllerModule());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}