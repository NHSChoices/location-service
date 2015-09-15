namespace GoatTrip.RestApi.Controllers {
    using System.Web.Http;
    using DAL;
    using Models;

    [RoutePrefix("info")]
    public class InfoController
        : ApiController {

        public InfoController(IConnectionManager connectionManager) {
            _connectionManager = connectionManager;
        }

        [Route("")]

        public IHttpActionResult Get() {

            var model = new InfoModel {
                InMemoryDbInitialised = _connectionManager.InMemoryDbInitialised
            };
            return Ok(model);
        }

        private readonly IConnectionManager _connectionManager;
    }
}