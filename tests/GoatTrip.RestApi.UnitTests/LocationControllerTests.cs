
namespace GoatTrip.RestApi.UnitTests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using Controllers;
    using Xunit;

    public class LocationControllerTests
    {
        private readonly LocationController _sut;

        public LocationControllerTests() {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.SetConfiguration(new HttpConfiguration());

            _sut = new LocationController {
                Request = httpRequestMessage
            };

        }


        [Fact]
        public void Get_WithLessThanOneCharacter_ReturnsBadRequest()
        {
            var result = _sut.Get("");
            var response = result.ExecuteAsync(new CancellationToken(false)).Result;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void Get_WithAtLeastOneCharacter_DoesntReturnBadRequest() {
            var result = _sut.Get("x");
            var response = result.ExecuteAsync(new CancellationToken(false)).Result;

            Assert.NotEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
