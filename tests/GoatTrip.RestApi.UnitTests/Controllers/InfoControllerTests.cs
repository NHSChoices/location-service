
namespace GoatTrip.RestApi.UnitTests.Controllers {
    using System.Web.Http.Results;
    using DAL;
    using Moq;
    using RestApi.Controllers;
    using Xunit;

    public class InfoControllerTests {

        [Fact]
        public void Get_Always_ReturnsInfoModel() {

            var sut = new InfoController(new Mock<IConnectionManager>().Object);

            var result = sut.Get();
            var conNegResult = Assert.IsType<OkNegotiatedContentResult<InfoModel>>(result);

            //Assert.Equal("data: 12", conNegResult.Content);
        }

    }
}
