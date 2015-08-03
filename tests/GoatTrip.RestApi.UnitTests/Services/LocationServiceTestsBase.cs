namespace GoatTrip.RestApi.UnitTests.Services {
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DAL;
    using DAL.DTOs;
    using Moq;
    using RestApi.Models;
    using RestApi.Services;
    using Xunit;

    public abstract class LocationServiceTestsBase
    {
        protected LocationServiceTestsBase() {

            _mockQueryValidator = new Mock<ILocationQueryValidator>();
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);

            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockQuerySanitiser = new Mock<ILocationQuerySanitiser>();
            _mockQuerySanitiser.Setup(s => s.Sanitise(It.IsAny<string>())).Returns<string>(q => q.ToLower());

            _sut = new LocationService(_mockLocationRepository.Object, _mockQueryValidator.Object, _mockQuerySanitiser.Object);

            _mockDataReader = new Mock<IDataRecord>();
            _mockDataReader.Setup(r => r[It.IsAny<string>()]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "X_COORDINATE")]).Returns("1.0");
            _mockDataReader.Setup(r => r[It.Is<string>(y => y == "Y_COORDINATE")]).Returns("2.0");

        }

        protected static void AssertIsValidResult(List<LocationGroupModel> result, int count, string postcode) {
            Assert.Equal(1, result.Count());
            Assert.Equal(count, result.First().Locations.Count());
            if (count > 1)
                Assert.True(result.First().Locations.All(l => l.Postcode == postcode));
            else
                Assert.Equal(postcode, result.First().Locations.First().Postcode);
        }

        protected void CreateMockResults(string postcode, int count = 1) {
            _mockDataReader.Setup(r => r[It.Is<string>(p => p == POSTCODE_LOCATOR_FIELD)]).Returns(postcode.ToUpper());

            _mockLocationRepository.Setup(r => r.FindLocations(It.Is<string>(s => s == postcode.ToLower())))
                .Returns(() =>
                {
                    var result = new List<Location>(count);
                    for (int i = 0; i < count; ++i)
                    {
                        result.Add(new Location(_mockDataReader.Object));
                    }
                    return result;
                });
        }

        protected readonly LocationService _sut;
        protected readonly Mock<ILocationRepository> _mockLocationRepository;
        protected readonly Mock<ILocationQueryValidator> _mockQueryValidator;
        protected readonly Mock<ILocationQuerySanitiser> _mockQuerySanitiser;
        protected readonly Mock<IDataRecord> _mockDataReader;
        protected const string POSTCODE_LOCATOR_FIELD = "POSTCODE_LOCATOR";
    }
}