namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DAL;
    using DAL.DTOs;
    using Models;

    public class LocationService
        : ILocationService {

        public LocationService(ILocationRepository repository, ILocationQueryValidator queryValidator, ILocationQuerySanitiser sanitiser) {
            _repository = repository;
            _queryValidator = queryValidator;
            _sanitiser = sanitiser;
        }

        public IEnumerable<LocationGroupModel> Get(string query) {

            ValidateAndThrow(query);

            var sanitisedQuery = _sanitiser.Sanitise(query);

            var results = _repository.FindLocations(sanitisedQuery);

            var locations = Map(results);

            var groupedResult = Group(locations);

            return groupedResult;
        }

        private IEnumerable<LocationModel> Map(IEnumerable<Location> locations) {
            return locations.Select(l => new LocationModel {
                OrganisationName = l.OrganisationName,
                //BlpuOrganisation = l.,
                BuildingName = l.BuildingName,
                //PaoText= l.,
                //SaoText= l.OrganisationName,
                //Thoroughfare= l.,
                //DependentThoroughfare= l.dep,
                StreetDescription = l.StreetDescription,
                Locality = l.Localiry,
                TownName = l.TownName,
                AdministrativeArea = l.AdministrativeArea,
                PostTown = l.PostalTown,
                Postcode = l.PostCode,
                //PostcodeLocator= l.po,
                Coordinate = new CoordinateModel((int) l.XCoordinate, (int) l.YCoordinate)
            });
        }

        private IEnumerable<LocationGroupModel> Group(IEnumerable<LocationModel> locations) {
            return locations.GroupBy(l => l.Locality)
                .Select(g => new LocationGroupModel {
                    Description = g.Key, 
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
        private readonly ILocationQuerySanitiser _sanitiser;
    }
}