namespace GoatTrip.DAL.DTOs {
    using System.Collections.Generic;

    public class LocationGroup
    {
        public long UPRN { get; set; }
        public int LocationsCount { get; set; }
        public string GroupDescription { get; set; }

        public Dictionary<LocationDataField, string> GroupFields { get;  set; }

        public string FirstOrderByField {
            get {
                var fields = GroupDescription.Split(',');
                if (fields.Length > 1)
                    return fields[1];

                return GroupDescription;
            }
        }

        public string SecondOrderByField {
            get {
                return GroupDescription.Split(',')[0];
            }
        }

    }
}
