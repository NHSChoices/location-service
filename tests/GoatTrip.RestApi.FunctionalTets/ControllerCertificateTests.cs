using System.Net.Http;
using System.Web.Http;
using GoatTrip.RestApi.Controllers;
using GoatTrip.RestApi.Services;
using Moq;
using Xunit;

namespace GoatTrip.RestApi.FunctionalTets {

    public class ControllerCertificateTests {

        [Fact]
        public void NoCertificateTest()
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.SetConfiguration(new HttpConfiguration());

            LocationController sut = new LocationController(_mockQueryValidator.Object, _mockLocationService.Object)
            {
                Request = httpRequestMessage
            };

            var actionResult = sut.Get("SO111XX");
            Assert.IsType<BadRequestResult>(actionResult);
        }

        [Fact]
        public void CertificateTest()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpConfiguration = new HttpConfiguration();



            LocationController sut = new LocationController(_mockQueryValidator.Object, _mockLocationService.Object)
            {
                Request = httpRequestMessage
            };

            var actionResult = sut.Get("SO111XX");
            Assert.IsNotType<BadRequestResult>(actionResult);
        }


        protected readonly Mock<ILocationQueryValidator> _mockQueryValidator = new Mock<ILocationQueryValidator>();
        protected readonly Mock<ILocationService> _mockLocationService = new Mock<ILocationService>();

    }
}
