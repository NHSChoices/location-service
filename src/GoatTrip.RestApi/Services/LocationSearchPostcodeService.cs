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

        public IEnumerable<LocationModel> SearchByPostcode(string postcodeQuery) {
            ValidateAndThrow(postcodeQuery);
            var sanitisedQuery = _postCodeSanitiser.Sanitise(postcodeQuery);

            var results = _repository.FindLocations(sanitisedQuery);

            var groupedResult = Map(results);

            return groupedResult;
        }

        private IEnumerable<LocationModel> Map(IEnumerable<Location> results) {
          var locations = results.Select(l => _locationModelMapper.Map(l)).Take(20);
          return locations;
        }

        private readonly ILocationRepository _repository;
        private readonly ILocationQuerySanitiser _postCodeSanitiser;
        private readonly ILocationModelMapper _locationModelMapper;
    }
}