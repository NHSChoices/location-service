namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using Models;

    public interface ILocationDataRetriever {
        IEnumerable<LocationModel> RetrieveAll();
    }
}