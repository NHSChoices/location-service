
namespace GoatTrip.RestApi.IntegrationTests {
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using Controllers;
    using Xunit;

    [Trait("Category", "integration")]
    public class RoutingTests
    {
        [Fact]
        public void LocationGet_WithoutQuery_RoutesCorrectly() {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/location/");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            Assert.Equal(typeof(LocationController), route.Controller);
            Assert.Equal("Get", route.Action);
        }

        [Fact]
        public void LocationGetByAddress_WithQuery_RoutesCorrectly() {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/location/address/mill way");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            Assert.Equal(typeof (LocationController), route.Controller);
            Assert.Equal("GetByAddress", route.Action);
            Assert.Equal("mill way", route.RouteData.Values.First().Value);

        }

    }
}