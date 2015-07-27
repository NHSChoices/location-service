namespace GoatTrip.RestApi.UnitTests.Services {
    using RestApi.Services;
    using Xunit;

    public class LocationQuerySanitiserTests {

        [Fact]
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