namespace GoatTrip.RestApi.Services {
    public interface ILocationQueryValidator {
        bool IsValid(string query);
    }
}