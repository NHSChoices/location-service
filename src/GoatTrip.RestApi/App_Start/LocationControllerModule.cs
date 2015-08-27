using Autofac;
using GoatTrip.DAL;
using GoatTrip.DAL.Lucene;
using GoatTrip.RestApi.Controllers;
using GoatTrip.RestApi.Services;

namespace GoatTrip.RestApi {
    public class LocationControllerModule
        : Module {


        protected override void Load(ContainerBuilder builder) {

            builder.Register(c => new LocationQueryValidator()).As<ILocationQueryValidator>();
            builder.Register(c => new PostcodeQuerySanitiser()).As<ILocationQuerySanitiser>();
          //  builder.Register(c => new SqlIteLocationQueryFields()).As<ILocationQueryFields>();

            builder.Register(c => new LuceneQueryFields()).As<ILocationQueryFields>();
            builder.Register(c => new LocationService(c.Resolve<ILocationRepository>(), c.Resolve<ILocationGroupRepository>(), c.Resolve<ILocationQueryValidator>(), new PostcodeQuerySanitiser(), new SearchQuerySanitiser(), c.Resolve<ILocationQueryFields>())).As<ILocationService>();

            builder.Register(c => new LocationController(c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationService>(), c.Resolve<ILocationQueryFields>()));
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
            builder.Register(c => new LocationGroupBuilder()).As<ILocationGroupBuilder>();
            builder.Register(c => new ConnectionManager(_databasePath, _discConnectionOnly)).As<IConnectionManager>().SingleInstance();
            builder.Register(c => new LocationRepository(c.Resolve<IConnectionManager>(), c.Resolve<ILocationGroupBuilder>())).As<ILocationRepository>();
        }

        private readonly string _databasePath;
        private readonly bool _discConnectionOnly;
    }


   
}