namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using DAL;

    public class LocationsGroupedByAddressStrategy
        : ILocationGroupingStrategy {
        public LocationsGroupedByAddressStrategy() {
            Fields = new List<LocationQueryField> {
                LocationQueryField.Street,
                LocationQueryField.Town,
                LocationQueryField.PostCode,
            };
        }

        public IEnumerable<LocationQueryField> Fields { get; set; }
    }
}