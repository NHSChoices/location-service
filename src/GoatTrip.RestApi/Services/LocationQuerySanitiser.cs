using System.Text.RegularExpressions;

namespace GoatTrip.RestApi.Services {
    public class PostcodeQuerySanitiser
        : ILocationQuerySanitiser {
        public string Sanitise(string query) {
            query = query.ToLower();
            return EnsureNoSpaces(query).Trim();
        }

        private static string EnsureNoSpaces(string query) {
            return Regex.Replace(query, @"\s+", "");
        }
    }
}