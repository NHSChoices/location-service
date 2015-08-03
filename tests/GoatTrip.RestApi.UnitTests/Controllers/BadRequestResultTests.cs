using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using GoatTrip.RestApi.Controllers;
using Moq;
using Xunit;

namespace GoatTrip.RestApi.UnitTests.Controllers {
    public class BadRequestResultTests {

        [Fact]
        public void ExecuteAsync_Always_Returns400StatusCode() {

            var mockRequest = new Mock<HttpRequestMessage>();
            mockRequest.Object.SetConfiguration(new HttpConfiguration());
            var sut = new BadRequestResult(mockRequest.Object, "query");

            var response = sut.ExecuteAsync(new CancellationToken(false));

            Assert.Equal(HttpStatusCode.BadRequest, response.Result.StatusCode);
        }
    }
}