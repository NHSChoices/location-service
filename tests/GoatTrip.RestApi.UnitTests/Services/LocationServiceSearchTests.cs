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
            _mockGroupStrat.Setup(g => g.Fields).Returns(new List<LocationQueryField> { LocationQueryField.Street });
        }

        [Fact]
        public void Search_WithGroupingStrategy_CallsGroupOnStrategy()
        {
            CreateMockResults("SO22 2XX", 3);

            _sut.Search("SO22 2XX", _mockGroupStrat.Object).ToList();

            //_mockGroupStrat.Verify(g => g.Group(It.IsAny<IEnumerable<LocationGroup>>()), Times.Once);
        }

        [Fact]
        public void Search_WithSmallSingleGroupReturned_RequeriesRepo()
        {
            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(LocationService.GROUPING_THRESHOLD - 1);

            _mockLocationRepository.Setup(r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> { new LocationGroup(_mockDataReader.Object, _mockGroupStrat.Object.Fields) });

            _sut.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationRepository.Verify(r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Exactly(2));
        }

        [Fact]
        public void Search_WithLargeSingleGroupReturned_RequeriesRepo()
        {
            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(LocationService.GROUPING_THRESHOLD + 1);

            _mockLocationRepository.Setup(r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> { new LocationGroup(_mockDataReader.Object, _mockGroupStrat.Object.Fields) });

            _sut.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationRepository.Verify(r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Exactly(2));
        }

        [Fact]
        public void Search_WithTwoGroupsAndLocationSumLessThanThreshold_RequeriesRepo()
        {
            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(10);

            _mockLocationRepository.Setup(
                r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> {
                    new LocationGroup(_mockDataReader.Object, _mockGroupStrat.Object.Fields),
                    new LocationGroup(_mockDataReader.Object, _mockGroupStrat.Object.Fields)
                });

            _sut.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationRepository.Verify(r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Exactly(2));
        }

        [Fact]
        public void Search_WithTwoGroupsAndLocationSumLowerThanThreshold_DoesRequeryRepo()
        {
            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(10);

            _mockLocationRepository.Setup(
                r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> {
                    new LocationGroup(_mockDataReader.Object, _mockGroupStrat.Object.Fields),
                    new LocationGroup(_mockDataReader.Object, _mockGroupStrat.Object.Fields)
                });

            _sut.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationRepository.Verify(r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Exactly(2));
        }

        [Fact]
        public void Search_WithTwoGroupsAndLocationSumGreaterThanThreshold_DoesntRequeryRepo()
        {

            _mockDataReader.Setup(r => r[POSTCODE_LOCATOR_FIELD]).Returns("SO22 2XX");
            _mockDataReader.Setup(r => r.FieldCount).Returns(2);
            _mockDataReader.Setup(r => r.GetName(0)).Returns("Number");
            _mockDataReader.Setup(r => r.GetName(1)).Returns(POSTCODE_LOCATOR_FIELD);
            _mockDataReader.Setup(r => r["Number"]).Returns(LocationService.GROUPING_THRESHOLD + 1);

            _mockLocationRepository.Setup(
                r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup> {
                    new LocationGroup(_mockDataReader.Object, _mockGroupStrat.Object.Fields),
                    new LocationGroup(_mockDataReader.Object, _mockGroupStrat.Object.Fields)
                });

            _sut.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationRepository.Verify(r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Once);
        }

        [Fact]
        public void Search_WithNoResults_DoesntRequeryRepo()
        {

            _mockLocationRepository.Setup(
                r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()))
                .Returns(new List<LocationGroup>());

            _sut.Search("SO22 2XX", _mockGroupStrat.Object);

            _mockLocationRepository.Verify(r => r.FindLocations(It.Is<string>(s => s == "so22 2xx"), It.IsAny<ILocationGroupingStrategy>()), Times.Once);
        }

        private readonly Mock<ILocationGroupingStrategy> _mockGroupStrat;

    }
}