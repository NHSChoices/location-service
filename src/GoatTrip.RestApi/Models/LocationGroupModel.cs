using System.Collections.Generic;

namespace GoatTrip.RestApi.Models {
    public class LocationGroupModel {

        public string Description { get; set; }
        public IEnumerable<LocationModel> Locations { get; set; }
        public int Count { get; set; }
    }
}