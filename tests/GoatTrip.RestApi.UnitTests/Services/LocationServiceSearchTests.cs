namespace GoatTrip.RestApi.UnitTests.Services {
    using System.Collections.Generic;
    using System.Linq;
    using DAL;
    using DAL.DTOs;
    using Moq;
    using RestApi.Services;
    using Xunit;

    public class LocationServiceSearchTests
        : LocationServiceTestsBase {

        public LocationServiceSearchTests() {
            _mockGroupStrat = new Mock<ILocationGroupingStrategy>();
            _mockGroupStrat.Setup(g => g.Fields).Returns(new List<LocationQueryField> { _locationQueryFields.Street });
        }

        [Fact]
        public void Search_WithSmallSingleGroupReturned_RequeriesRepo()
        {
            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(LocationSearchService.GROUPING_THRESHOLD - 1);

            _mockLocationGroupRepository.Setup(r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> { _builder.Build(_mockDataReader.Object, _mockGroupStrat.Object.Fields) });

            _sutSearch.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationGroupRepository.Verify(r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Exactly(2));
        }

        [Fact]
        public void Search_WithLargeSingleGroupReturned_RequeriesRepo()
        {
            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(LocationSearchService.GROUPING_THRESHOLD + 1);

            _mockLocationGroupRepository.Setup(r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> { _builder.Build(_mockDataReader.Object, _mockGroupStrat.Object.Fields) });

            _sutSearch.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationGroupRepository.Verify(r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Exactly(2));
        }

        [Fact]
        public void Search_WithTwoGroupsAndLocationSumLessThanThreshold_RequeriesRepo()
        {
            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(10);

            _mockLocationGroupRepository.Setup(
                r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> {
                    _builder.Build(_mockDataReader.Object, _mockGroupStrat.Object.Fields),
                    _builder.Build(_mockDataReader.Object, _mockGroupStrat.Object.Fields)
                });

            _sutSearch.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationGroupRepository.Verify(r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Exactly(2));
        }

        [Fact]
        public void Search_WithTwoGroupsAndLocationSumLowerThanThreshold_DoesRequeryRepo()
        {
            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(10);

            _mockLocationGroupRepository.Setup(
                r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> {
                    _builder.Build(_mockDataReader.Object, _mockGroupStrat.Object.Fields),
                    _builder.Build(_mockDataReader.Object, _mockGroupStrat.Object.Fields)
                });

            _sutSearch.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationGroupRepository.Verify(r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Exactly(2));
        }

        [Fact]
        public void Search_WithTwoGroupsAndLocationSumGreaterThanThreshold_DoesntRequeryRepo()
        {

            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(LocationSearchService.GROUPING_THRESHOLD + 1);

            _mockLocationGroupRepository.Setup(
                r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> {
                    _builder.Build(_mockDataReader.Object, _mockGroupStrat.Object.Fields),
                    _builder.Build(_mockDataReader.Object, _mockGroupStrat.Object.Fields)
                });

            _sutSearch.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationGroupRepository.Verify(r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Once);
        }

        [Fact]
        public void Search_WithNoResults_DoesntRequeryRepo() {

            _mockLocationGroupRepository.Setup(
                r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup>());

            _sutSearch.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationGroupRepository.Verify(r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Once);
        }

        [Fact]
        public void Search_WithGroupedResults_ReturnsCorrectNextData() {

            _mockDataReader.Setup(r => r.FieldCount).Returns(2);

            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            
            _mockDataReader.Setup(r => r["Number"]).Returns(LocationSearchService.GROUPING_THRESHOLD + 1);
            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");


            _mockLocationGroupRepository.Setup(
                r => r.FindGroupedLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> {
                    new LocationGroup { GroupDescription = "someplace", LocationsCount = 2},
                    new LocationGroup { UPRN = 123, LocationsCount = 1}
                });

            var results = _sutSearch.Search("SO22 2XX", _mockGroupStrat.Object);

            Assert.Equal("/location/search/someplace", results.First().Next);
            Assert.Equal("/location/123", results.Last().Next);

        }

        private readonly Mock<ILocationGroupingStrategy> _mockGroupStrat;

    }
}