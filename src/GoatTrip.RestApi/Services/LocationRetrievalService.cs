
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

    public static class IEnumerableOfLocationGroupExtensions {
        public static bool HasSingleGroup(this IEnumerable<LocationGroup> operand) {
            return operand.Count() == 1;
        }
    }
}