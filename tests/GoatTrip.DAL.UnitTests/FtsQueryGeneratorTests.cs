using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL;
using Moq;
using Xunit;
namespace GoatTrip.DAL.Tests
{
    public class FtsQueryGeneratorTests
    {
        private  Mock<IfTSQueryTokenizer> _mockTokenizer;

        private Mock<ILocationGroupingStrategy>  _mockGroupingStrategy;

        public FtsQueryGeneratorTests()
        {
            _mockTokenizer = new Mock<IfTSQueryTokenizer>();
           

            _mockGroupingStrategy = new Mock<ILocationGroupingStrategy>();
            _mockGroupingStrategy.Setup(g => g.Fields)
                .Returns(new List<LocationQueryField>() {LocationQueryField.Town, LocationQueryField.PostCode});

        }

        [Fact()]
        public void GeneratefTSSearchQuery_singleToken_Test()
        {
            _mockTokenizer.Setup(t => t.Tokens).Returns(new string[] { "SingleToken" });
            _mockTokenizer.Setup(t => t.GetMatchQuery()).Returns("SingleToken*");

            var queryGenerator = new FtsQueryGenerator(_mockGroupingStrategy.Object, _mockTokenizer.Object);
            var expected =
                "SELECT locations.TOWN_NAME,locations.POSTCODE, COUNT(*) as Number " +
                "from locations JOIN locations_srch ON locations.locationId = locations_srch.docid " +
                "WHERE locations_srch MATCH 'SingleToken*' GROUP BY locations.TOWN_NAME,locations.POSTCODE " +
                "ORDER by Number desc LIMIT 100;";

            Assert.Equal(expected, queryGenerator.GeneratefTSSearchQuery());
        }

        [Fact()]
        public void GeneratefTSSearchQuery_multipleTokens_Test()
        {
            _mockTokenizer.Setup(t => t.Tokens).Returns(new string[] { "multiple","Tokens", "Test" });
            _mockTokenizer.Setup(t => t.GetMatchQuery()).Returns("multiple Tokens");
            _mockTokenizer.Setup(t => t.GetLikeQuery()).Returns("%Test%");

            var queryGenerator = new FtsQueryGenerator(_mockGroupingStrategy.Object, _mockTokenizer.Object);
            var expected =
                "SELECT matchResults.* FROM (SELECT locations.TOWN_NAME,locations.POSTCODE, COUNT(*) as Number " +
                "from locations JOIN locations_srch ON locations.locationId = locations_srch.docid " +
                "WHERE locations_srch MATCH 'multiple Tokens' " +
                "GROUP BY locations.TOWN_NAME,locations.POSTCODE ORDER by Number desc ) as matchResults " +
                "WHERE matchResults.TOWN_NAME like '%Test%' " +
                "OR matchResults.POSTCODE like '%Test%' " +
                "LIMIT 100;";

            Assert.Equal(expected, queryGenerator.GeneratefTSSearchQuery());
        }
    }
}
