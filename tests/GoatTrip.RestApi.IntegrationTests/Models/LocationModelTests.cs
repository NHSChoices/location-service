
using System;
using System.Data;
using System.Linq;
using GoatTrip.Common.Formatters;
using GoatTrip.DAL;
using GoatTrip.DAL.DTOs;
using GoatTrip.RestApi.Models;
using Moq;
using Xunit;

namespace GoatTrip.RestApi.IntegrationTests.Models {
    [Trait("Category", "integration")]
    public class LocationModelTests {
        private readonly Mock<IDataRecord> _mockDataReader;
        private readonly Mock<IConditionalFormatter<string, string>> _mockLocationFormatter;

        //private readonly IEnumerable<string> _dtoPropertiesBlackList;
        //private readonly IEnumerable<string> _modelPropertiesBlackList;

        public LocationModelTests() {
            _mockDataReader = new Mock<IDataRecord>();
            _mockLocationFormatter = new Mock<IConditionalFormatter<string, string>>();

            _mockDataReader.Setup(r => r[It.IsAny<string>()]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r["UPRN"]).Returns<string>(a => "1");
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "X_COORDINATE")]).Returns<string>(a => "1.0");
            _mockDataReader.Setup(r => r[It.Is<string>(y => y == "Y_COORDINATE")]).Returns<string>(a => "2.0");

            _mockLocationFormatter.Setup(r => r.DetermineConditionsAndFormat(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string value, string type) => value);
        }

        [Fact]
        public void Ctor_WithLocation_MapsAllFields() {

            var model = new Location(_mockDataReader.Object, _mockLocationFormatter.Object);

            var sut = new LocationModel(model);

            //mapping
            Assert.Equal(model.OrganisationName, sut.OrganisationName);
            Assert.Equal(model.BuildingName, sut.BuildingName);
            Assert.Equal(model.StreetDescription, sut.StreetDescription);
            Assert.Equal(model.HouseNumber, sut.HouseNumber);
            Assert.Equal(model.Localiry, sut.Locality);
            Assert.Equal(model.TownName, sut.TownName);
            Assert.Equal(model.AdministrativeArea, sut.AdministrativeArea);
            Assert.Equal(model.PostalTown, sut.PostTown);
            Assert.Equal(model.PostCode, sut.Postcode);
            Assert.Equal(model.PostcodeLocator, sut.PostcodeLocator);
            Assert.NotNull(sut.Coordinate);
            Assert.Equal(1.0f, sut.Coordinate.X);
            Assert.Equal(2.0f, sut.Coordinate.Y);

            var dtoSettableProperties = typeof (Location).GetProperties().Where(p => p.GetSetMethod() != null);
            var modelSettableProperties = typeof (LocationModel).GetProperties().Where(p => p.GetSetMethod() != null);

            //modelSettableProperties.Where(p => p.)

            //missing fields on model
            foreach (var prop in modelSettableProperties) {
                Console.Write("Testing LocationModel." + prop.Name + " has been mapped...");
                Assert.NotNull(prop.GetValue(sut));
                Console.WriteLine("pass");
            }

            //missing fields from DTO

        }
    }
}

