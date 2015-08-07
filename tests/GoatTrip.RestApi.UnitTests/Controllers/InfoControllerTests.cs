
namespace GoatTrip.RestApi.UnitTests.Controllers {
    using System.Web.Http.Results;
    using DAL;
    using Moq;
    using RestApi.Controllers;
    using Xunit;

    public class InfoControllerTests {

        public InfoControllerTests() {
            _mockConnectionManager = new Mock<IConnectionManager>();
            _sut = new InfoController(_mockConnectionManager.Object);
        }

        [Fact]
        public void Get_Always_ReturnsInfoModel() {

            var result = _sut.Get();
            var conNegResult = Assert.IsType<OkNegotiatedContentResult<InfoModel>>(result);

            //Assert.Equal("data: 12", conNegResult.Content);
        }

        [Fact]
        public void Get_Always_CallsInMemoryDbInitialised() {
            _sut.Get();

            _mockConnectionManager.Verify(c => c.InMemoryDbInitialised, Times.Once());
        }

        private Mock<IConnectionManager> _mockConnectionManager;
        private InfoController _sut;
    }

}
