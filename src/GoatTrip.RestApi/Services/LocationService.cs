namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class LocationService
        : ILocationService {

        public LocationService(ILocationDataRetriever dataRetriever, ILocationQueryValidator queryValidator, ILocationQuerySanitiser sanitiser) {
            _dataRetriever = dataRetriever;
            _queryValidator = queryValidator;
            _sanitiser = sanitiser;
        }

        public IEnumerable<LocationModel> Get(string query) {

            ValidateAndThrow(query);

            var sanitisedQuery = _sanitiser.Sanitise(query);

            var results = _dataRetriever.RetrieveAll().Where(l => l.Postcode.ToLower().Contains(sanitisedQuery));

            return results;
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