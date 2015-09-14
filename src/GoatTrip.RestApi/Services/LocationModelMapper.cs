namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using DAL.DTOs;
    using Models;

    public class LocationModelMapper
        : ILocationModelMapper {
        public LocationModel Map(Location location) {

            var address = BuildAddress(location);

            return new LocationModel {
                UPRN = location.UPRN.ToString(),
                AddressLines = address,
                OrganisationName = location.OrganisationName,
                BuildingName = location.BuildingName,
                HouseNumber = location.HouseNumber,
                StreetDescription = location.StreetDescription,
                Locality = location.Locality,
                TownName = location.TownName,
                AdministrativeArea = location.AdministrativeArea,
                PostTown = location.PostalTown,
                Postcode = location.PostCode,
                PostcodeLocator = location.PostcodeLocator,
                Coordinate = new CoordinateModel(location.XCoordinate, location.YCoordinate)
            };
        }

        private IEnumerable<string> BuildAddress(Location location) {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(location.OrganisationName))
                result.Add(location.OrganisationName);

            if (!string.IsNullOrEmpty(location.BuildingName)) {
                if (location.BuildingName != location.OrganisationName)
                    result.Add(location.BuildingName);
            }

            var street = "";

            if (!string.IsNullOrEmpty(location.HouseNumber) && location.HouseNumber != "0") {
                if (location.HouseNumber != location.BuildingName &&
                    location.HouseNumber != location.OrganisationName)
                    street += location.HouseNumber;
            }

            if (!string.IsNullOrEmpty(location.StreetDescription)) {
                if (location.StreetDescription != location.BuildingName &&
                    location.StreetDescription != location.OrganisationName &&
                    location.StreetDescription != location.HouseNumber)
                    street += " " + location.StreetDescription;
            }

            if (!string.IsNullOrEmpty(street))
                result.Add(street.Trim());

            if (!string.IsNullOrEmpty(location.Locality)) {
                if (location.Locality != location.BuildingName &&
                    location.Locality != location.OrganisationName &&
                    location.Locality != location.HouseNumber &&
                    location.Locality != location.StreetDescription)
                    result.Add(location.Locality);
            }

            if (!string.IsNullOrEmpty(location.TownName)) {
                if (location.TownName != location.BuildingName &&
                    location.TownName != location.OrganisationName &&
                    location.TownName != location.HouseNumber &&
                    location.TownName != location.StreetDescription &&
                    location.TownName != location.Locality)
                    result.Add(location.TownName);
            }

            if (!string.IsNullOrEmpty(location.AdministrativeArea)) {
                if (location.AdministrativeArea != location.BuildingName &&
                    location.AdministrativeArea != location.OrganisationName &&
                    location.AdministrativeArea != location.HouseNumber &&
                    location.AdministrativeArea != location.StreetDescription &&
                    location.AdministrativeArea != location.Locality &&
                    location.AdministrativeArea != location.TownName)
                    result.Add(location.AdministrativeArea);
            }

            if (!string.IsNullOrEmpty(location.PostCode)) {
                if (location.PostCode != location.BuildingName &&
                    location.PostCode != location.OrganisationName &&
                    location.PostCode != location.HouseNumber &&
                    location.PostCode != location.StreetDescription &&
                    location.PostCode != location.Locality &&
                    location.PostCode != location.TownName &&
                    location.PostCode != location.AdministrativeArea)
                    result.Add(location.PostCode);
            }

            return result;
        }
    }
}