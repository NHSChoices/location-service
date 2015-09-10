using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using GoatTrip.DAL.Lucene;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace GoatTrip.LucenceIndexer
{
    public class IndexPerformanceTester
    {

        private  FSDirectory _directory = FSDirectory.Open(@"C:\DASProjects\Development\location-service\GoatTrip.LucenceIndexer\Index\");
        private StandardAnalyzer _analyzer;
        private Lucene.Net.Search.IndexSearcher _searcher;

        public IndexPerformanceTester(string indexDirectoryPath)
        {
            _directory = FSDirectory.Open(indexDirectoryPath);
            _analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
           // _searcher = new GroupedIndexSearcher(_directory);
        }

        public void ExecSrch(string query, Action<string> callback, int noOfThreads)
        {
            var queryText = query.Trim() + "*";
            for (int i = 0; i < noOfThreads; i++)
            {
                var queryThread = new QueryThread(queryText, _searcher, i, _analyzer, callback);
               // var thread = new Thread(new ThreadStart(queryThread.Query_grouped));
              //  thread.Start();
                Thread.Sleep(2);
            }
        }
    }
}
