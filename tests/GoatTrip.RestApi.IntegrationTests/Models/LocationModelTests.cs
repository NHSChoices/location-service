﻿
namespace GoatTrip.RestApi.IntegrationTests.Models {
    using System;
    using System.Data;
    using System.Linq;
    using DAL.DTOs;
    using Moq;
    using RestApi.Models;
    using Xunit;

    [Trait("Category", "integration")]
    public class LocationModelTests {
        private readonly Mock<IDataRecord> _mockDataReader;

        //private readonly IEnumerable<string> _dtoPropertiesBlackList;
        //private readonly IEnumerable<string> _modelPropertiesBlackList;

        public LocationModelTests() {
            _mockDataReader = new Mock<IDataRecord>();

            _mockDataReader.Setup(r => r[It.IsAny<string>()]).Returns<string>(a => a);
            _mockDataReader.Setup(r => r[It.Is<string>(x => x == "X_COORDINATE")]).Returns<string>(a => "1.0");
            _mockDataReader.Setup(r => r[It.Is<string>(y => y == "Y_COORDINATE")]).Returns<string>(a => "2.0");
        }

        [Fact]
        public void Ctor_WithLocation_MapsAllFields() {
            
            var model = new Location(_mockDataReader.Object);

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

