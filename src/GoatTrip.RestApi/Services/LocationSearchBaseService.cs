namespace GoatTrip.RestApi.Services {
    public abstract class LocationSearchBaseService {

        protected LocationSearchBaseService(ILocationQueryValidator queryValidator) {
            _queryValidator = queryValidator;
        }

        protected void ValidateAndThrow(string query) {
            if (!_queryValidator.IsValid(query))
                throw new InvalidLocationQueryException();
        }

        private readonly ILocationQueryValidator _queryValidator;
    }
}