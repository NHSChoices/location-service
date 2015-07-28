namespace GoatTrip.RestApi.Models {
    using System.Collections.Generic;

    public class LocationGroupModel {

        public string Description { get; set; }
        public IEnumerable<LocationModel> Locations { get; set; }
        public int Count { get; set; }
    }
}