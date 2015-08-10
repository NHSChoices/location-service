using System.Linq;

namespace GoatTrip.DAL.Tests
{
    using System;
    using Xunit;

    public class LocationGroupingStrategyBuilderTests
    {
        [Fact()]
        public void LocationGroupBy_Single_Fields_Test()
        {
            var result = new LocationGroupingStrategyBuilder(LocationQueryField.HouseNumber)
                .Build();

            Assert.NotNull(result.Fields);
            Assert.Equal(1, result.Fields.Count());
            Assert.Equal("PAO_START_NUMBER", result.Fields.First().Name);
        }

        [Fact()]
        public void LocationGroupBy_AndBy_Fields_Test()
        {
            var result = new LocationGroupingStrategyBuilder(LocationQueryField.HouseNumber)
                            .ThenBy(LocationQueryField.Street)
                            .Build();

            Assert.NotNull(result.Fields);
            Assert.Equal(2, result.Fields.Count());
            Assert.True(result.Fields.Any(f => f.Name == "PAO_START_NUMBER"));
            Assert.True(result.Fields.Any(f => f.Name == "STREET_DESCRIPTION"));
        }

        [Fact()]
        public void LocationGroupBy_Returns_Grouped_Fields()
        {
            var result = new LocationGroupingStrategyBuilder(LocationQueryField.HouseNumber)
                .ThenBy(LocationQueryField.Street)
                .ThenBy(LocationQueryField.Town)
                .Build();

            Assert.NotNull(result.Fields);
            Assert.Equal(result.Fields.Count(), 3);
            Assert.True(result.Fields.Any(f => f.Name == "PAO_START_NUMBER"));
            Assert.True(result.Fields.Any(f => f.Name == "STREET_DESCRIPTION"));
            Assert.True(result.Fields.Any(f => f.Name == "TOWN_NAME"));
        }

        [Fact()]
        public void LocationGroupBy_Returns_Grouped_Fields_Correct_Order()
        {
            var result = new LocationGroupingStrategyBuilder(LocationQueryField.HouseNumber)
                .ThenBy(LocationQueryField.Street)
                .ThenBy(LocationQueryField.Town)
                .Build();

            Assert.NotNull(result.Fields);
            Assert.Equal(result.Fields.Count(), 3);
            Assert.Equal(LocationQueryField.HouseNumber.Key, result.Fields.ElementAt(0).Key);
            Assert.Equal(LocationQueryField.Street.Key, result.Fields.ElementAt(1).Key);
            Assert.Equal(LocationQueryField.Town.Key, result.Fields.ElementAt(2).Key);
        }

    
    }
}
