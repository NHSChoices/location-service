namespace GoatTrip.RestApi.Services {
    using DAL.DTOs;
    using Models;

    public interface ILocationModelMapper {
        LocationModel Map(Location location);
    }
}