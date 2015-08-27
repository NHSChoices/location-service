using System.Collections.Generic;
using GoatTrip.DAL.DTOs;

namespace GoatTrip.DAL
{
    public interface ILocationGroupRepository
    {
        IEnumerable<LocationGroup> FindGroupedLocations(string query, ILocationGroupingStrategy groupingStrategy);
    }
}