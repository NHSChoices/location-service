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
                               ")";

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
    }
}
