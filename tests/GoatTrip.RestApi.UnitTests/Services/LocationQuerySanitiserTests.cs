namespace GoatTrip.RestApi.UnitTests.Services {
    using RestApi.Services;
    using Xunit;

    public class LocationQuerySanitiserTests {

        [Fact]
        public void Sanitise_WithExtranuousSpacesInQuery_StripsSpaces() {
            var result = _sut.Sanitise("SO11   1XX");
            Assert.Equal("SO111XX", result);

            result = _sut.Sanitise("SO11  1XX");
            Assert.Equal("SO111XX", result);

            result = _sut.Sanitise(" SO11 1XX ");
            Assert.Equal("SO111XX", result);

            result = _sut.Sanitise("SO111XX");
            Assert.Equal("SO111XX", result);

        }

        readonly LocationQuerySanitiser _sut = new LocationQuerySanitiser();

    }
}