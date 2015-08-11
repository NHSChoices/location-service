using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL;
using Xunit;
namespace GoatTrip.DAL.Tests
{
    public class LocationGroupByTests
    {
        [Fact()]
        public void LocationGroupBy_Single_Fields_Test()
        {
            var result = new LocationGroupByStringBuilder(LocationDataField.HouseNumber).ToString();
            Assert.Equal("PAO_START_NUMBER", result);
        }

        [Fact()]
        public void LocationGroupBy_AndBy_Fields_Test()
        {
            var result = new LocationGroupByStringBuilder(LocationDataField.HouseNumber)
                            .ThenBy(LocationDataField.Street).ToString();
            Assert.Equal("PAO_START_NUMBER,STREET_DESCRIPTION", result);
        }

        [Fact()]
        public void LocationGroupBy_Invalid_Data_Field()
        {
            Assert.Throws<ArgumentException>(() => new LocationGroupByStringBuilder((LocationDataField)999).ToString());

        }
        [Fact()]
        public void LocationGroupBy_Returns_Grouped_Fields()
        {

            var result = new LocationGroupByStringBuilder(LocationDataField.HouseNumber)
                .ThenBy(LocationDataField.Street)
                .ThenBy(LocationDataField.Town);

            Assert.Equal(result.GroupByFields.Count(), 3);
            Assert.Equal(result.GroupByFields[LocationDataField.HouseNumber], "PAO_START_NUMBER");
            Assert.Equal(result.GroupByFields[LocationDataField.Street], "STREET_DESCRIPTION");
            Assert.Equal(result.GroupByFields[LocationDataField.Town], "TOWN_NAME");

        }

        [Fact()]
        public void LocationGroupBy_Returns_Grouped_Fields_Correct_Order()
        {

            var result = new LocationGroupByStringBuilder(LocationDataField.HouseNumber)
                .ThenBy(LocationDataField.Street)
                .ThenBy(LocationDataField.Town);

            Assert.Equal(LocationDataField.HouseNumber, result.GroupByFields.ElementAt(0).Key);
            Assert.Equal(LocationDataField.Street, result.GroupByFields.ElementAt(1).Key);
            Assert.Equal(LocationDataField.Town, result.GroupByFields.ElementAt(2).Key);
        }

    
    }
}
