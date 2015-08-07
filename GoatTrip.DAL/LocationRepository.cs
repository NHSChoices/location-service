﻿using System.Collections.Generic;
using GoatTrip.DAL.DTOs;

namespace GoatTrip.DAL
{
    public class LocationRepository : ILocationRepository
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


        public IEnumerable<LocationGroup> FindLocationGroupsbyAddress(string addressLookup, LocationGroupByStringBuilder groupBy)
        {
            string statement = "SELECT " + groupBy.ToString() + ", COUNT(*) as Number  " +
                    "FROM locations WHERE locationId IN(" +
                    "select docid from locations_srch WHERE locations_srch " +
                    "MATCH @addressSearch) LIMIT 100 " +
                    "GROUP BY" + groupBy.ToString() + " " +
                    "ORDER by Number desc;";

            List<LocationGroup> locations = new List<LocationGroup>();

            using (IManagedDataReader reader = _connectionManager.GetReader(statement, new StatementParamaters() { { "@addressSearch", "'" + addressLookup + "*'" } }))
            {
                while (reader.Read())
                {
                    locations.Add(new LocationGroup(reader.DataReader, groupBy));
                }
            }
            return locations;
        }
    }
}
