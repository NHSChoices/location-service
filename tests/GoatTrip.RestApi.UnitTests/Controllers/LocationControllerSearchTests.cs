
using GoatTrip.DAL;
using GoatTrip.RestApi.Services;

namespace GoatTrip.RestApi.UnitTests.Controllers {
    using RestApi.Controllers;
    using Moq;
    using Xunit;

    public class LocationControllerSearchTests
        : LocationControllerBaseTests {

        [Fact]
        public void Search_Always_CallsIsValid() {
            _sut.Search("");

            _mockQueryValidator.Verify(v => v.IsValid(It.IsAny<string>()));
        }

        [Fact]
        public void Search_WithInvalidQuery_ReturnsBadRequest() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>()))
                .Returns(false);

            var result = _sut.Search("");
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public void Search_WithValidQuery_DoesntReturnBadRequest() {

            var result = _sut.Search("x");
            Assert.False(result is BadRequestResult);
        }

        [Fact]
        public void Search_WithValidQuery_CallsService() {

            _sut.Search("x");
            _mockLocationSearchService.Verify(s => s.Search(It.Is<string>(q => q == "x"), It.IsAny<ILocationGroupingStrategy>()));
        }

    }

}
