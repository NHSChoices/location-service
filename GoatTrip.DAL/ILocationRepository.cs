using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public interface ILocationRepository
    {
        IEnumerable<DTOs.Location> FindLocations(string postCode);
    }
}
