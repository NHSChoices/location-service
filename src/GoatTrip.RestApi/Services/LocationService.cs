
namespace GoatTrip.RestApi.Services {

    using DAL.DTOs;
    using System.Collections.Generic;
    using System.Linq;
    using DAL;
    using Models;

    public class LocationRetrievalService
        : ILocationRetrievalService {
        private ILocationIdEncoder _encoder;
        private ILocationRepository _repository;

        public LocationRetrievalService(ILocationRepository repository, ILocationIdEncoder encoder) {
            _repository = repository;
            _encoder = encoder;
        }

        public LocationModel Get(string id) {
            var decodedId = _encoder.Decode(id);
            var location = _repository.Get(decodedId);
            return new LocationModel(location);
        }

    }

    public abstract class LocationSearchBaseService {

        protected LocationSearchBaseService(ILocationQueryValidator queryValidator) {
            _queryValidator = queryValidator;
        }

        protected void ValidateAndThrow(string query) {
            if (!_queryValidator.IsValid(query))
                throw new InvalidLocationQueryException();
        }

        private readonly ILocationQueryValidator _queryValidator;
    }

    public class LocationSearchPostcodeService
        : LocationSearchBaseService, ILocationSearchPostcodeService {
        private readonly ILocationRepository _repository;
        private readonly ILocationQuerySanitiser _postCodeSanitiser;

        public LocationSearchPostcodeService(ILocationRepository repository,
            ILocationQueryValidator queryValidator, ILocationQuerySanitiser postCodeSanitiser)
            : base(queryValidator) {
            _repository = repository;
            _postCodeSanitiser = postCodeSanitiser;
        }

        public IEnumerable<LocationGroupModel> SearchByPostcode(string postcodeQuery) {
            ValidateAndThrow(postcodeQuery);
            var sanitisedQuery = _postCodeSanitiser.Sanitise(postcodeQuery);

            var results = _repository.FindLocations(sanitisedQuery);

            var groupedResult = Group(results);

            return groupedResult;
        }

        private IEnumerable<LocationGroupModel> Group(IEnumerable<Location> results) {
            var locations = results.Select(l => new LocationModel(l));
            return locations.GroupBy(l => l.Postcode)
                .Select(g => new LocationGroupModel(g.First().GroupDescription, g.Count(), "/locations/search/" + g.First().GroupDescription));
        }
    }

    public class LocationSearchService
        : LocationSearchBaseService, ILocationSearchService {

        public const int GROUPING_THRESHOLD = 100; //if the total number of returned locations is lower than this figure the results will be de-grouped by requerying the dataset.

        public LocationSearchService(ILocationGroupRepository groupRepository, ILocationQueryValidator queryValidator, ILocationQuerySanitiser searchSanitiser, ILocationQueryFields locationQueryFields, ILocationIdEncoder encoder)
            : base(queryValidator) {
            _searchSanitiser = searchSanitiser;
            _locationQueryFields = locationQueryFields;
            _encoder = encoder;
            _groupRepository = groupRepository;
        }

        public IEnumerable<LocationGroupModel> Search(string addressQuery, ILocationGroupingStrategy groupingStrategy) {
            ValidateAndThrow(addressQuery);
            var sanitisedQuery = _searchSanitiser.Sanitise(addressQuery);

            var results = _groupRepository.FindGroupedLocations(sanitisedQuery, groupingStrategy);

            var refinedResults = RequeryIfRequired(results.ToList(), sanitisedQuery, groupingStrategy);
            return refinedResults.Select(lg => new LocationGroupModel(lg.GroupDescription, lg.LocationsCount, BuildNextUri(lg)));
        }

        private string BuildNextUri(LocationGroup lg) {
            if (lg.LocationsCount == 1)
                return "/location/" + _encoder.Encode(lg.LocationId.ToString());

            return "/location/search/" + lg.GroupDescription;
        }

        private IEnumerable<LocationGroup> RequeryIfRequired(ICollection<LocationGroup> results, string addressQuery, ILocationGroupingStrategy groupingStrategy) {

            var locationsSum = results.Sum(g => g.LocationsCount);

            if (!results.Any() || locationsSum == 0)
                return results;

            if (results.HasSingleGroup() || locationsSum < GROUPING_THRESHOLD) {
                var groupingStrategyBuilder = new LocationGroupingStrategyBuilder(_locationQueryFields.PrimaryText)
                    .ThenBy(_locationQueryFields.SecondaryText)
                    .ThenBy(_locationQueryFields.HouseNumber)
                    .ThenBy(groupingStrategy);

                return _groupRepository.FindGroupedLocations(addressQuery, groupingStrategyBuilder.Build());
            }

            return results;
        }

        private readonly ILocationQuerySanitiser _searchSanitiser;
        private readonly ILocationIdEncoder _encoder;
        private readonly ILocationQueryFields _locationQueryFields;
        private readonly ILocationGroupRepository _groupRepository;
    }

    public static class IEnumerableOfLocationGroupExtensions {
        public static bool HasSingleGroup(this IEnumerable<LocationGroup> operand) {
            return operand.Count() == 1;
        }
    }
}