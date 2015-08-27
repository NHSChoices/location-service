

namespace GoatTrip.RestApi.Models {
    using DAL.DTOs;

    public class LocationGroupModel {

        public string Description { get; set; }

        public int Count { get; set; }
  
        public string Next { get; set; }

        public LocationGroupModel()
        { }


        public LocationGroupModel(string description, int count, string next) {
            Description = description;
            Count = count;
            Next = next;
        }
    }
}