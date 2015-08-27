

namespace GoatTrip.RestApi.UnitTests.Services {
    using System.Collections.Generic;
    using System.Linq;
    using GoatTrip.DAL.DTOs;
    using GoatTrip.RestApi.Services;
    using Moq;
    using Xunit;

    public class LocationServiceSearchByAddressTests
        : LocationServiceTestsBase {

        [Fact]
        public void SearchByAddress_Always_CallsIsValid() {
            _sut.SearchByAddress("ANY");

            _mockQueryValidator.Verify(v => v.IsValid(It.Is<string>(q => q == "ANY")));
        }

        [Fact]
        public void SearchByAddress_Always_CallsSanitise() {
            _sut.SearchByAddress("SO22 2XX");

            _mockQuerySanitiser.Verify(s => s.Sanitise(It.Is<string>(q => q == "SO22 2XX")));
        }

        [Fact]
        public void SearchByAddress_WithInvalidAddress_ThrowsInvalidLocationQueryException() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(false);

            Assert.Throws<InvalidLocationQueryException>(() => _sut.SearchByAddress(""));
        }

        [Fact]
        public void SearchByAddress_Always_CallsFindLocationsByAddress() {

            _sut.SearchByAddress("Coronation street");

            _mockLocationRepository.Verify(r => r.FindLocationsbyAddress(It.Is<string>(p => p == "coronation street")));
        }

        [Fact]
        public void SearchByAddress_WithExistingAddress_ReturnsThatLocation() {

            _mockDataReader.Setup(r => r[It.Is<string>(p => p == POSTCODE_LOCATOR_FIELD)]).Returns("SO99 9XX");

            _mockLocationRepository.Setup(r => r.FindLocationsbyAddress(It.Is<string>(s => s == "coronation street")))
                .Returns(() => new List<Location> {
                    new Location(_mockDataReader.Object)
                });

            var result = _sut.SearchByAddress("Coronation street").ToList();

            AssertIsValidResult(result, 1, "SO99 9XX");

        }




    }
}