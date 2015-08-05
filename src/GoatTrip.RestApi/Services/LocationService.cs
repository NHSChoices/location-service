using System.Collections.Generic;
using System.Linq;
using GoatTrip.DAL;
using GoatTrip.RestApi.Models;

namespace GoatTrip.RestApi.Services {
    public class LocationService
        : ILocationService {

        public LocationService(ILocationRepository repository, ILocationQueryValidator queryValidator, ILocationQuerySanitiser postCodeSanitiser, ILocationQuerySanitiser searchSanitiser) {
            _repository = repository;
            _queryValidator = queryValidator;
            _postCodeSanitiser = postCodeSanitiser;
            _searchSanitiser = searchSanitiser;
        }

        public IEnumerable<LocationGroupModel> Get(string query) {
            ValidateAndThrow(query);

            var sanitisedQuery = _postCodeSanitiser.Sanitise(query);

            var results = _repository.FindLocations(sanitisedQuery);

            var locations = results.Select(l => new LocationModel(l));

            var groupedResult = Group(locations);

            return groupedResult;
        }

        public IEnumerable<LocationGroupModel> GetByAddress(string addressQuery) {
            ValidateAndThrow(addressQuery);

            var sanitisedQuery = _searchSanitiser.Sanitise(addressQuery);

            var results = _repository.FindLocationsbyAddress(sanitisedQuery);

            var locations = results.Select(l => new LocationModel(l));

            var groupedResult = Group(locations);

            return groupedResult;
        }

        private IEnumerable<LocationGroupModel> Group(IEnumerable<LocationModel> locations) {
            return locations.GroupBy(l => l.Postcode)
                .Select(g => new LocationGroupModel {
                    Description = g.First().GroupDescription,
                    Locations = g.ToList(), 
                    Count = g.Count()
                });
        }

        private void ValidateAndThrow(string query) {
            if (!_queryValidator.IsValid(query))
                throw new InvalidLocationQueryException();
        }

        private readonly ILocationRepository _repository;
        private readonly ILocationQueryValidator _queryValidator;
        private readonly ILocationQuerySanitiser _postCodeSanitiser;
        private readonly ILocationQuerySanitiser _searchSanitiser;
    }
}