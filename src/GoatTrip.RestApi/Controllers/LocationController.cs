namespace GoatTrip.RestApi.Controllers {

    using System.Collections.Generic;
    using System.Web.Http;
    using DAL;
    using Services;

    [RoutePrefix(RoutePrefix)]
    public class LocationController
        : ApiController {

        public const string RoutePrefix = "location";
        public const string SearchRoute = "search";
        public const string GetByPostcodeRoute = "postcode";

        public LocationController(ILocationQueryValidator queryValidator, ILocationRetrievalService retrievalService, ILocationSearchService searchService, ILocationSearchPostcodeService searchPostcodeService, ILocationQueryFields locationQueryFields) {
            _queryValidator = queryValidator;
            _retrievalService = retrievalService;
            _searchService = searchService;
            _searchPostcodeService = searchPostcodeService;
            _locationQueryFields = locationQueryFields;
        }

        [Route(SearchRoute + "/{query?}")]
        [HttpGet]
        public IHttpActionResult Search(string query = "") {

            if (!_queryValidator.IsValid(query))
                return new BadRequestResult(Request, query);

            var result = _searchService.Search(query, new LocationsGroupedByAddressStrategy(_locationQueryFields));

            return Ok(result);
        }

        [Route(GetByPostcodeRoute + "/{query?}")]
        public IHttpActionResult GetByPostcode(string query = "") {

            if (!_queryValidator.IsValid(query))
                return new BadRequestResult(Request, query);

            var result = _searchPostcodeService.SearchByPostcode(query);

            return Ok(result);
        }

        [Route("{query?}")]
        public IHttpActionResult Get(string query = "") {

            if (!_queryValidator.IsValid(query))
                return new BadRequestResult(Request, query);

            try {
                var result = _retrievalService.Get(query);

                return Ok(result);
            } catch (LocationNotFoundException) {
                return NotFound();
            }
        }

        private readonly ILocationQueryValidator _queryValidator;
        private readonly ILocationRetrievalService _retrievalService;
        private readonly ILocationSearchService _searchService;
        private readonly ILocationSearchPostcodeService _searchPostcodeService;
        private readonly ILocationQueryFields _locationQueryFields;
    }

    public class LocationsGroupedByAddressStrategy
        : ILocationGroupingStrategy {
        public LocationsGroupedByAddressStrategy(ILocationQueryFields locationQueryFields) {
            Fields = new List<LocationQueryField> {
                locationQueryFields.Street,
                locationQueryFields.Town,
                locationQueryFields.PostCode,
                locationQueryFields.PostCodeLocator
                
            };
        }

        public IEnumerable<LocationQueryField> Fields { get; set; }
    }
}