
namespace GoatTrip.RestApi.Services {

    using DAL;
    using Models;

    public class LocationRetrievalService
        : ILocationRetrievalService {
        private readonly ILocationIdEncoder _encoder;
        private readonly ILocationModelMapper _locationModelMapper;
        private readonly ILocationRepository _repository;

        public LocationRetrievalService(ILocationRepository repository, ILocationIdEncoder encoder, ILocationModelMapper locationModelMapper) {
            _repository = repository;
            _encoder = encoder;
            _locationModelMapper = locationModelMapper;
        }

        public LocationModel Get(string id) {
            var decodedId = _encoder.Decode(id);
            var location = _repository.Get(decodedId);
            return _locationModelMapper.Map(location);
        }
    }
}