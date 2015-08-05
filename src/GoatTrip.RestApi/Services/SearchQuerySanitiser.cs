namespace GoatTrip.RestApi.Services {
    public class SearchQuerySanitiser
        : ILocationQuerySanitiser {
        public string Sanitise(string query) {
            query = query.Replace(",", " ");
            return query.Trim();
        }
    }
}