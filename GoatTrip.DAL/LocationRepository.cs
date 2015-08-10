using System.Collections.Generic;
using GoatTrip.DAL.DTOs;
using System.Linq;

namespace GoatTrip.DAL
{
    public class LocationRepository
        : ILocationRepository
    {
        private IConnectionManager _connectionManager;

        public LocationRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public IEnumerable<Location> FindLocations(string postCode)
        {
            string statement = "SELECT * FROM locations where POSTCODE_LOCATOR between @postcode and @postcodematch";

            List<Location> locations = new List<Location>();

            using (IManagedDataReader reader = _connectionManager.GetReader(statement, new StatementParamaters() { { "@postcode", postCode}, { "@postcodematch", postCode+ "{" } }))
            {
                while (reader.Read())
                {
                    locations.Add(new Location(reader.DataReader));
                }
            }
            return locations;
        }



        public IEnumerable<Location> FindLocationsbyAddress(string addressLookup) {
            string statement = "SELECT * FROM locations WHERE " +
                               "locationId IN(" +
                               "SELECT docid FROM locations_srch WHERE locations_srch MATCH @addressSearch" +
                               " ORDER BY PAO_START_NUMBER, STREET_DESCRIPTION, TOWN_NAME LIMIT 100)";

            List<Location> locations = new List<Location>();

            using (IManagedDataReader reader = _connectionManager.GetReader(statement, new StatementParamaters() { { "@addressSearch", "'" + addressLookup + "*'" } }))
            {
                while (reader.Read())
                {
                    locations.Add(new Location(reader.DataReader));
                }
            }
            return locations;
        }

        public IEnumerable<LocationGroup> FindLocations(string addressLookup, ILocationGroupingStrategy groupingStrategy) {

            string statement = "SELECT " + LocationQueryField.Concatenate(groupingStrategy.Fields) + ", COUNT(*) as Number  " +
                               "FROM locations WHERE locationId IN(" +
                               "select docid from locations_srch WHERE locations_srch " +
                               "MATCH @addressSearch) " +
                               "GROUP BY " + LocationQueryField.Concatenate(groupingStrategy.Fields) + " " +
                               "ORDER by Number desc LIMIT 100;";

            List<LocationGroup> locations = new List<LocationGroup>();

            using (IManagedDataReader reader = _connectionManager.GetReader(statement, new StatementParamaters() { { "@addressSearch", "'" + addressLookup + "*'" } }))
            {
                while (reader.Read())
                {
                    locations.Add(new LocationGroup(reader.DataReader, groupingStrategy.Fields));
                }
            }
            return locations;

        }

    }

    public class LocationQueryField {

        public string Name { get; private set; }

        public LocationDataField Key { get; private set; }

        private LocationQueryField(string name, LocationDataField key) {
            Name = name;
            Key = key;
        }

        public static LocationQueryField HouseNumber { get { return new LocationQueryField("PAO_START_NUMBER", LocationDataField.HouseNumber); } }

        public static LocationQueryField Town { get { return new LocationQueryField("TOWN_NAME", LocationDataField.Town); } }

        public static LocationQueryField Street { get { return new LocationQueryField("STREET_DESCRIPTION", LocationDataField.Street); } }

        public static LocationQueryField AdministrativeArea { get { return new LocationQueryField("ADMINISTRATIVE_AREA", LocationDataField.AdministrativeArea); } }

        public static LocationQueryField PostCode { get { return new LocationQueryField("POSTCODE", LocationDataField.PostCode); } }

        public static string Concatenate(IEnumerable<LocationQueryField> fields) {
            return fields.Select(f => f.Name).Aggregate((i, j) => i + ',' + j);
        }

    }
}
