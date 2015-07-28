namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using Models;

    public interface ILocationService {
        IEnumerable<LocationGroupModel> Get(string query);
    }
}