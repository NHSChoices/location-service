
namespace GoatTrip.RestApi.UnitTests {
    using System.Linq;
    using System.Web.Http;
    using Xunit;

    public class WebApiConfigTests {

        [Fact]
        public void Config_Always_CallsMapHttpRouteWithCorrectRoute() {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            Assert.True(config.Routes.Any(r => r.RouteTemplate == "{controller}/{query}"));
        }
    }
}
