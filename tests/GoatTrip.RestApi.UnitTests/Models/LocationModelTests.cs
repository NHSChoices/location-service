
using GoatTrip.RestApi.Models;
using Xunit;

namespace GoatTrip.RestApi.UnitTests.Models {
    public class LocationModelTests {

        [Fact]
        public void GroupDescription_WithAllPropertiesSet_BuildsCorrectString() {
            var sut = new LocationModel();

            Assert.Equal("", sut.GroupDescription);

            sut.Postcode = "SO11 1XX";
            Assert.Equal("SO11 1XX", sut.GroupDescription);
            sut.BuildingName = "Some building";
            Assert.Equal("SO11 1XX, Some building", sut.GroupDescription);
            sut.StreetDescription = "Some street";
            Assert.Equal("SO11 1XX, Some building, Some street", sut.GroupDescription);
            sut.Locality = "Southampton";
            Assert.Equal("SO11 1XX, Some building, Some street, Southampton", sut.GroupDescription);

        }
    }
}
