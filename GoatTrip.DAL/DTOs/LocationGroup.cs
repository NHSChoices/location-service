namespace GoatTrip.DAL.DTOs {
    using System.Collections.Generic;

    public class LocationGroup
    {
        public long UPRN { get; set; }
        public int LocationsCount { get; set; }
        public string GroupDescription { get; set; }

        public Dictionary<LocationDataField, string> GroupFields { get;  set; }
    }
}
