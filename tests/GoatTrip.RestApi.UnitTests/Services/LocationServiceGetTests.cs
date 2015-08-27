namespace GoatTrip.RestApi.UnitTests.Services {
    using DAL;
    using Moq;
    using Xunit;

    public class LocationServiceGetTests
        : LocationServiceTestsBase {

        [Fact]
        public void Get_WithLocationNotFoundThrownByRepo_DoesntSwallowException() {
            _mockLocationRepository.Setup(r => r.Get(It.IsAny<string>())).Throws(new LocationNotFoundException("something"));

            Assert.Throws<LocationNotFoundException>(() => _sut.Get("anything"));
        }

    }
}