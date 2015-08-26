namespace GoatTrip.RestApi.Controllers {
    using System.Collections.Generic;
using System.Web.Http;
    using DAL;
    using Services;

    [RoutePrefix("location")]
    public class LocationController
        : ApiController {

        public LocationController(ILocationQueryValidator queryValidator, ILocationService service, ILocationQueryFields locationQueryFields) {
            _queryValidator = queryValidator;
            _service = service;
            _locationQueryFields = locationQueryFields;
        }

        [Route("search/{query?}")]
        [HttpGet]
        public IHttpActionResult Search(string query = "") {

            if (!_queryValidator.IsValid(query))
                return new BadRequestResult(Request, query);

            var result = _service.Get(query, new LocationsGroupedByAddressStrategy(_locationQueryFields));

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
        private readonly ILocationQueryFields _locationQueryFields;
        }

    public class LocationsGroupedByAddressStrategy
        : ILocationGroupingStrategy {
        public LocationsGroupedByAddressStrategy(ILocationQueryFields locationQueryFields) {
            Fields = new List<SqLiteQueryField> {
                locationQueryFields.Street,
                locationQueryFields.Town,
                locationQueryFields.PostCode,
            };
        }

        public IEnumerable<SqLiteQueryField> Fields { get; set; }
    }
}