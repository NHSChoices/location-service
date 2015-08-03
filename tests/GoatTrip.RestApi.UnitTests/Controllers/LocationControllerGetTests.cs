namespace GoatTrip.RestApi.UnitTests.Controllers {
    using Moq;
    using RestApi.Controllers;
    using Xunit;

    public class LocationControllerGetTests
        : LocationControllerBaseTests {

        [Fact]
        public void Get_Always_CallsIsValid() {
            _sut.Get("");

            _mockQueryValidator.Verify(v => v.IsValid(It.IsAny<string>()));
        }

        [Fact]
        public void Get_WithInvalidQuery_ReturnsBadRequest() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(false);

            var result = _sut.Get("");
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public void Get_WithValidQuery_DoesntReturnBadRequest() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(true);

            var result = _sut.Get("x");
            Assert.False(result is BadRequestResult);
        }

        [Fact]
        public void Get_WithValidQuery_CallsService() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(true);

            _sut.Get("x");
            _mockLocationService.Verify(s => s.Get(It.Is<string>(q => q == "x")));
        }
    }
}