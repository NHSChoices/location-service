
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using GoatTrip.RestApi.Controllers;
using Xunit;

namespace GoatTrip.RestApi.IntegrationTests {
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
        public void LocationGet_WithQuery_RoutesCorrectly() {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/location/so666xx");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            Assert.Equal(typeof(LocationController), route.Controller);
            Assert.Equal("Get", route.Action);
            Assert.Equal("so666xx", route.RouteData.Values.First().Value);
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

        [Fact]
        public void InfoGet_Always_RoutesCorrectly() {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/info");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            Assert.Equal(typeof(InfoController), route.Controller);
            Assert.Equal("Get", route.Action);

        }
    }
}