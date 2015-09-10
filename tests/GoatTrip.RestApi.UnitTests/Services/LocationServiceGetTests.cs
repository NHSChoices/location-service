namespace GoatTrip.RestApi.UnitTests.Services {
    using DAL;
    using DAL.DTOs;
    using Moq;
    using Xunit;

    public class LocationServiceGetTests
        : LocationServiceTestsBase {

        [Fact]
        public void Get_WithLocationNotFoundThrownByRepo_DoesntSwallowException() {
            _mockLocationRepository.Setup(r => r.Get(It.IsAny<string>())).Throws(new LocationNotFoundException("something"));

            Assert.Throws<LocationNotFoundException>(() => _sutGet.Get("anything"));
        }

        [Fact]
        public void Get_WithExistingLocation_ReturnsFoundLocation() {
            _mockIdEncoder.Setup(e => e.Decode("anything")).Returns("anything");

            _mockDataReader.Setup(r => r[POSTCODE_FIELD]).Returns("anything");

            _mockLocationRepository.Setup(r => r.Get("anything"))
                .Returns(new Location(_mockDataReader.Object, _mockLocationFormatter.Object));

            var result = _sutGet.Get("anything");

            Assert.Equal("anything", result.Postcode);
        }

    }
}