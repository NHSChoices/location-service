namespace GoatTrip.RestApi.Controllers {
    public interface ILocationQueryValidator {
        bool IsValid(string query);
    }
}