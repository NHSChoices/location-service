using System.Collections.Generic;
using System.Linq;

namespace GoatTrip.DAL
{
    public class LocationQueryField {

        public string Name { get; private set; }

        public LocationDataField Key { get; private set; }

        private LocationQueryField(string name, LocationDataField key) {
            Name = name;
            Key = key;
        }

        public static LocationQueryField HouseNumber { get { return new LocationQueryField("PAO_START_NUMBER", LocationDataField.HouseNumber); } }
        public static LocationQueryField HouseSuffix { get { return new LocationQueryField("PAO_START_SUFFIX", LocationDataField.HouseSuffix); } }
        public static LocationQueryField Town { get { return new LocationQueryField("TOWN_NAME", LocationDataField.Town); } }
        public static LocationQueryField Street { get { return new LocationQueryField("STREET_DESCRIPTION", LocationDataField.Street); } }
        public static LocationQueryField AdministrativeArea { get { return new LocationQueryField("ADMINISTRATIVE_AREA", LocationDataField.AdministrativeArea); } }
        public static LocationQueryField PostCode { get { return new LocationQueryField("POSTCODE", LocationDataField.PostCode); } }

        public static string Concatenate(IEnumerable<LocationQueryField> fields) {
            return fields.Select(f => f.Name).Aggregate((i, j) => i + ',' + j);
        }

    }
}