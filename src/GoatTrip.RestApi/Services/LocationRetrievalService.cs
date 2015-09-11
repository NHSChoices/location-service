
namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using System.Linq;
    using DAL.DTOs;
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

    public interface ILocationModelMapper {
        LocationModel Map(Location location);
    }

    public class LocationModelMapper : ILocationModelMapper {
        public LocationModel Map(Location location) {
            return new LocationModel {
                OrganisationName = location.OrganisationName,
                BuildingName = location.BuildingName,
                HouseNumber = location.HouseNumber,
                StreetDescription = location.StreetDescription,
                Locality = location.Localiry,
                TownName = location.TownName,
                AdministrativeArea = location.AdministrativeArea,
                PostTown = location.PostalTown,
                Postcode = location.PostCode,
                PostcodeLocator = location.PostcodeLocator,
                Coordinate = new CoordinateModel(location.XCoordinate, location.YCoordinate)
            };
        }
    }

    public static class IEnumerableOfLocationGroupExtensions
    {
        public static bool HasSingleGroup(this IEnumerable<LocationGroup> operand)
        {
            return operand.Count() == 1;
        }
    }

}