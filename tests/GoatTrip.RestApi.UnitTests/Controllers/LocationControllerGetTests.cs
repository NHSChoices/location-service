
namespace GoatTrip.RestApi.UnitTests.Controllers {
    using System.Web.Http.Results;
    using DAL;
    using Moq;
    using RestApi.Models;
    using Xunit;
    using BadRequestResult = RestApi.Controllers.BadRequestResult;

    public class LocationControllerGetTests
        : LocationControllerBaseTests {

        [Fact]
        public void Get_Always_CallsIsValid() {
            _sut.Get("something");

            _mockQueryValidator.Verify(v => v.IsValid("something"));
        }

        [Fact]
        public void Get_WithInvalidId_ReturnsBadResponse() {
            _mockQueryValidator.Setup(v => v.IsValid(""))
                .Returns(false);

            var result = _sut.Get("");
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public void Get_WithValidId_CallsService() {
            _sut.Get("anything");

            _mockLocationRetrievalService.Verify(s => s.Get("anything"), Times.Once);
        }

        [Fact]
        public void Get_WithReturnedResults_ReturnsThatResult() {
            var location = new LocationModel {
                Postcode = "somewhere"
            };
            _mockLocationRetrievalService.Setup(s => s.Get("anything")).Returns(location);

            _sut.Get("anything");

            Assert.Equal("somewhere", location.Postcode);
        }

        [Fact]
        public void Get_WithNonExistantId_Returns404() {
            _mockLocationRetrievalService.Setup(s => s.Get("missing")).Throws(new LocationNotFoundException("missing"));

            var result = _sut.Get("missing");
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }
    }
}