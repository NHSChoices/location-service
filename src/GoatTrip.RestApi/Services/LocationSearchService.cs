namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using System.Linq;
    using Common.System.Collections.Generic;
    using Controllers;
    using DAL;
    using DAL.DTOs;
    using Models;

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
                return string.Format("/{0}/{1}", LocationController.RoutePrefix, _encoder.Encode(lg.LocationId.ToString()));

            return string.Format("/{0}/{1}/{2}", LocationController.RoutePrefix, LocationController.SearchRoute, lg.GroupDescription);
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
}