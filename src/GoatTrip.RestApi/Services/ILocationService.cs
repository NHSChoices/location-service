using System.Collections.Generic;
using GoatTrip.DAL;
using GoatTrip.RestApi.Models;

namespace GoatTrip.RestApi.Services {
    public interface ILocationService {

        LocationModel Get(string id);
        IEnumerable<LocationGroupModel> Search(string query, ILocationGroupingStrategy groupingStrategy);
        IEnumerable<LocationGroupModel> SearchByAddress(string addressQuery);
        IEnumerable<LocationGroupModel> SearchByPostcode(string postcodeQuery);
    }
}