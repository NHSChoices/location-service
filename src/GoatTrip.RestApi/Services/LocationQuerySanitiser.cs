namespace GoatTrip.RestApi.Services {
    using System.Text.RegularExpressions;

    public class LocationQuerySanitiser
        : ILocationQuerySanitiser {
        public string Sanitise(string query) {
            query = query.ToLower();
            return EnsureSingleSpace(query).Trim();
        }

        private static string EnsureSingleSpace(string query) {
            return Regex.Replace(query, @"\s+", " ");
        }
    }
}