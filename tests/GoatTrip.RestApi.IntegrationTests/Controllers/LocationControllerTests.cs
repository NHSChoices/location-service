
namespace GoatTrip.RestApi.IntegrationTests.Controllers {
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using Moq;
    using RestApi.Controllers;
    using RestApi.Models;
    using Services;
    using Xunit;

    public class LocationControllerTests {

        [Fact]
        [Trait("Category", "integration")]
        public void Get_Never_SerialisesGroupDescription()
        {
            var mockValidator = new Mock<ILocationQueryValidator>();
            mockValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);

            var fakeLocation = new LocationModel {
                Postcode = "test"
            };

            var mockService = new Mock<ILocationService>();
            mockService.Setup(s => s.Get(It.IsAny<string>())).Returns(new List<LocationGroupModel> {
                new LocationGroupModel {
                    Description = fakeLocation.GroupDescription,
                    Locations = new List<LocationModel> {
                        fakeLocation
                    }
                }
            });

            var sut = new LocationController(mockValidator.Object, mockService.Object) {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            var result = sut.Get("test");

            var response = result.ExecuteAsync(new CancellationToken(false));
            var content = response.Result.Content.ReadAsStringAsync().Result;

            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            Assert.NotNull(json[0]["Description"]);
            Assert.Null(json[0]["Locations"][0]["GroupDescription"]);
        }

    }
}
