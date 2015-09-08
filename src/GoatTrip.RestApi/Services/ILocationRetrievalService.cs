namespace GoatTrip.RestApi.Services {
    using Models;

    public interface ILocationRetrievalService {
        LocationModel Get(string id);
    }
}