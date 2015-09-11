using GoatTrip.DAL.DTOs;

namespace GoatTrip.RestApi.Models {
    using System.Collections.Generic;

    public class LocationModel {

        public IEnumerable<string> AddressLines { get; set; }

        public string OrganisationName { get; set; }

        public string BuildingName { get; set; }

        public string StreetDescription { get; set; }
        public string Locality { get; set; }
        public string TownName { get; set; }
        public string AdministrativeArea { get; set; }
        public string PostTown { get; set; }
        public string Postcode { get; set; }
        public string PostcodeLocator { get; set; }

        public CoordinateModel Coordinate { get; set; }
        public string HouseNumber { get; set; }

        public string GroupDescription {
            get {
                var result = "";
                if (!string.IsNullOrEmpty(Postcode))
                    result += Postcode;
                if (!string.IsNullOrEmpty(BuildingName))
                    result += ", " + BuildingName;
                if (!string.IsNullOrEmpty(StreetDescription))
                    result += ", " + StreetDescription;
                if (!string.IsNullOrEmpty(Locality))
                    result += ", " + Locality;
                return result;

            }
        }
    }
}