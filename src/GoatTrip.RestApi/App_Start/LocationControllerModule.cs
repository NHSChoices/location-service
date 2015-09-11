using Autofac;
using GoatTrip.Common.Formatters;
using GoatTrip.DAL;
using GoatTrip.DAL.DTOs;
using GoatTrip.DAL.Formatters;
using GoatTrip.RestApi.Controllers;
using GoatTrip.RestApi.Services;

namespace GoatTrip.RestApi {
    public class LocationControllerModule
        : Module {


        protected override void Load(ContainerBuilder builder) {

            builder.Register(c => new LocationQueryValidator()).As<ILocationQueryValidator>();
            builder.Register(c => new PostcodeQuerySanitiser()).As<ILocationQuerySanitiser>();
            builder.Register(c => new Base64LocationIdEncoder()).As<ILocationIdEncoder>();
            builder.Register(c => new LocationModelMapper()).As<ILocationModelMapper>();

            builder.Register(c => new LuceneQueryFields()).As<ILocationQueryFields>();
            builder.Register(c => new LocationRetrievalService(c.Resolve<ILocationRepository>(), c.Resolve<ILocationIdEncoder>(), c.Resolve<ILocationModelMapper>())).As<ILocationRetrievalService>();
            builder.Register(c => new LocationSearchPostcodeService(c.Resolve<ILocationRepository>(), c.Resolve<ILocationQueryValidator>(), new PostcodeQuerySanitiser(), c.Resolve<ILocationModelMapper>())).As<ILocationSearchPostcodeService>();
            builder.Register(c => new LocationSearchService(c.Resolve<ILocationGroupRepository>(), c.Resolve<ILocationQueryValidator>(), new SearchQuerySanitiser(), c.Resolve<ILocationQueryFields>(), c.Resolve<ILocationIdEncoder>())).As<ILocationSearchService>();

            builder.Register(c => new LocationController(c.Resolve<ILocationQueryValidator>(), c.Resolve<ILocationRetrievalService>(), c.Resolve<ILocationSearchService>(), c.Resolve<ILocationSearchPostcodeService>(), c.Resolve<ILocationQueryFields>()));
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
            builder.Register(c => new TitleCaseFormatter()).As<IFormatter<string>>();
            builder.Register(c => new LocationFormatConditions()).As<IFormatConditions<string>>();
            builder.Register(c => new LocationDataFieldFormatConditions()).As<IFormatConditions<LocationDataField>>();
            builder.Register(c => new ConditionalFormatter<string, string>(c.Resolve<IFormatter<string>>(), c.Resolve<IFormatConditions<string>>())).As<IConditionalFormatter<string, string>>();
            builder.Register(c => new ConditionalFormatter<string, LocationDataField>(c.Resolve<IFormatter<string>>(), c.Resolve<IFormatConditions<LocationDataField>>())).As<IConditionalFormatter<string, LocationDataField>>();


            builder.Register(c => new LocationGroupBuilder(c.Resolve<IConditionalFormatter<string, LocationDataField>>())).As<ILocationGroupBuilder>();
            builder.Register(c => new ConnectionManager(_databasePath, _discConnectionOnly)).As<IConnectionManager>().SingleInstance();
            builder.Register(c => new LocationRepository(c.Resolve<IConnectionManager>(), c.Resolve<ILocationGroupBuilder>(),c.Resolve<IConditionalFormatter<string,string>>())).As<ILocationRepository>();
        }

        private readonly string _databasePath;
        private readonly bool _discConnectionOnly;
    }


   
}