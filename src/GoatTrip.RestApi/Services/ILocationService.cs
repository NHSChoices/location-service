using System.Collections.Generic;
using GoatTrip.RestApi.Models;

namespace GoatTrip.RestApi.Services {
    public interface ILocationService {
        IEnumerable<LocationGroupModel> Get(string query);
        IEnumerable<LocationGroupModel> GetByAddress(string addressQuery);
        IEnumerable<LocationGroupModel> GetByPostcode(string postcodeQuery);
    }
}