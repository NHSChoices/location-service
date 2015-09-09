using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL;
using GoatTrip.DAL.Lucene;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace GoatTrip.LucenceIndexer
{
    public class QueryThread
    {
        private string _query = "";
        private IndexSearcher _searcher;
        private int _threadno = 0;
        private Analyzer _analyzer;
        private GroupCollector _collector;
        private Action<string> _callback;
        public QueryThread(string query, IndexSearcher searcher, int threadno, Analyzer analyzer, Action<string> callback)
        {
            _query = query;
            _searcher = searcher;
            _threadno = threadno;
            _analyzer = analyzer;
            _callback = callback;
            var queryFields = new LuceneQueryFields();
            var collector = new GroupCollector(new LocationGroupBuilder()
                , new List<LocationQueryField>()
                {
                    queryFields.Street,
                    queryFields.Town,
                    queryFields.PostCode
                });
            _collector = collector;

        }

        public void Query()
        {
            Stopwatch t = new Stopwatch();
            t.Start();

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, new string[] { "StartNumber", "Street", "Town", "Postcode" }, _analyzer);
            parser.DefaultOperator = QueryParser.Operator.AND;
            Query query = parser.Parse(_query);


            var collector = TopScoreDocCollector.Create(100, true);

            TopDocs hits = _searcher.Search(query, null, 100);

            //iterate over the results.
            for (int i = 0; i < 100 && i < hits.ScoreDocs.Length; i++)
            {
                Document doc = _searcher.Doc(hits.ScoreDocs[i].Doc);
                string contentValue = doc.Get("StartNumber") + "," + doc.Get("Street") + "," + doc.Get("Town") + "," + doc.Get("Postcode");
              //  if (_callback != null) _callback(contentValue);
            }
            t.Stop();
            if(_callback != null) _callback("Thread no " + _threadno + " completed in " + t.ElapsedMilliseconds + "ms");
        }

        public void Query_grouped()
        {
            Stopwatch t = new Stopwatch();
            t.Start();

            var parser = new MultiFieldQueryParser(Version.LUCENE_29, new string[] { "StartNumber", "Street", "Town", "Postcode" }, _analyzer);
            parser.DefaultOperator = QueryParser.Operator.AND;
            Query query = parser.Parse(_query);

            _searcher.Search(query, _collector);
            for (int i = 0; i < _collector.Groups.Count; i++)
            {
                var doc = _collector.Groups[i];
                string contentValue = doc.GroupDescription + " " + doc.LocationsCount;
              //  if (_callback != null) _callback(contentValue);
            }
            t.Stop();
            if (_callback != null) _callback("Thread no " + _threadno + " completed in " + t.ElapsedMilliseconds + "ms");
        }



    }
}
