
namespace GoatTrip.RestApi.Controllers {
    using System.Web.Http;

    public class LocationController
        : ApiController {

        public IHttpActionResult Get(string query) {

            if (IsBadRequest(query)) {
                return new BadRequestResult(Request);
            }

            return Ok(new object());
        }

        private bool IsBadRequest(string query) {

            if (string.IsNullOrEmpty(query))
                return true;

            if (query.Length <= 0)
                return true;

            return false;
        }
    }
}