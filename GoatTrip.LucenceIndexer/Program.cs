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

    class Program
    {
        private static string _indexpath;
        private static string _sqliteDbFile;
        private static bool show_help = false;
        private static IndexPerformanceTester tester = new IndexPerformanceTester(@"C:\DASProjects\Development\location-service\GoatTrip.LucenceIndexer\Index\");
        private static ArgumentsParser _parser = new ArgumentsParser(new Dictionary<string, Action<string>>()
        {
            {"s", a => _sqliteDbFile = a},
            {"i", a => _indexpath = a}
        });
       

        static void Main(string[] args)
        {
            _parser.ParseArgs(args);
            LuceneIndexImport importer = new LuceneIndexImport(_indexpath, _sqliteDbFile);
            importer.ImportData(Console.WriteLine);

        }

        private static void ExecutePerformanceTest()
        {
            string query = Console.ReadLine();
            tester.ExecSrch(query, Console.WriteLine, 50);
            Program.ExecutePerformanceTest();
        }
      

    }


}
