
using System.Data;
using System.Runtime.InteropServices;
using FluentAssertions;
using Moq;
using Xunit;

namespace GoatTrip.DAL.UnitTests {
    public class LocationRepositoryTests {

        public LocationRepositoryTests() {
            _mockReader = new Mock<IManagedDataReader>();
            _mockLocationGroupBuilder = new Mock<ILocationGroupBuilder>();

            _mockConnectionManager = new Mock<IConnectionManager>();
            _mockConnectionManager.Setup(c => c.GetReader(It.IsAny<string>(), It.IsAny<StatementParamaters>()))
                .Returns(_mockReader.Object);

            _sut = new LocationRepository(_mockConnectionManager.Object, _mockLocationGroupBuilder.Object);
            _mockDataReader = new Mock<IDataReader>();
            _mockDataReader.Setup(r => r["ADMINISTRATIVE_AREA"]).Returns("");
            _mockDataReader.Setup(r => r["BUILDING_NAME"]).Returns("");
            _mockDataReader.Setup(r => r["BLPU_ORGANISATION"]).Returns("");
            _mockDataReader.Setup(r => r["STREET_DESCRIPTION"]).Returns("");
            _mockDataReader.Setup(r => r["PAO_START_NUMBER"]).Returns("");
            _mockDataReader.Setup(r => r["LOCALITY"]).Returns("");
            _mockDataReader.Setup(r => r["TOWN_NAME"]).Returns("");
            _mockDataReader.Setup(r => r["POST_TOWN"]).Returns("");
            _mockDataReader.Setup(r => r["POSTCODE"]).Returns("");
            _mockDataReader.Setup(r => r["X_COORDINATE"]).Returns("1.0");
            _mockDataReader.Setup(r => r["Y_COORDINATE"]).Returns("2.0");
            _mockDataReader.Setup(r => r["POSTCODE_LOCATOR"]).Returns("");

            _mockReader.Setup(r => r.DataReader).Returns(_mockDataReader.Object);
        }

        [Fact]
        public void FindLocationsbyAddress_WithNonexistantAddress_ReturnsEmptyCollection() {

            _mockReader.Setup(r => r.Read()).Returns(false);

            var result = _sut.FindLocationsbyAddress("anything");

            Assert.Empty(result);
        }

        [Fact]
        public void FindLocationsbyAddress_Always_CallsGetReader() {
            _sut.FindLocationsbyAddress("anything");

            _mockConnectionManager.Verify(c => c.GetReader(It.IsAny<string>(), It.IsAny<StatementParamaters>()));
        }

        [Fact]
        public void FindLocationsbyAddress_Always_CallsReadForEachRecord() {

            _mockReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(false);

            _sut.FindLocationsbyAddress("anything");

            _mockReader.Verify(r => r.Read(), Times.Exactly(4));
        }

        [Fact]
        public void FindLocationsbyAddress_WithRecordsMatched_ReturnsMatchedRecords() {
            _mockReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(false);

            _mockDataReader.SetupSequence(r => r["ADMINISTRATIVE_AREA"])
                .Returns("1")
                .Returns("1")
                .Returns("2")
                .Returns("2")
                .Returns("3")
                .Returns("3");

            var result = _sut.FindLocationsbyAddress("anything");

            result.Should().HaveCount(3)
                .And.ContainSingle(l => l.AdministrativeArea == "1")
                .And.ContainSingle(l => l.AdministrativeArea == "2")
                .And.ContainSingle(l => l.AdministrativeArea == "3");
        }

        private readonly LocationRepository _sut;
        private readonly Mock<IManagedDataReader> _mockReader;
        private readonly Mock<IConnectionManager> _mockConnectionManager;
        private readonly Mock<IDataReader> _mockDataReader;
        private readonly Mock<ILocationGroupBuilder> _mockLocationGroupBuilder;

    }
}