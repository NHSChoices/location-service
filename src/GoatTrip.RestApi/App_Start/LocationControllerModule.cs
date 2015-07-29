namespace GoatTrip.RestApi {
    using Autofac;
    using Controllers;
    using DAL;
    using Services;

    public class LocationControllerModule
        : Module {

        public LocationControllerModule(string databasePath) {
            _databasePath = databasePath;
        }

        protected override void Load(ContainerBuilder builder) {

            builder.Register(c => new ConnectionManager(_databasePath)).As<IConnectionManager>();
            builder.Register(c => new LocationRepository(c.Resolve<IConnectionManager>())).As<ILocationRepository>();

            builder.Register(c => new LocationQueryValidator()).As<ILocationQueryValidator>();
            builder.Register(c => new LocationQuerySanitiser()).As<ILocationQuerySanitiser>();

            builder.Register(c => new LocationService(c.Resolve<ILocationRepository>(), c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationQuerySanitiser>())).As<ILocationService>();

            builder.Register(c => new LocationController(c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationService>()));
        }

        private readonly string _databasePath;
    }
}