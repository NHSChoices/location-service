namespace GoatTrip.RestApi.UnitTests.Services {
    using RestApi.Services;
    using Xunit;

    public class SearchQuerySanitiserTests {

        [Fact]
        public void Sanitise_WithCommaInQuery_ReplacesCommaWithSpace() {
            var result = _sut.Sanitise("Some,address");
            Assert.Equal("Some address", result);
        }

        [Fact]
        public void Sanitise_WithSpacesRoundQuery_TrimsQuery() {
            var result = _sut.Sanitise(" Some address  ");
            Assert.Equal("Some address", result);
        }

        private readonly SearchQuerySanitiser _sut = new SearchQuerySanitiser();
    }
}