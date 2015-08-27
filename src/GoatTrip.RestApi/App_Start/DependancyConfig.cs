using System;
using System.IO;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using GoatTrip.DAL.Lucene;

namespace GoatTrip.RestApi {
    public static class DependancyConfig {

        public static void Configure() {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //builder.RegisterWebApiFilterProvider(config);
            var dbPath = WebConfigurationManager.AppSettings["sqLiteDb"].Replace(@"~\", "");
            bool useDiskConnOnlyValue;
            if (!bool.TryParse(WebConfigurationManager.AppSettings["useDiskConnOnly"], out useDiskConnOnlyValue))
            {
                throw new InvalidOperationException("Invalid useDiskCOnnOnly in web.config");
            }

            var lucenceIndeDirectory = WebConfigurationManager.AppSettings["lucenceIndeDirectory"].Replace(@"~\", "");

            builder.RegisterModule(new LuceneIndexModule(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, lucenceIndeDirectory)));

            builder.RegisterModule(new ConnectionManagerModule(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbPath), useDiskConnOnlyValue));
            builder.RegisterModule(new LocationControllerModule());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}