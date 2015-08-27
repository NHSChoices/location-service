using System.Net.Http;
using System.Web.Http;
using GoatTrip.DAL;
using GoatTrip.RestApi.Controllers;
using GoatTrip.RestApi.Services;
using Moq;

namespace GoatTrip.RestApi.UnitTests.Controllers {
    public abstract class LocationControllerBaseTests {
        protected LocationControllerBaseTests() {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.SetConfiguration(new HttpConfiguration());

            _sut = new LocationController(_mockQueryValidator.Object, _mockLocationService.Object, _mockLocationQueryFields.Object)
            {
                Request = httpRequestMessage
            };

            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(true);

        }

        protected readonly LocationController _sut;
        protected readonly Mock<ILocationQueryValidator> _mockQueryValidator = new Mock<ILocationQueryValidator>();
        protected readonly Mock<ILocationService> _mockLocationService = new Mock<ILocationService>();
        protected readonly Mock<ILocationQueryFields> _mockLocationQueryFields = new Mock<ILocationQueryFields>();
    }
}