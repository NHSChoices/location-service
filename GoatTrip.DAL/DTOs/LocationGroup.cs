

namespace GoatTrip.DAL.DTOs {

    using System;
    using System.Data;
    using System.Linq;
    using System.Collections.Generic;

    public class LocationGroup
    {
        public int LocationsCount { get; set; }
        public string GroupDescription { get;  set; }
        public Dictionary<LocationDataField, string> GroupFields { get;  set; }
    }
}
