
namespace GoatTrip.RestApi.IntegrationTests {
    using System.Net.Http;
    using System.Web.Http;
    using Controllers;
    using Xunit;

    public class RoutingTests {
        [Fact]
        [Trait("Category", "integration")]
        public void AllRoutes_Always_RouteToCorrectControllerMethods() {
            // arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/location/");
            var config = new HttpConfiguration();

            // act
            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            // asserts
            Assert.Equal(typeof(LocationController), route.Controller);
            Assert.Equal("Get", route.Action);
        }
    }
}
