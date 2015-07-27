namespace GoatTrip.RestApi.Services {

    public class LocationQueryValidator
        : ILocationQueryValidator {

        public bool IsValid(string query) {

            if (!IsMinumumLength(query))
                return false;

            return true;
        }

        private static bool IsMinumumLength(string query) {
            if (string.IsNullOrEmpty(query))
                return false;

            return query.Length >= 1;
        }
    }
}