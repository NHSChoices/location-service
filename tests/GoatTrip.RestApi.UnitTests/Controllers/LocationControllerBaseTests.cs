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

            _sut = new LocationController(_mockQueryValidator.Object, _mockLocationRetrievalService.Object, _mockLocationSearchService.Object, _mockLocationSearchPostcodeService.Object, _mockLocationQueryFields.Object)
            {
                Request = httpRequestMessage
            };

            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(true);

        }

        protected readonly LocationController _sut;
        protected readonly Mock<ILocationQueryValidator> _mockQueryValidator = new Mock<ILocationQueryValidator>();
        protected readonly Mock<ILocationRetrievalService> _mockLocationRetrievalService = new Mock<ILocationRetrievalService>();
        protected readonly Mock<ILocationSearchService> _mockLocationSearchService = new Mock<ILocationSearchService>();
        protected readonly Mock<ILocationSearchPostcodeService> _mockLocationSearchPostcodeService = new Mock<ILocationSearchPostcodeService>();
        protected readonly Mock<ILocationQueryFields> _mockLocationQueryFields = new Mock<ILocationQueryFields>();
    }
}