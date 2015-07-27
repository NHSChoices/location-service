namespace GoatTrip.RestApi.Models {

    public class CoordinateModel {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class LocationModel {

        public string OrganisationName { get; set; }
        /// <summary>
        /// Basic land and property unit
        /// </summary>
        public string BlpuOrganisation { get; set; }
        public string BuildingName { get; set; }
        /// <summary>
        /// Primary addressable object
        /// </summary>
        public string PaoText { get; set; }
        /// <summary>
        /// Secondary addressable object
        /// </summary>
        public string SaoText { get; set; }

        public string StreetDescription { get; set; }
        public string Thoroughfare { get; set; }
        public string DependentThoroughfare { get; set; }
        public string Locality { get; set; }
        public string TownName { get; set; }
        public string AdministrativeArea { get; set; }
        public string PostTown { get; set; }
        public string Postcode { get; set; }
        public string PostcodeLocator { get; set; }

        public CoordinateModel Coordinate { get; set; }
    }
}