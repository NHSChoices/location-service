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
            string statement = "SELECT * FROM locations where POSTCODE_LOCATOR between @postcode and @postcodematch";

            List<DTOs.Location> locations = new List<Location>();

            using (IManagedDataReader reader = _connectionManager.GetReader(statement, new StatementParamaters() { { "@postcode", postCode.ToUpper()}, { "@postcodematch", postCode.ToUpper() + "{" } }))
            {
                while (reader.Read())
                {
                    locations.Add(new Location(reader.DataReader));
                }
            }
            return locations;
        }



        public IEnumerable<Location> FindLocationsbyAddress(string addressLookup)
        {
            throw new NotImplementedException();
        }
    }
}
