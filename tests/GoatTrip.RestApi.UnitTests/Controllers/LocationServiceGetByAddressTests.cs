
namespace GoatTrip.RestApi.UnitTests.Controllers {
    using Moq;
    using RestApi.Controllers;
    using Xunit;

    public class LocationServiceGetByAddressTests
        : LocationControllerBaseTests {

        [Fact]
        public void GetByAddress_Always_CallsIsValid()
        {
            _sut.GetByAddress("");

            _mockQueryValidator.Verify(v => v.IsValid(It.IsAny<string>()));
        }

        [Fact]
        public void GetByAddress_WithInvalidQuery_ReturnsBadRequest()
        {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(false);

            var result = _sut.GetByAddress("");
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public void GetByAddress_WithValidQuery_DoesntReturnBadRequest()
        {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(true);

            var result = _sut.GetByAddress("x");
            Assert.False(result is BadRequestResult);
        }

        [Fact]
        public void GetByAddress_WithValidQuery_CallsService() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(true);

            _sut.GetByAddress("x");
            _mockLocationService.Verify(s => s.GetByAddress(It.Is<string>(q => q == "x")));
        }

    }

}
