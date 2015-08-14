using System.Linq;
using GoatTrip.RestApi.Services;
using Moq;
using Xunit;

namespace GoatTrip.RestApi.UnitTests.Services {
    public class LocationServiceTests : LocationServiceTestsBase {

        [Fact]
        public void GetByPostcode_Always_CallsIsValid() {
            _sut.GetByPostcode("ANY");

            _mockQueryValidator.Verify(v => v.IsValid(It.Is<string>(q => q == "ANY")));
        }

        [Fact]
        public void GetByPostcode_WithInvalidQuery_ThrowsInvalidLocationQueryException() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(false);

            Assert.Throws<InvalidLocationQueryException>(() => _sut.GetByPostcode(""));
        }

        [Fact]
        public void GetByPostcode_WithExistingPostcode_ReturnsThatLocation() {
            CreateMockResults("SO11 1XX");

            var result = _sut.GetByPostcode("SO11 1XX").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");
        }

        [Fact]
        public void GetByPostcode_Always_CallsSanitise() {
            _sut.GetByPostcode("SO22 2XX");

            _mockQuerySanitiser.Verify(s => s.Sanitise(It.Is<string>(q => q == "SO22 2XX")));
        }

        [Fact]
        public void GetByPostcode_WithPostcode_MatchesRegardlessOfCase() {

            CreateMockResults("SO11 1XX");

            var result = _sut.GetByPostcode("SO11 1XX").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");

            result = _sut.GetByPostcode("so11 1xx").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");
        }

        [Fact]
        public void GetByPostcode_WithMoreThanOneExistingPostcode_ReturnsAllMatchingLocations()
        {
            CreateMockResults("SO22 2XX", 3);

            var result = _sut.GetByPostcode("SO22 2XX").ToList();

            AssertIsValidResult(result, 3, "SO22 2XX");
        }
    }
}