namespace GoatTrip.RestApi {
    using Autofac;
    using Controllers;
    using Services;

    public class LocationControllerModule
        : Module {

        protected override void Load(ContainerBuilder builder) {

            builder.Register(c => new DummyDataRetriever()).As<ILocationDataRetriever>();
            builder.Register(c => new LocationQueryValidator()).As<ILocationQueryValidator>();
            builder.Register(c => new LocationQuerySanitiser()).As<ILocationQuerySanitiser>();

            builder.Register(c => new LocationService(c.Resolve<ILocationDataRetriever>(), c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationQuerySanitiser>())).As<ILocationService>();

            builder.Register(c => new LocationController(c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationService>()));
        }
    }
}