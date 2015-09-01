using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoatTrip.DAL;
using GoatTrip.DAL.Lucene;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
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
        public QueryThread(string query, IndexSearcher searcher, int threadno, Analyzer analyzer)
        {
            _query = query;
            _searcher = searcher;
            _threadno = threadno;
            _analyzer = analyzer;
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
    

            var parser = new MultiFieldQueryParser(Version.LUCENE_29, new string[] { "StartNumber", "Street", "Town", "Postcode" }, _analyzer);
            parser.DefaultOperator = QueryParser.Operator.AND;
            Query query = parser.Parse(_query);
            

            var collector = TopScoreDocCollector.Create(100, true);

            TopDocs hits = _searcher.Search(query, null, 100);

            //iterate over the results.
            for (int i = 0; i < 100 && i< hits.ScoreDocs.Length; i++)
            {
                Document doc = _searcher.Doc(hits.ScoreDocs[i].Doc);
                string contentValue = doc.Get("StartNumber") + "," + doc.Get("Street") + "," + doc.Get("Town") + "," + doc.Get("Postcode");
                Console.WriteLine(contentValue);

            }
            t.Stop();
            Console.WriteLine("Thread no " + _threadno + " completed in " + t.ElapsedMilliseconds + "ms");
        }

        public void Query_grouped()
        {
            Stopwatch t = new Stopwatch();
            t.Start();

            var parser = new MultiFieldQueryParser(Version.LUCENE_29, new string[] { "StartNumber", "Street", "Town", "Postcode" }, _analyzer);
            parser.DefaultOperator = QueryParser.Operator.AND;
            Query query = parser.Parse(_query);

            _searcher.Search(query, _collector);

            //TopDocs hits = _searcher.Search(query, null, 100);
            for (int i = 0; i  < _collector.Groups.Count; i++)
            {
                //Document doc = _searcher.Doc(hits.ScoreDocs[i].Doc);
                var doc = _collector.Groups[i];
                string contentValue = doc.GroupDescription + " " + doc.LocationsCount;
                //ring contentValue = hits.HitsPerFacet[i].Name + hits.HitsPerFacet[i].HitCount.ToString();
                Console.WriteLine(contentValue);

            }
            t.Stop();
            Console.WriteLine("Thread no " + _threadno + " completed in " + t.ElapsedMilliseconds + "ms");
        }



    }


    class Program
    {

        private static FSDirectory directory = FSDirectory.Open(@"C:\DASProjects\Development\location-service\GoatTrip.LucenceIndexer\Index\");
        private static StandardAnalyzer analyzer = new StandardAnalyzer(Version.LUCENE_29);
        Lucene.Net.Search.IndexSearcher searcher = new Lucene.Net.Search.IndexSearcher(directory);
     
        static void Main(string[] args)
        {

            ExecApp();
           
        }

        private static void ExecApp()
        {
            var memdir = new RAMDirectory(directory);

            GroupedIndexSearcher searcher = new GroupedIndexSearcher(memdir);

            ExecSrch(searcher);
        }

        private static void ExecSrch(IndexSearcher searcher)
        {
          

            var queryText = Console.ReadLine().Trim() + "*";
            for (int i = 0; i < 1; i++)
            {

                var queryThread = new QueryThread(queryText, searcher, i, analyzer);
                var thread = new Thread(new ThreadStart(queryThread.Query_grouped));
                thread.Start();
                Thread.Sleep(2);
            }
            ExecSrch(searcher);
        }



        private static void ImportData()
        {
            ConnectionManager connMamager = new ConnectionManager(@"C:\DASProjects\Development\location-service\src\GoatTrip.RestApi\bin\App_Data\locationsimported.db", true);
            IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
            int importCount = 0;
            var sqlStatement = "SELECT LocationId,BUILDING_NAME,PAO_START_NUMBER,PAO_START_SUFFIX,STREET_DESCRIPTION,TOWN_NAME,ADMINISTRATIVE_AREA,POSTCODE_LOCATOR,POSTCODE,PAO_TEXT,SAO_TEXT FROM locations";
            using (var reader = connMamager.GetReader(sqlStatement, new StatementParamaters() { }))
            {
                while (reader.Read())
                {
                    var houseNumber = reader.DataReader["PAO_START_NUMBER"].ToString();
                    var suffix = reader.DataReader["PAO_START_SUFFIX"].ToString();
                    if (!String.IsNullOrWhiteSpace(suffix)) houseNumber += suffix.Trim();
                    Document doc = new Document();
                    doc.Add(new Field("id", reader.DataReader["LocationId"].ToString(), Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("BuildingName", reader.DataReader["BUILDING_NAME"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("StartNumber", houseNumber, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("StartSuffix", reader.DataReader["PAO_START_SUFFIX"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("Street", reader.DataReader["STREET_DESCRIPTION"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("Town", reader.DataReader["TOWN_NAME"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("AdminArea", reader.DataReader["ADMINISTRATIVE_AREA"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("PostcodeLocator", reader.DataReader["POSTCODE_LOCATOR"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("Postcode", reader.DataReader["POSTCODE"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("PrimaryText", reader.DataReader["PAO_TEXT"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("SecondaryText", reader.DataReader["SAO_TEXT"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    writer.AddDocument(doc);
                    importCount++;
                    if (importCount % 1000 == 0) Console.WriteLine(importCount.ToString() + " records added.");
                }
            }

            writer.Optimize();
            writer.Commit(); //Add This
            writer.Close();
            Console.WriteLine("Done!");
        }
    }


}
