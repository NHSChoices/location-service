using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace GoatTrip.DAL.Lucene
{
    public class LucenceIndexManager : ILocationIndexManager
    {
        public LucenceIndexManager(ILuceneDirectoryAdapter directoryAdaptor)
           
        {
            _searcher = new GroupedIndexSearcher(directoryAdaptor.GeDirectory());
            _analyzer = new StandardAnalyzer(Version.LUCENE_29);
        }



        public IGroupIndexSearcher IndexSearcher
        {
            get { return _searcher; }
        }

        public Query GenerateLuceneQuery(string query, ILocationGroupingStrategy groupingStrategy)
        {
            var parser = new MultiFieldQueryParser(Version.LUCENE_29,
             groupingStrategy.Fields.Select(f => f.Name).ToArray(),
             _analyzer);
            parser.DefaultOperator = QueryParser.Operator.AND;
            Query luceneQuery = parser.Parse(query);
            return luceneQuery;
        }


        private readonly string _indexDirectoryPath;
        private readonly Analyzer _analyzer;
        private readonly Directory _indexDirectory;
        private readonly GroupedIndexSearcher _searcher;
    }
}
