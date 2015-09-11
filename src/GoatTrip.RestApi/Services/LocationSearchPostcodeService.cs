namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using System.Linq;
    using DAL;
    using DAL.DTOs;
    using Models;

    public class LocationSearchPostcodeService
        : LocationSearchBaseService, ILocationSearchPostcodeService {

        public LocationSearchPostcodeService(ILocationRepository repository, ILocationQueryValidator queryValidator, ILocationQuerySanitiser postCodeSanitiser, ILocationModelMapper locationModelMapper)
            : base(queryValidator) {
            _repository = repository;
            _postCodeSanitiser = postCodeSanitiser;
            _locationModelMapper = locationModelMapper;
        }

        public IEnumerable<LocationGroupModel> SearchByPostcode(string postcodeQuery) {
            ValidateAndThrow(postcodeQuery);
            var sanitisedQuery = _postCodeSanitiser.Sanitise(postcodeQuery);

            var results = _repository.FindLocations(sanitisedQuery);

            var groupedResult = Group(results);

            return groupedResult;
        }

        private IEnumerable<LocationGroupModel> Group(IEnumerable<Location> results) {
            var locations = results.Select(l => _locationModelMapper.Map(l));
            return locations.GroupBy(l => l.Postcode)
                .Select(g => new LocationGroupModel(g.First().GroupDescription, g.Count(), "/locations/search/" + g.First().GroupDescription));
        }

        private readonly ILocationRepository _repository;
        private readonly ILocationQuerySanitiser _postCodeSanitiser;
        private readonly ILocationModelMapper _locationModelMapper;
    }
}