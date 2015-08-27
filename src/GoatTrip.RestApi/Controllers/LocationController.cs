namespace GoatTrip.RestApi.Controllers {
    using System.Web.Http;
    using DAL;
    using Services;

    [RoutePrefix("location")]
    public class LocationController
        : ApiController {

        public LocationController(ILocationQueryValidator queryValidator, ILocationService service) {
            _queryValidator = queryValidator;
            _service = service;
        }

        [Route("search/{query?}")]
        [HttpGet]
        public IHttpActionResult Search(string query = "") {

            if (!_queryValidator.IsValid(query))
                return new BadRequestResult(Request, query);

            var result = _service.Search(query, new LocationsGroupedByAddressStrategy());

            return Ok(result);
        }

        [Route("postcode/{query?}")]
        public IHttpActionResult GetByPostcode(string query = "") {

            if (!_queryValidator.IsValid(query))
                return new BadRequestResult(Request, query);

            var result = _service.SearchByPostcode(query);

            return Ok(result);
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id) {

            if (!_queryValidator.IsValid(id))
                return new BadRequestResult(Request, id);

            try {
                var result = _service.Get(id);
                return Ok(result);
            }
            catch (LocationNotFoundException) {
                return NotFound();
            }
        }

        private readonly ILocationQueryValidator _queryValidator;
        private readonly ILocationService _service;
    }
}