namespace GoatTrip.RestApi.Services {
    public interface ILocationQuerySanitiser {
        string Sanitise(string query);
    }
}