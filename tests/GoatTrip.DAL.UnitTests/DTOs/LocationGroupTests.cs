using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL.DTOs;
using Moq;
using Xunit;
namespace GoatTrip.DAL.DTOs.Tests
{
    public class LocationGroupBuilderTests
    {
        private Mock<IDataRecord> _mockDataRecord;
        private IEnumerable<LocationQueryField> _queryFields;
        private LocationGroupBuilder _builder;
        private ILocationQueryFields _locationQueryFields;

        public LocationGroupBuilderTests()
        {
            _builder = new LocationGroupBuilder();
            _locationQueryFields = new SqlIteLocationQueryFields();
            _queryFields = new List<LocationQueryField> { _locationQueryFields.HouseNumber, _locationQueryFields.Street, _locationQueryFields.Town };

            _mockDataRecord = new Mock<IDataRecord>();
            _mockDataRecord.Setup(r => r[It.IsAny<string>()]).Returns("");
            _mockDataRecord.Setup(r => r[It.Is<string>(x => x == "PAO_START_NUMBER")]).Returns("22");
            _mockDataRecord.Setup(r => r[It.Is<string>(b => b == "PAO_START_SUFFIX")]).Returns("");
            _mockDataRecord.Setup(r => r[It.Is<string>(y => y == "STREET_DESCRIPTION")]).Returns("Test Road");
            _mockDataRecord.Setup(r => r[It.Is<string>(z => z == "TOWN_NAME")]).Returns("TestTown");
            _mockDataRecord.Setup(r => r[It.Is<string>(z => z == "POSTCODE")]).Returns("TS17 TTT");
            _mockDataRecord.Setup(r => r[It.Is<string>(z => z == "Number")]).Returns(32);
        }

        [Fact()]
        public void LocationGroup_With_Reader_Returns_Count_Test()
        {
            _mockDataRecord.Setup(r => r.FieldCount).Returns(1);
            _mockDataRecord.Setup(r => r["Number"]).Returns(32);
            _mockDataRecord.Setup(r => r.GetName(0)).Returns("Number");

            var result = _builder.Build(_mockDataRecord.Object, _queryFields);
            Assert.Equal(32,result.LocationsCount);
           
        }

        [Fact()]
        public void LocationGroup_With_No_HouseNumber_Returns_Description_Test()
        {
            var queryFields = new List<LocationQueryField> { _locationQueryFields.Street, _locationQueryFields.Town, _locationQueryFields.PostCode };
            var result = _builder.Build(_mockDataRecord.Object, queryFields);
            Assert.Equal("Test Road, TestTown, TS17 TTT", result.GroupDescription);
        }

        [Fact()]
        public void LocationGroup_With_Reader_Returns_Description_Test()
        {
            var result = _builder.Build(_mockDataRecord.Object, _queryFields);
            Assert.Equal("22, Test Road, TestTown", result.GroupDescription);
        }

        [Fact()]
        public void LocationGroup_With_Reader_AND_Suffix_Returns_Description_Test()
        {
            _mockDataRecord.Setup(r => r[It.Is<string>(x => x == "PAO_START_SUFFIX")]).Returns("A");
            var queryFields = new List<LocationQueryField> { _locationQueryFields.HouseNumber, _locationQueryFields.HouseSuffix, _locationQueryFields.Street, _locationQueryFields.Town };
            var result = _builder.Build(_mockDataRecord.Object, queryFields);
            Assert.Equal("22A, Test Road, TestTown",result.GroupDescription);
        }

        [Fact()]
        public void LocationGroup_With_Reader_AND_SAOText_Returns_Description_Test()
        {
            _mockDataRecord.Setup(r => r[It.Is<string>(x => x == "PAO_START_SUFFIX")]).Returns("A");
            var queryFields = new List<LocationQueryField> { _locationQueryFields.HouseNumber, _locationQueryFields.HouseSuffix, _locationQueryFields.Street, _locationQueryFields.Town };
            var result = _builder.Build(_mockDataRecord.Object, queryFields);
            Assert.Equal("22A, Test Road, TestTown", result.GroupDescription);
        }

        [Fact()]
        public void LocationGroup_With_Reader_AND_PAOText_Returns_Description_Test()
        {
            _mockDataRecord.Setup(r => r[It.Is<string>(x => x == "PAO_TEXT")]).Returns("Some address detail");
            _mockDataRecord.Setup(r => r[It.Is<string>(x => x == "SAO_TEXT")]).Returns("Sub detail");
            var queryFields = new List<LocationQueryField> { _locationQueryFields.PrimaryText, _locationQueryFields.SecondaryText, _locationQueryFields.HouseNumber, _locationQueryFields.HouseSuffix, _locationQueryFields.Street, _locationQueryFields.Town };
            var result = _builder.Build(_mockDataRecord.Object, queryFields);
            Assert.Equal("Some address detail, Sub detail, 22, Test Road, TestTown", result.GroupDescription);
        }



                   

        [Fact()]
        public void LocationGroup_With_Reader_Returns_GroupFields_Test()
        {
            var result = _builder.Build(_mockDataRecord.Object, _queryFields);
            Assert.Equal(3,result.GroupFields.Count());
            Assert.Equal("22",result.GroupFields[LocationDataField.HouseNumber]);
            Assert.Equal("Test Road",result.GroupFields[LocationDataField.Street]);
            Assert.Equal("TestTown",result.GroupFields[LocationDataField.Town]);
        }
    }
}
