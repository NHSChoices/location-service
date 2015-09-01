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

namespace GoatTrip.DAL
{
    public class LocationGroupRepository : ILocationGroupRepository
    {
        public LocationGroupRepository(IGroupIndexSearcher searcher, ILocationGroupBuilder locationGroupBuilder)
        {
            _searcher = searcher;
            _locationGroupBuilder = locationGroupBuilder;
        }

        private ILocationGroupBuilder _locationGroupBuilder;


        public IEnumerable<LocationGroup> FindGroupedLocations(string query, ILocationGroupingStrategy groupingStrategy)
        {

            var wildcardQuery = query.Trim() + "*";

            var luceneQuery = GenerateLuceneQuery(wildcardQuery, groupingStrategy);
            var collector = new GroupCollector(_locationGroupBuilder, groupingStrategy.Fields);
            _searcher.Search(luceneQuery, collector);
            return collector.Groups;
        }

        private Query GenerateLuceneQuery(string query, ILocationGroupingStrategy groupingStrategy)
        {
            var parser = new MultiFieldQueryParser(Version.LUCENE_29,
             groupingStrategy.Fields.Select(f => f.Name).ToArray(),
             _analyzer);
            parser.DefaultOperator = QueryParser.Operator.AND;
            parser.FuzzyPrefixLength = 3;
            Query luceneQuery = parser.Parse(query);
            return luceneQuery;
        }
        private Analyzer  _analyzer = new StandardAnalyzer(Version.LUCENE_29);
        private readonly IGroupIndexSearcher _searcher;
    }
}
