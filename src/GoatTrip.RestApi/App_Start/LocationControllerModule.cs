using Autofac;
using GoatTrip.DAL;
using GoatTrip.RestApi.Controllers;
using GoatTrip.RestApi.Services;

namespace GoatTrip.RestApi {
    public class LocationControllerModule
        : Module {


        protected override void Load(ContainerBuilder builder) {

        

            builder.Register(c => new LocationQueryValidator()).As<ILocationQueryValidator>();
            builder.Register(c => new PostcodeQuerySanitiser()).As<ILocationQuerySanitiser>();

            builder.Register(c => new LocationService(c.Resolve<ILocationRepository>(), c.Resolve<ILocationQueryValidator>(), new PostcodeQuerySanitiser(), new SearchQuerySanitiser())).As<ILocationService>();

            builder.Register(c => new LocationController(c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationService>()));
        }

    }

    public class ConnectionManagerModule : Module
    {
        public ConnectionManagerModule(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConnectionManager(_databasePath)).As<IConnectionManager>().SingleInstance();
            builder.Register(c => new LocationRepository(c.Resolve<IConnectionManager>())).As<ILocationRepository>();
        }

        private readonly string _databasePath;
    }
}