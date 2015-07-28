namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Models;

    public class LocationService
        : ILocationService {

        public LocationService(ILocationDataRetriever dataRetriever, ILocationQueryValidator queryValidator, ILocationQuerySanitiser sanitiser) {
            _dataRetriever = dataRetriever;
            _queryValidator = queryValidator;
            _sanitiser = sanitiser;
        }

        public IEnumerable<LocationGroupModel> Get(string query) {

            ValidateAndThrow(query);

            var sanitisedQuery = _sanitiser.Sanitise(query);

            var results = _dataRetriever.RetrieveAll().Where(l => l.Postcode.ToLower().Contains(sanitisedQuery));

            var groupedResult = Group(results);

            return groupedResult;
        }

        private IEnumerable<LocationGroupModel> Group(IEnumerable<LocationModel> locations) {
            return locations.GroupBy(l => l.Locality)
                .Select(g => new LocationGroupModel {
                    Description = g.Key, 
                    Locations = g.ToList(), 
                    Count = g.Count()
                });
        }

        private void ValidateAndThrow(string query) {
            if (!_queryValidator.IsValid(query))
                throw new InvalidLocationQueryException();
        }

        private readonly ILocationDataRetriever _dataRetriever;
        private readonly ILocationQueryValidator _queryValidator;
        private readonly ILocationQuerySanitiser _sanitiser;
    }
}