
using GoatTrip.DAL.DTOs;

namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using System.Linq;
    using DAL;
    using Models;

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

        public IEnumerable<LocationGroupModel> Get(string addressQuery, ILocationGroupingStrategy groupingStrategy) {
            ValidateAndThrow(addressQuery);

            var sanitisedQuery = _searchSanitiser.Sanitise(addressQuery);

            var results = _repository.FindLocations(sanitisedQuery, groupingStrategy);

            var refinedResults = RequeryIfRequired(results, addressQuery, groupingStrategy);

            return refinedResults.Select(lg => new LocationGroupModel(lg));
        }

        private IEnumerable<LocationGroup> RequeryIfRequired(IEnumerable<LocationGroup> results, string addressQuery, ILocationGroupingStrategy groupingStrategy) {

            int locationsCount = 0;
            results.ToList().ForEach(g => locationsCount += g.LocationsCount);

            if (results.Count() != 1 && locationsCount >= 100)
                return results;

            var groupingStrategyBuilder = new LocationGroupingStrategyBuilder(LocationQueryField.HouseNumber)
                .ThenBy(groupingStrategy);
            return _repository.FindLocations(addressQuery, groupingStrategyBuilder.Build());
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
                .Select(g => new LocationGroupModel() {
                    Description = g.First().GroupDescription,
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