using System.Linq;

namespace GoatTrip.DAL
{
    public class FtsQueryGenerator
    {
        private IfTSQueryTokenizer _ftsTokenizer;
        private ILocationGroupingStrategy _locationGroupingStrategy;
        private const string ALIAS_PREFIX = "locations";
        public FtsQueryGenerator(ILocationGroupingStrategy groupingStrategy, IfTSQueryTokenizer fTsQueryTokenizer)
        {
            _locationGroupingStrategy = groupingStrategy;
            _ftsTokenizer = fTsQueryTokenizer;
        }

        public string Generate()
        {
            var query = "SELECT MAX(LocationId) as LocationId, " + LocationQueryField.Concatenate(_locationGroupingStrategy.Fields, ALIAS_PREFIX) + ", COUNT(*) as Number from locations " +
                        "JOIN locations_srch ON locations.locationId = locations_srch.docid " +
                        "WHERE locations_srch MATCH '"+ _ftsTokenizer.GetMatchQuery() +"' " +
                        "GROUP BY " + LocationQueryField.Concatenate(_locationGroupingStrategy.Fields, ALIAS_PREFIX) + " ORDER by Number desc ";

            if (_ftsTokenizer.Tokens.Length > 1)
            {
                query =
                    "SELECT matchResults.* FROM " +
                    "(" + query + ") as matchResults " +
                    "WHERE " + GenerateGroupLikeStatement();
            }

            query += "LIMIT 100;";
            return query;
        }

        private string GenerateGroupLikeStatement()
        {
           return _locationGroupingStrategy.Fields.Select(
                f => "matchResults." + f.Name + " like '" + _ftsTokenizer.GetLikeQuery() + "' ")
                .Aggregate((i, j) => i + "OR " + j);
        }


    }
}