
using GoatTrip.DAL.DTOs;

namespace GoatTrip.RestApi.Models {
    public class LocationGroupModel {

        public LocationGroupModel(LocationGroup group)
        {
            Count = group.LocationsCount;
            Description = group.GroupDescription;
        }

        public LocationGroupModel()
        {
        }

        public string Description { get; set; }
        public int Count { get; set; }
    }
}