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
    public class LocationGroupTests
    {
        private Mock<IDataRecord> _mockDataRecord;
        private IEnumerable<LocationQueryField> _queryFields;

        public LocationGroupTests() {
            _queryFields =  new List<LocationQueryField> { LocationQueryField.HouseNumber, LocationQueryField.Street, LocationQueryField.Town };

            _mockDataRecord = new Mock<IDataRecord>();
            _mockDataRecord.Setup(r => r[It.IsAny<string>()]).Returns("");
            _mockDataRecord.Setup(r => r[It.Is<string>(x => x == "PAO_START_NUMBER")]).Returns("22");
            _mockDataRecord.Setup(r => r[It.Is<string>(y => y == "STREET_DESCRIPTION")]).Returns("Test Road");
            _mockDataRecord.Setup(r => r[It.Is<string>(z => z == "TOWN_NAME")]).Returns("TestTown");
            _mockDataRecord.Setup(r => r[It.Is<string>(z => z == "Number")]).Returns(32);
        }

        [Fact()]
        public void LocationGroup_With_Reader_Returns_Count_Test()
        {
            var result = new LocationGroup(_mockDataRecord.Object, _queryFields);
            Assert.Equal(32,result.LocationsCount);
           
        }

        [Fact()]
        public void LocationGroup_With_Reader_Returns_Description_Test()
        {
            var result = new LocationGroup(_mockDataRecord.Object, _queryFields);
            Assert.Equal("22, Test Road, TestTown",result.GroupDescription);
        }

        [Fact()]
        public void LocationGroup_With_Reader_Returns_GroupFields_Test()
        {
            var result = new LocationGroup(_mockDataRecord.Object, _queryFields);
            Assert.Equal(3,result.GroupFields.Count());
            Assert.Equal("22",result.GroupFields[LocationDataField.HouseNumber]);
            Assert.Equal("Test Road",result.GroupFields[LocationDataField.Street]);
            Assert.Equal("TestTown",result.GroupFields[LocationDataField.Town]);
        }
    }
}
