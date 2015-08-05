
namespace GoatTrip.RestApi.UnitTests.Services {
    using GoatTrip.RestApi.Services;
    using Xunit;

    public class PostcodeQuerySanitiserTests {

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

        [Fact]
        public void Sanitise_WithSpacesRoundQuery_TrimsQuery() {
            var result = _sut.Sanitise("   SO111XX ");
            Assert.Equal("so111xx", result);
        }

        private readonly PostcodeQuerySanitiser _sut = new PostcodeQuerySanitiser();
    }
}