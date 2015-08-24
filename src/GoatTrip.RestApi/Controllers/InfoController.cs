using System.Web.Http.Cors;

namespace GoatTrip.RestApi.Controllers {
    using System.Web.Http;
    using DAL;

    [RoutePrefix("info")]
    [EnableCors(origins:"*", headers:"*",methods:"*")]
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

    public class InfoModel {
        public bool InMemoryDbInitialised { get; set; }
    }
}