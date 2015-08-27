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
            builder.Register(c => new Base64LocationIdEncoder()).As<ILocationIdEncoder>();

            builder.Register(c => new LocationService(c.Resolve<ILocationRepository>(), c.Resolve<ILocationQueryValidator>(), new PostcodeQuerySanitiser(), new SearchQuerySanitiser(), c.Resolve<ILocationIdEncoder>())).As<ILocationService>();

            builder.Register(c => new LocationController(c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationService>()));
            builder.Register(c => new InfoController(c.Resolve<IConnectionManager>()));
        }

    }

    public class ConnectionManagerModule : Module
    {
        public ConnectionManagerModule(string databasePath, bool useDiskConnectionOnly)
        {
            _databasePath = databasePath;
            _discConnectionOnly = useDiskConnectionOnly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConnectionManager(_databasePath, _discConnectionOnly)).As<IConnectionManager>().SingleInstance();
            builder.Register(c => new LocationRepository(c.Resolve<IConnectionManager>())).As<ILocationRepository>();
        }

        private readonly string _databasePath;
        private readonly bool _discConnectionOnly;
    }
}