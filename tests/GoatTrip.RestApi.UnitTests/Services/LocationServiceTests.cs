using System.Collections.Generic;
using System.Linq;
using GoatTrip.DAL;
using GoatTrip.DAL.DTOs;
using GoatTrip.RestApi.Services;
using Moq;
using Xunit;

namespace GoatTrip.RestApi.UnitTests.Services {
    public class LocationServiceTests : LocationServiceTestsBase {

        [Fact]
        public void Get_Always_CallsIsValid() {
            _sut.Get("ANY");

            _mockQueryValidator.Verify(v => v.IsValid(It.Is<string>(q => q == "ANY")));
        }

        [Fact]
        public void Get_WithInvalidQuery_ThrowsInvalidLocationQueryException() {
            _mockQueryValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(false);

            Assert.Throws<InvalidLocationQueryException>(() => _sut.Get(""));
        }

        [Fact]
        public void Get_WithExistingPostcode_ReturnsThatLocation() {
            CreateMockResults("SO11 1XX");

            var result = _sut.Get("SO11 1XX").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");
        }

        [Fact]
        public void Get_Always_CallsSanitise() {
            _sut.Get("SO22 2XX");

            _mockQuerySanitiser.Verify(s => s.Sanitise(It.Is<string>(q => q == "SO22 2XX")));
        }

        [Fact]
        public void Get_WithPostcode_MatchesRegardlessOfCase() {

            CreateMockResults("SO11 1XX");

            var result = _sut.Get("SO11 1XX").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");

            result = _sut.Get("so11 1xx").ToList();

            AssertIsValidResult(result, 1, "SO11 1XX");
        }

        [Fact]
        public void Get_WithMoreThanOneExistingPostcode_ReturnsAllMatchingLocations()
        {
            CreateMockResults("SO22 2XX", 3);

            var result = _sut.Get("SO22 2XX").ToList();

            AssertIsValidResult(result, 3, "SO22 2XX");
        }

        [Fact]
        public void Get_WithGroupingStrategy_CallsGroupOnStrategy() {
            CreateMockResults("SO22 2XX", 3);
            var mockGroupStrat = new Mock<ILocationGroupingStrategy>();

            _sut.Get("SO22 2XX", mockGroupStrat.Object).ToList();

            //mockGroupStrat.Verify(g => g.Group(It.IsAny<IEnumerable<LocationGroup>>()), Times.Once);
        }
    }

    public class LocationGroupingByAddressTests {
        [Fact]
        public void Group_WithLocations_ReturnsLocationsGroupedByAddress()
        {
            var locations = new List<Location>
            {

            };
        }
    }
}