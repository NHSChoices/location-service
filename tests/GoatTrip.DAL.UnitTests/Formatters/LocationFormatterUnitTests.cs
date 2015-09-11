using GoatTrip.Common.Formatters;
using GoatTrip.DAL.DTOs;
using GoatTrip.DAL.Formatters;
using Xunit;

namespace GoatTrip.DAL.UnitTests.Formatters
{
    public class LocationFormatterUnitTests
    {
        public LocationFormatterUnitTests()
        {
            
        }

        [Fact()]
        public void LocationDataFieldFormatConditions()
        {
            IFormatConditions<LocationDataField> sut = new LocationDataFieldFormatConditions();
            Assert.False(sut.ShouldFormat(LocationDataField.PostCode));
            Assert.True(sut.ShouldFormat(LocationDataField.Street));
        }

        [Fact()]
        public void LocationFormatCondition()
        {
            IFormatConditions<string> sut = new LocationFormatConditions();
            Assert.True(sut.ShouldFormat(LocationFields.StreetDescription));
            Assert.False(sut.ShouldFormat(LocationFields.Postcode));
        }
    }
}
