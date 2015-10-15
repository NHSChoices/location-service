
namespace GoatTrip.DAL
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Lucene.Net.Analysis;
    using global::Lucene.Net.Analysis.Standard;
    using global::Lucene.Net.QueryParsers;
    using global::Lucene.Net.Search;
    using Version = global::Lucene.Net.Util.Version;

    using DTOs;
    using Lucene;
    using Common.Collections.Generic;

    public class LocationGroupRepository : ILocationGroupRepository
    {
        public LocationGroupRepository(IGroupIndexSearcher searcher, ILocationGroupBuilder locationGroupBuilder, ILocationQueryFields locationQueryFields)
        {
            _searcher = searcher;
            _locationGroupBuilder = locationGroupBuilder;
            _queryFields = locationQueryFields;
        }

        private readonly ILocationGroupBuilder _locationGroupBuilder;


        public IEnumerable<LocationGroup> FindGroupedLocations(string query, ILocationGroupingStrategy groupingStrategy)
        {

            var wildcardQuery = query.Trim() + "*";
            var queryFields = groupingStrategy.Fields;
            if (query.Any(char.IsDigit))
               queryFields = queryFields.Concat(new List<LocationQueryField>() {_queryFields.HouseNumber});
            var luceneQuery = GenerateLuceneQuery(wildcardQuery, queryFields);
            var collector = new GroupCollector(_locationGroupBuilder, groupingStrategy.Fields);
            _searcher.Search(luceneQuery, collector);
            return collector.Groups.OrderBy(g => g.FirstOrderByField, new AlphanumericComparerFast()).ThenBy(g => g.SecondOrderByField, new AlphanumericComparerFast());
        }

        private Query GenerateLuceneQuery(string query, IEnumerable<LocationQueryField> fieldsToQuery)
        {
            var parser = new MultiFieldQueryParser(Version.LUCENE_29,
                fieldsToQuery.Select(f => f.Name).Distinct().ToArray(),
                _analyzer) {
                    DefaultOperator = QueryParser.Operator.AND,
                    FuzzyPrefixLength = 3
                };
            Query luceneQuery = parser.Parse(query);
            return luceneQuery;
        }


        private readonly Analyzer  _analyzer = new StandardAnalyzer(Version.LUCENE_29);
        private readonly IGroupIndexSearcher _searcher;
        private readonly ILocationQueryFields _queryFields;
    }
}
