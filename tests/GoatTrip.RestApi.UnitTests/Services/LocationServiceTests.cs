namespace GoatTrip.RestApi.UnitTests.Services {

    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Moq;
    using RestApi.Controllers;
    using RestApi.Services;
    using Xunit;

    public class LocationServiceTests {

        public LocationServiceTests() {

            _mockQueryValidator = new Mock<ILocationQueryValidator>();
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);

            _mockDataRetriever = new Mock<ILocationDataRetriever>();
            _mockDataRetriever.Setup(r => r.RetrieveAll()).Returns(new List<LocationModel> {
                new LocationModel {
                    Postcode = "SO11 1XX"
                },
                new LocationModel {
                    Postcode = "SO22 2XX"
                },
                new LocationModel {
                    Postcode = "SO22 2XX"
                },
                new LocationModel {
                    Postcode = "SO22 2XX"
                }
            });

            _mockQuerySanitiser = new Mock<ILocationQuerySanitiser>();
            _mockQuerySanitiser.Setup(s => s.Sanitise(It.IsAny<string>())).Returns<string>(q => q);

            _sut = new LocationService(_mockDataRetriever.Object, _mockQueryValidator.Object, _mockQuerySanitiser.Object);
        }

        [Fact]
        public void Get_Always_CallsIsValid() {
            _sut.Get("ANY");

            _mockQueryValidator.Verify(v => v.IsValid(It.Is<string>(q => q == "ANY")));
        }

        [Fact]
        public void Get_WithInvliadQuery_ThrowsInvalidLocationQueryException() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(false);

            Assert.Throws<InvalidLocationQueryException>(() => _sut.Get(""));
        }

        [Fact]
        public void Get_WithExistingPostcode_ReturnsThatLocation() {
            var result = _sut.Get("SO11 1XX");

            Assert.Equal(1, result.Count());
        }

        [Fact]
        public void Get_WithMoreThanOneExistingPostcode_ReturnsAllMatchingLocations() {
            var result = _sut.Get("SO22 2XX");

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void Get_Always_CallsSanitise() {
            _sut.Get("SO22 2XX");

            _mockQuerySanitiser.Verify(s => s.Sanitise(It.Is<string>(q => q == "SO22 2XX")));
        }

        private readonly LocationService _sut;
        private readonly Mock<ILocationDataRetriever> _mockDataRetriever;
        private readonly Mock<ILocationQueryValidator> _mockQueryValidator;
        private readonly Mock<ILocationQuerySanitiser> _mockQuerySanitiser;
    }
}