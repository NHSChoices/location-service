using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL.DTOs;
using GoatTrip.DAL.Lucene;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;
using GoatTrip.Common.Collections.Generic;

namespace GoatTrip.DAL
{
    public class LocationGroupRepository : ILocationGroupRepository
    {
        public LocationGroupRepository(IGroupIndexSearcher searcher, ILocationGroupBuilder locationGroupBuilder, ILocationQueryFields locationQueryFields)
        {
            _searcher = searcher;
            _locationGroupBuilder = locationGroupBuilder;
            _queryFields = locationQueryFields;
        }

        private ILocationGroupBuilder _locationGroupBuilder;


        public IEnumerable<LocationGroup> FindGroupedLocations(string query, ILocationGroupingStrategy groupingStrategy)
        {

            var wildcardQuery = query.Trim() + "*";
            var queryFields = groupingStrategy.Fields;
            if (query.Any(c => Char.IsDigit(c)))
               queryFields = queryFields.Concat(new List<LocationQueryField>() {_queryFields.HouseNumber});
            var luceneQuery = GenerateLuceneQuery(wildcardQuery, queryFields);
            var collector = new GroupCollector(_locationGroupBuilder, groupingStrategy.Fields);
            _searcher.Search(luceneQuery, collector);
            return collector.Groups.OrderBy(g => g.GroupDescription, new AlphanumericComparerFast());
        }

        private Query GenerateLuceneQuery(string query, IEnumerable<LocationQueryField> fieldsToQuery)
        {
            var parser = new MultiFieldQueryParser(Version.LUCENE_29,
             fieldsToQuery.Select(f => f.Name).Distinct().ToArray(),
             _analyzer);
            parser.DefaultOperator = QueryParser.Operator.AND;
            parser.FuzzyPrefixLength = 3;
            Query luceneQuery = parser.Parse(query);
            return luceneQuery;
        }


        private Analyzer  _analyzer = new StandardAnalyzer(Version.LUCENE_29);
        private readonly IGroupIndexSearcher _searcher;
        private readonly ILocationQueryFields _queryFields;
    }
}
