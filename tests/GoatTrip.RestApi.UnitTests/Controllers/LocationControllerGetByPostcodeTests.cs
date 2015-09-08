using GoatTrip.RestApi.Controllers;
using Moq;
using Xunit;

namespace GoatTrip.RestApi.UnitTests.Controllers {
    public class LocationControllerGetByPostcodeTests
        : LocationControllerBaseTests {

        [Fact]
        public void GetByPostcode_Always_CallsIsValid() {
            _sut.GetByPostcode("");

            _mockQueryValidator.Verify(v => v.IsValid(It.IsAny<string>()));
        }

        [Fact]
        public void GetByPostcode_WithInvalidQuery_ReturnsBadRequest() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(false);

            var result = _sut.GetByPostcode("");
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public void GetByPostcode_WithValidQuery_DoesntReturnBadRequest() {

            var result = _sut.GetByPostcode("x");
            Assert.False(result is BadRequestResult);
        }

        [Fact]
        public void GetByPostcode_WithValidQuery_CallsService() {

            _sut.GetByPostcode("x");
            _mockLocationSearchPostcodeService.Verify(s => s.SearchByPostcode(It.Is<string>(q => q == "x")));
        }
    }
}