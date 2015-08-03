namespace GoatTrip.RestApi.UnitTests.Services {
    using System.Collections.Generic;
    using System.Linq;
    using DAL.DTOs;
    using Moq;
    using RestApi.Services;
    using Xunit;

    public class LocationServiceAddressTests
        : LocationServiceTestsBase {

        [Fact]
        public void Get_Always_CallsIsValid() {
            _sut.GetByAddress("ANY");

            _mockQueryValidator.Verify(v => v.IsValid(It.Is<string>(q => q == "ANY")));
        }

        [Fact]
        public void GetByAddress_WithInvalidAddress_ThrowsInvalidLocationQueryException() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(false);

            Assert.Throws<InvalidLocationQueryException>(() => _sut.GetByAddress(""));
        }

        [Fact]
        public void GetByAddress_Always_CallsFindLocationsByAddress() {

            _sut.GetByAddress("Coronation street");

            _mockLocationRepository.Verify(r => r.FindLocationsbyAddress(It.Is<string>(p => p == "Coronation street")));
        }

        [Fact]
        public void GetByAddress_WithExistingAddress_ReturnsThatLocation() {

            _mockDataReader.Setup(r => r[It.Is<string>(p => p == POSTCODE_LOCATOR_FIELD)]).Returns("SO99 9XX");

            _mockLocationRepository.Setup(r => r.FindLocationsbyAddress(It.Is<string>(s => s == "Coronation street")))
                .Returns(() => new List<Location> {
                    new Location(_mockDataReader.Object)
                });

            var result = _sut.GetByAddress("Coronation street").ToList();

            AssertIsValidResult(result, 1, "SO99 9XX");

        }




    }
}