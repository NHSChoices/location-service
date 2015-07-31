
namespace GoatTrip.RestApi.UnitTests.Models {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using DAL.DTOs;
    using Moq;
    using RestApi.Models;
    using Xunit;

    public class LocationModelTests {
        private readonly Mock<IDataRecord> _mockDataReader;

        //private readonly IEnumerable<string> _dtoPropertiesBlackList;
        //private readonly IEnumerable<string> _modelPropertiesBlackList;

        public LocationModelTests() {
            _mockDataReader = new Mock<IDataRecord>();

            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "ADMINISTRATIVE_AREA")]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "BUILDING_NAME")]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "BLPU_ORGANISATION")]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "STREET_DESCRIPTION")]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "PAO_START_NUMBER")]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "LOCALITY")]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "TOWN_NAME")]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "POST_TOWN")]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "POSTCODE")]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "X_COORDINATE")]).Returns<string>(a => "1.0");
            _mockDataReader.Setup(r => r[It.Is<string>(y => y == "Y_COORDINATE")]).Returns<string>(a => "2.0");
        }

        [Fact]
        public void Ctor_WithLocation_MapsAllFields() {
            
            var model = new Location(_mockDataReader.Object);

            var sut = new LocationModel(model);

            //mapping
            Assert.Equal("BLPU_ORGANISATION", sut.OrganisationName);
            Assert.Equal("BUILDING_NAME", sut.BuildingName);
            Assert.Equal("STREET_DESCRIPTION", sut.StreetDescription);
            Assert.Equal("PAO_START_NUMBER", sut.HouseNumber);
            Assert.Equal("LOCALITY", sut.Locality);
            Assert.Equal("TOWN_NAME", sut.TownName);
            Assert.Equal("ADMINISTRATIVE_AREA", sut.AdministrativeArea);
            Assert.Equal("POST_TOWN", sut.PostTown);
            Assert.Equal("POSTCODE", sut.Postcode);
            //Assert.Equal("POSTCODE_LOCATOR", sut.PostcodeLocator);
            Assert.NotNull(sut.Coordinate);
            Assert.Equal(1.0f, sut.Coordinate.X);
            Assert.Equal(2.0f, sut.Coordinate.Y);

            var dtoSettableProperties = typeof (Location).GetProperties().Where(p => p.GetSetMethod() != null);
            var modelSettableProperties = typeof (LocationModel).GetProperties().Where(p => p.GetSetMethod() != null);

            //modelSettableProperties.Where(p => p.)

            //missing fields from DTO
            foreach (var prop in modelSettableProperties) {
                Console.WriteLine("Testing " + prop.Name + " has been mapped.");
                Assert.NotNull(prop.GetValue(sut));
            }

            //missing fields on model
        }
    }
}

