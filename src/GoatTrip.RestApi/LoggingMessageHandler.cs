

namespace GoatTrip.RestApi {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    //using log4net;

    public class LoggingHandler
        : DelegatingHandler {

        /*public LoggingHandler(ILog log) {
            _log = log;
        }*/

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken) {
            LogRequest(request);

            return base.SendAsync(request, cancellationToken).ContinueWith(task => {
                var response = task.Result;

                LogResponse(response);

                return response;
            }, cancellationToken);
        }

        private void LogRequest(HttpRequestMessage request) {
            (request.Content ?? new StringContent("")).ReadAsStringAsync().ContinueWith(x => {
                /*_log.Info(string.Format("{4:yyyy-MM-dd HH:mm:ss} {5} {0} request [{1}]{2} - {3}",
                    request.GetCorrelationId(), request.Method, request.RequestUri, x.Result, DateTime.Now,
                    Username(request)));*/
            });
        }

        private void LogResponse(HttpResponseMessage response) {
            var request = response.RequestMessage;
            (response.Content ?? new StringContent("")).ReadAsStringAsync().ContinueWith(x => {
                /*_log.Info(string.Format("{3:yyyy-MM-dd HH:mm:ss} {4} {0} response [{1}] - {2}",
                    request.GetCorrelationId(), response.StatusCode, x.Result, DateTime.Now, Username(request)));*/
            });
        }

        private string Username(HttpRequestMessage request) {
            var values = new List<string>().AsEnumerable();
            if (request.Headers.TryGetValues("my-custom-header-for-current-user", out values) == false)
                return "<anonymous>";

            return values.First();
        }

        //private readonly ILog _log;
    }
}