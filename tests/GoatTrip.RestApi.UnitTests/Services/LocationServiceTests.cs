namespace GoatTrip.RestApi.UnitTests.Services {

    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DAL;
    using DAL.DTOs;
    using Microsoft.SqlServer.Server;
    using Models;
    using Moq;
    using RestApi.Services;
    using Xunit;

    public class LocationServiceTests
    {
        private const string POSTCODE_LOCATOR_FIELD = "POSTCODE_LOCATOR";
        public LocationServiceTests() {

            _mockQueryValidator = new Mock<ILocationQueryValidator>();
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);

            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockQuerySanitiser = new Mock<ILocationQuerySanitiser>();
            _mockQuerySanitiser.Setup(s => s.Sanitise(It.IsAny<string>())).Returns<string>(q => q.ToLower());

            _sut = new LocationService(_mockLocationRepository.Object, _mockQueryValidator.Object, _mockQuerySanitiser.Object);

            _mockDataReader = new Mock<IDataRecord>();
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "ADMINISTRATIVE_AREA")]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "BUILDING_NAME")]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "BLPU_ORGANISATION")]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "STREET_DESCRIPTION")]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "PAO_START_NUMBER")]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "LOCALITY")]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "TOWN_NAME")]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "POST_TOWN")]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "POSTCODE")]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "X_COORDINATE")]).Returns("1.0");
            _mockDataReader.Setup(r => r[It.Is<string>(y => y == "Y_COORDINATE")]).Returns("2.0");
            _mockDataReader.Setup(r => r[It.Is<string>(y => y == "POSTCODE_LOCATOR")]).Returns("");

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
            _mockDataReader.Setup(r => r[It.Is<string>(p => p == POSTCODE_LOCATOR_FIELD)]).Returns("SO11 1XX");

            _mockLocationRepository.Setup(r => r.FindLocations(It.Is<string>(s => s == "so11 1xx"))).Returns(new List<Location> {
                new Location(_mockDataReader.Object)});

            var result = _sut.Get("SO11 1XX").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");
        }

        [Fact]
        public void Get_WithMoreThanOneExistingPostcode_ReturnsAllMatchingLocations() {
            _mockDataReader.Setup(r => r[It.Is<string>(p => p == POSTCODE_LOCATOR_FIELD)]).Returns("SO22 2XX");

            _mockLocationRepository.Setup(r => r.FindLocations(It.Is<string>(s => s == "so22 2xx")))
                .Returns(new List<Location> {
                    new Location(_mockDataReader.Object),
                    new Location(_mockDataReader.Object),
                    new Location(_mockDataReader.Object)
                });

            var result = _sut.Get("SO22 2XX").ToList();

            AssertIsValidResult(result, 3, "SO22 2XX");
        }

        [Fact]
        public void Get_Always_CallsSanitise() {
            _sut.Get("SO22 2XX");

            _mockQuerySanitiser.Verify(s => s.Sanitise(It.Is<string>(q => q == "SO22 2XX")));
        }

        [Fact]
        public void Get_WithPostcode_MatchesRegardlessOfCase() {

            _mockDataReader.Setup(r => r[It.Is<string>(p => p == POSTCODE_LOCATOR_FIELD)]).Returns("SO11 1XX");

            _mockLocationRepository.Setup(r => r.FindLocations(It.Is<string>(s => s == "so11 1xx"))).Returns(new List<Location> {
                new Location(_mockDataReader.Object)});

            var result = _sut.Get("SO11 1XX").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");

            result = _sut.Get("so11 1xx").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");
        }

        private static void AssertIsValidResult(List<LocationGroupModel> result, int count, string postcode) {
            Assert.Equal(1, result.Count());
            Assert.Equal(count, result.First().Locations.Count());
            if (count > 1)
                Assert.True(result.First().Locations.All(l => l.Postcode == postcode));
            else
                Assert.Equal(postcode, result.First().Locations.First().Postcode);
        }


        private readonly LocationService _sut;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly Mock<ILocationQueryValidator> _mockQueryValidator;
        private readonly Mock<ILocationQuerySanitiser> _mockQuerySanitiser;
        private Mock<IDataRecord> _mockDataReader;
    }
}