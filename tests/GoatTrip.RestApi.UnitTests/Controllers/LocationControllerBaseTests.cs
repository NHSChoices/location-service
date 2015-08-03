using System.Net.Http;
using System.Web.Http;
using GoatTrip.RestApi.Controllers;
using GoatTrip.RestApi.Services;
using Moq;

namespace GoatTrip.RestApi.UnitTests.Controllers {
    public abstract class LocationControllerBaseTests {
        protected LocationControllerBaseTests() {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.SetConfiguration(new HttpConfiguration());

            _sut = new LocationController(_mockQueryValidator.Object, _mockLocationService.Object) {
                Request = httpRequestMessage
            };
        }

        protected readonly LocationController _sut;
        protected readonly Mock<ILocationQueryValidator> _mockQueryValidator = new Mock<ILocationQueryValidator>();
        protected readonly Mock<ILocationService> _mockLocationService = new Mock<ILocationService>();
    }
}