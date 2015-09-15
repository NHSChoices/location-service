

namespace GoatTrip.RestApi.IntegrationTests.Models {
    using Services;
    using System;
    using System.Data;
    using System.Linq;
    using Common.Formatters;
    using DAL.DTOs;
    using RestApi.Models;
    using Moq;
    using Xunit;

    [Trait("Category", "integration")]
    public class LocationModelMapperTests {
        public class LocationModelTests {

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
            public void Map_WithLocation_MapsAllFields() {

                var model = new Location(_mockDataReader.Object, _mockLocationFormatter.Object);

                var sut = new LocationModelMapper();
                var result = sut.Map(model);

                //mapping
                Assert.Equal(model.OrganisationName, result.OrganisationName);
                Assert.Equal(model.BuildingName, result.BuildingName);
                Assert.Equal(model.StreetDescription, result.StreetDescription);
                Assert.Equal(model.HouseNumber, result.HouseNumber);
                Assert.Equal(model.Locality, result.Locality);
                Assert.Equal(model.TownName, result.TownName);
                Assert.Equal(model.AdministrativeArea, result.AdministrativeArea);
                Assert.Equal(model.PostalTown, result.PostTown);
                Assert.Equal(model.PostCode, result.Postcode);
                Assert.Equal(model.PostcodeLocator, result.PostcodeLocator);
                Assert.NotNull(result.Coordinate);
                Assert.Equal(1.0f, result.Coordinate.X);
                Assert.Equal(2.0f, result.Coordinate.Y);

                var dtoSettableProperties = typeof (Location).GetProperties().Where(p => p.GetSetMethod() != null);
                var modelSettableProperties = typeof (LocationModel).GetProperties()
                    .Where(p => p.GetSetMethod() != null);

                //modelSettableProperties.Where(p => p.)

                //missing fields on model
                foreach (var prop in modelSettableProperties) {
                    Console.Write("Testing LocationModel." + prop.Name + " has been mapped...");
                    Assert.NotNull(prop.GetValue(result));
                    Console.WriteLine("pass");
                }

                //missing fields from DTO

            }

            [Fact]
            public void Map_WithLocationWithZeroHouseNumber_ExcludesHouseNumberInAddressLines() {
                _mockDataReader.Setup(r => r[LocationFields.PaoStartNumber]).Returns(0);
                var model = new Location(_mockDataReader.Object, _mockLocationFormatter.Object);

                var sut = new LocationModelMapper();
                var result = sut.Map(model);

                Assert.False(result.AddressLines.Any(a => a.Contains("0")));
            }

            [Fact]
            public void Map_WithLocationWith7Fields_Returns7AddressLines() {
                var model = new Location(_mockDataReader.Object, _mockLocationFormatter.Object);

                var sut = new LocationModelMapper();
                var result = sut.Map(model);

                Assert.Equal(7, result.AddressLines.Count());
            }

            [Fact]
            public void Map_WithDuplicateFields_DoesntRepeatLines()
            {
                _mockDataReader.Setup(r => r[LocationFields.BlpuOrganisation]).Returns("something");
                _mockDataReader.Setup(r => r[LocationFields.BuildingName]).Returns("something");
                var model = new Location(_mockDataReader.Object, _mockLocationFormatter.Object);

                var sut = new LocationModelMapper();
                var result = sut.Map(model);

                Assert.Equal(6, result.AddressLines.Count());
            }



            private readonly Mock<IDataRecord> _mockDataReader;
            private readonly Mock<IConditionalFormatter<string, string>> _mockLocationFormatter;
            //private readonly IEnumerable<string> _dtoPropertiesBlackList;
            //private readonly IEnumerable<string> _modelPropertiesBlackList;
        }
    }
}
