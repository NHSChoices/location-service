using System.Collections.Generic;
using GoatTrip.DAL;
using GoatTrip.RestApi.Models;

namespace GoatTrip.RestApi.Services {
    public interface ILocationService {
        IEnumerable<LocationGroupModel> Get(string query);
        IEnumerable<LocationGroupModel> Get(string query, ILocationGroupingStrategy groupingStrategy);
        IEnumerable<LocationGroupModel> GetByAddress(string addressQuery);

    }
}