
using GoatTrip.Common.Formatters;

namespace GoatTrip.RestApi.UnitTests.Services {
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DAL;
    using DAL.DTOs;
    using RestApi.Models;
    using RestApi.Services;
    using Moq;
    using Xunit;

    public abstract class LocationServiceTestsBase
    {
        protected LocationServiceTestsBase() {

            _mockIdEncoder = new Mock<ILocationIdEncoder>();

            _mockLocationQueryFields = new Mock<ILocationQueryFields>();

            _mockQueryValidator = new Mock<ILocationQueryValidator>();
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);

            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockLocationGroupRepository = new Mock<ILocationGroupRepository>();
            _mockQuerySanitiser = new Mock<ILocationQuerySanitiser>();
            _mockQuerySanitiser.Setup(s => s.Sanitise(It.IsAny<string>())).Returns<string>(q => q.ToLower());

            _mockLocationGroupFormatter = new Mock<IConditionalFormatter<string, LocationDataField>>();
            _mockLocationGroupFormatter.Setup(
                r => r.DetermineConditionsAndFormat(It.IsAny<string>(), It.IsAny<LocationDataField>())).Returns<string>(a => a);

            _mockLocationFormatter = new Mock<IConditionalFormatter<string, string>>();
            _mockLocationFormatter.Setup(
                r => r.DetermineConditionsAndFormat(It.IsAny<string>(), It.IsAny<string>())).Returns<string>(a=> a);

            _sutSearch = new LocationSearchService(_mockLocationGroupRepository.Object, _mockQueryValidator.Object, _mockQuerySanitiser.Object, _mockLocationQueryFields.Object, _mockIdEncoder.Object);
            _sutPostcode = new LocationSearchPostcodeService(_mockLocationRepository.Object, _mockQueryValidator.Object, _mockQuerySanitiser.Object);
            _sutGet = new LocationRetrievalService(_mockLocationRepository.Object, _mockIdEncoder.Object);

            _mockDataReader = new Mock<IDataRecord>();
            _mockDataReader.Setup(r => r[It.IsAny<string>()]).Returns("");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "X_COORDINATE")]).Returns("1.0");
            _mockDataReader.Setup(r => r[It.Is<string>(y => y == "Y_COORDINATE")]).Returns("2.0");

            _builder = new LocationGroupBuilder(_mockLocationGroupFormatter.Object);
            _locationQueryFields = new SqlIteLocationQueryFields();
        }

        protected static void AssertIsValidResult(List<LocationGroupModel> result, int count, string postcode) {
            Assert.Equal(1, result.Count());
            Assert.Equal(count, result.First().Count);
            Assert.True(result.Any(l => l.Description.Contains(postcode)));
        }

        protected void CreateMockResults(string postcode, int count = 1) {
            _mockIdEncoder.Setup(e => e.Decode(postcode)).Returns(postcode);

            _mockDataReader.Setup(r => r[POSTCODE_FIELD]).Returns(postcode.ToUpper());

            _mockLocationRepository.Setup(r => r.FindLocations(It.Is<string>(s => s == postcode.ToLower())))
                .Returns(() =>
                {
                    var result = new List<Location>(count);
                    for (int i = 0; i < count; ++i)
                    {
                        result.Add(new Location(_mockDataReader.Object, _mockLocationFormatter.Object));
                    }
                    return result;
                });

        }

        protected readonly LocationSearchService _sutSearch;
        protected readonly LocationSearchPostcodeService _sutPostcode;
        protected readonly LocationRetrievalService _sutGet;

        protected readonly Mock<IConditionalFormatter<string, LocationDataField>> _mockLocationGroupFormatter;
        protected readonly Mock<IConditionalFormatter<string, string>> _mockLocationFormatter;

        
        protected readonly Mock<ILocationRepository> _mockLocationRepository;
        protected readonly Mock<ILocationQueryValidator> _mockQueryValidator;
        protected readonly Mock<ILocationQuerySanitiser> _mockQuerySanitiser;
        protected readonly Mock<ILocationIdEncoder> _mockIdEncoder;
        protected readonly Mock<ILocationQueryFields> _mockLocationQueryFields;
        protected readonly Mock<IDataRecord> _mockDataReader;
        protected readonly Mock<ILocationGroupRepository> _mockLocationGroupRepository;
        protected readonly LocationGroupBuilder _builder;
        protected readonly ILocationQueryFields _locationQueryFields;

        protected const string POSTCODE_LOCATOR_FIELD = "POSTCODE_LOCATOR";
        protected const string POSTCODE_FIELD = "POSTCODE";
    }
}