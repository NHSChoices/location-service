using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
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

        public IEnumerable<DTOs.Location> FindLocations(string postCode)
        {
            string statement = "SELECT * FROM locations where POSTCODE_LOCATOR like @postcode";

            List<DTOs.Location> locations = new List<Location>();

            using (IManagedDataReader reader = _connectionManager.GetReader(statement, new StatementParamaters() { { "@postcode", postCode + "%" } }))
            {
                while (reader.Read())
                {
                    locations.Add(new Location(reader.DataReader));
                }
            }
            return locations;
        }



        public IEnumerable<Location> FindLocationsbyAddress(string addressLookup) {
            string statement = "SELECT * FROM locations where " +
                               "ORGANISATION_NAME like @address" +
                               " or BUILDING_NAME like @address" +
                               " or PAO_START_NUMBER like @address" +
                               //" or PAO_START_SUFFIX like @address" +
                               " or STREET_DESCRIPTION like @address" +
                               " or LOCALITY like @address" +
                               " or TOWN_NAME like @address" +
                               " or ADMINISTRATIVE_AREA like @address" +
                               " or POST_TOWN like @address";
            1
            List<DTOs.Location> locations = new List<Location>();

            using (IManagedDataReader reader = _connectionManager.GetReader(statement, new StatementParamaters() { { "@address", "%" + addressLookup + "%" } }))
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
