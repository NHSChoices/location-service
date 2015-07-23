namespace GoatTrip.RestApi.Controllers {
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class BadRequestResult
        : IHttpActionResult {

        public BadRequestResult(HttpRequestMessage request) {
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {
            HttpError err = new HttpError("");
            return Task.FromResult(_request.CreateResponse(HttpStatusCode.BadRequest, err));
        }

        private readonly HttpRequestMessage _request;
    }
}