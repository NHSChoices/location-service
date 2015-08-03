using GoatTrip.RestApi.Services;
using Xunit;

namespace GoatTrip.RestApi.UnitTests.Services {
    public class LocationQuerySanitiserTests {

        [Fact(Skip = "DB needs to be updated to exclude spaces, currently spaces aren't stripped in code.")]

        public void Sanitise_WithExtranuousSpacesInQuery_StripsSpaces() {
            var result = _sut.Sanitise("so11   1xx");
            Assert.Equal("so111xx", result);

            result = _sut.Sanitise("so11  1xx");
            Assert.Equal("so111xx", result);

            result = _sut.Sanitise(" so11 1xx ");
            Assert.Equal("so111xx", result);

            result = _sut.Sanitise("so111xx");
            Assert.Equal("so111xx", result);

        }

        [Fact]
        public void Sanitise_WithUpperCaseQuery_ReturnsLowerCaseQuery() {
            var result = _sut.Sanitise("SO111XX");
            Assert.Equal("so111xx", result);
        }

        readonly LocationQuerySanitiser _sut = new LocationQuerySanitiser();

    }
}