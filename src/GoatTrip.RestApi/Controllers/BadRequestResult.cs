using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GoatTrip.RestApi.Controllers {
    public class BadRequestResult
        : IHttpActionResult {

        public BadRequestResult(HttpRequestMessage request, string query) {
            _request = request;
            _query = query;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {
            HttpError err = new HttpError(string.Format("The query provided ('{0}') is invalid.", _query));
            return Task.FromResult(_request.CreateResponse(HttpStatusCode.BadRequest, err));
        }

        private readonly HttpRequestMessage _request;
        private readonly string _query;
    }
}