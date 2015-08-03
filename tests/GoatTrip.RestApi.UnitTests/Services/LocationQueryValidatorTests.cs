using GoatTrip.RestApi.Services;
using Xunit;

namespace GoatTrip.RestApi.UnitTests.Services {
    public class LocationQueryValidatorTests {
        [Fact]
        public void IsValid_WithLessThanOneCharacterQuery_ReturnsFalse() {
            var sut = new LocationQueryValidator();
            var result = sut.IsValid("");

            Assert.False(result);
        }

        [Fact]
        public void IsValid_WithOneCharacterQuery_ReturnsTrue() {
            var sut = new LocationQueryValidator();
            var result = sut.IsValid("x");

            Assert.True(result);
        }
    }
}