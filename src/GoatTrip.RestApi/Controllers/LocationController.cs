using System.Web.Http.Cors;

namespace GoatTrip.RestApi.Controllers {
    using System.Collections.Generic;
using System.Web.Http;
    using DAL;
    using Services;

    [RoutePrefix("location")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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

            var result = _service.Get(query, new LocationsGroupedByAddressStrategy());

            return Ok(result);
        }

        [Route("{query?}")]
        public IHttpActionResult Get(string query = "") {

            if (!_queryValidator.IsValid(query))
                return new BadRequestResult(Request, query);

            var result = _service.Get(query);

            return Ok(result);
        }

        private readonly ILocationQueryValidator _queryValidator;
        private readonly ILocationService _service;
    }

    public class LocationsGroupedByAddressStrategy
        : ILocationGroupingStrategy {
        public LocationsGroupedByAddressStrategy() {
            Fields = new List<LocationQueryField> {
                LocationQueryField.Street,
                LocationQueryField.Town,
                LocationQueryField.PostCode,
            };
        }

        public IEnumerable<LocationQueryField> Fields { get; set; }
    }
}