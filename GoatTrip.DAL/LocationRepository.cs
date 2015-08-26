using System.Collections.Generic;
using GoatTrip.DAL.DTOs;

namespace GoatTrip.DAL
{
    public class LocationRepository
        : ILocationRepository
    {
        private IConnectionManager _connectionManager;
        private ILocationGroupBuilder _locationGroupBuilder;

        public LocationRepository(IConnectionManager connectionManager, ILocationGroupBuilder locationGroupBuilder)
        {
            _connectionManager = connectionManager;
            _locationGroupBuilder = locationGroupBuilder;
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

            var tokenizer = new fTSQueryTokenizer(addressLookup);

            var statement = new FtsQueryGenerator(groupingStrategy, tokenizer).Generate();

            List<LocationGroup> locations = new List<LocationGroup>();

            using (IManagedDataReader reader = _connectionManager.GetReader(statement, new StatementParamaters() { { "@addressSearch", "'" + addressLookup + "*'" } }))
            {
                while (reader.Read())
                {
                    locations.Add(_locationGroupBuilder.Build(reader.DataReader, groupingStrategy.Fields));
                }
            }
            return locations;

        }

    }
}
