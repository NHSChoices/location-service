
namespace GoatTrip.RestApi.UnitTests.Services {
    using System.Linq;
    using RestApi.Services;
    using Moq;
    using Xunit;

    public class LocationServiceSearchByPostcodeTests
        : LocationServiceTestsBase {

        [Fact]
        public void SearchByPostcode_Always_CallsIsValid() {
            _sut.SearchByPostcode("ANY");

            _mockQueryValidator.Verify(v => v.IsValid(It.Is<string>(q => q == "ANY")));
        }

        [Fact]
        public void SearchByPostcode_WithInvalidQuery_ThrowsInvalidLocationQueryException() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(false);

            Assert.Throws<InvalidLocationQueryException>(() => _sut.SearchByPostcode(""));
        }

        [Fact]
        public void SearchByPostcode_WithExistingPostcode_ReturnsThatLocation() {
            CreateMockResults("SO11 1XX");

            var result = _sut.SearchByPostcode("SO11 1XX").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");
        }

        [Fact]
        public void SearchByPostcode_Always_CallsSanitise() {
            _sut.SearchByPostcode("SO22 2XX");

            _mockQuerySanitiser.Verify(s => s.Sanitise(It.Is<string>(q => q == "SO22 2XX")));
        }

        [Fact]
        public void SearchByPostcode_WithPostcode_MatchesRegardlessOfCase() {

            CreateMockResults("SO11 1XX");

            var result = _sut.SearchByPostcode("SO11 1XX").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");

            result = _sut.SearchByPostcode("so11 1xx").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");
        }

        [Fact]
        public void SearchByPostcode_WithMoreThanOneExistingPostcode_ReturnsAllMatchingLocations()
        {
            CreateMockResults("SO22 2XX", 3);

            var result = _sut.SearchByPostcode("SO22 2XX").ToList();

            AssertIsValidResult(result, 3, "SO22 2XX");
        }
    }
}