using System.Text.RegularExpressions;

namespace GoatTrip.RestApi.Services {
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