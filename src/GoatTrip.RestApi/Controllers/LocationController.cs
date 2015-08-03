
namespace GoatTrip.RestApi.Controllers {
    using System.Web.Http;
    using Services;

    [RoutePrefix("location")]
    public class LocationController
        : ApiController {

        public LocationController(ILocationQueryValidator queryValidator, ILocationService service) {
            _queryValidator = queryValidator;
            _service = service;
        }

        [Route("address/{query}")]
        public IHttpActionResult GetByAddress(string query = "") {

            if (!_queryValidator.IsValid(query)) {
                return new BadRequestResult(Request, query);
            }

            var result = _service.GetByAddress(query);

            return Ok(result);
        }

        [Route("{query?}")]
        public IHttpActionResult Get(string query = "") {

            if (!_queryValidator.IsValid(query)) {
                return new BadRequestResult(Request, query);
            }

            var result = _service.Get(query);

            return Ok(result);
        }

        private readonly ILocationQueryValidator _queryValidator;
        private readonly ILocationService _service;
    }

}