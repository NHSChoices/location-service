

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
        public void LocationGet_WithId_RoutesCorrectly() {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/location/id");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            Assert.Equal(typeof(LocationController), route.Controller);
            Assert.Equal("Get", route.Action);
            Assert.Equal("id", route.RouteData.Values.First().Value);
        }

        [Fact]
        public void LocationGetByPostcode_WithPostcode_RoutesCorrectly() {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/location/postcode/so666xx");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            Assert.Equal(typeof(LocationController), route.Controller);
            Assert.Equal("GetByPostcode", route.Action);
            Assert.Equal("so666xx", route.RouteData.Values.First().Value);
        }

        [Fact]
        public void LocationGetByPostcode_WithoutPostcode_RoutesCorrectly()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/location/postcode/");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            Assert.Equal(typeof(LocationController), route.Controller);
            Assert.Equal("GetByPostcode", route.Action);
        }


        [Fact]
        public void LocationSearch_WithQuery_RoutesCorrectly() {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/location/search/mill way");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            Assert.Equal(typeof (LocationController), route.Controller);
            Assert.Equal("Search", route.Action);
            Assert.Equal("mill way", route.RouteData.Values.First().Value);

        }

        [Fact]
        public void LocationSearch_WithoutQuery_RoutesCorrectly() {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://domain/location/search/mill way");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = WebApi.RouteRequest(config, request);

            Assert.Equal(typeof(LocationController), route.Controller);
            Assert.Equal("Search", route.Action);
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