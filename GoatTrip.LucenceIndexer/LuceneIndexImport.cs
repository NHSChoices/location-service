using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace GoatTrip.LucenceIndexer
{
    public class LuceneIndexImport
    {
        private SQLiteConnection _connection;

        private FSDirectory _indexDirectory;
        private StandardAnalyzer _analyzer;
        public LuceneIndexImport(string indexDirectory, string sqlLiteDbPath)
        {
            _connection = new SQLiteConnection("data source=" + sqlLiteDbPath + "; Version=3; Pooling=True; Max Pool Size=100;");
            _indexDirectory = FSDirectory.Open(indexDirectory);
            _analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
        }

        public void ImportData(Action<string> callback)
        {

            IndexWriter writer = new IndexWriter(_indexDirectory, _analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
            int importCount = 0;
            var sqlStatement = "SELECT UPRN,BUILDING_NAME,PAO_START_NUMBER,PAO_START_SUFFIX,STREET_DESCRIPTION,TOWN_NAME,ADMINISTRATIVE_AREA,POSTCODE_LOCATOR,POSTCODE,PAO_TEXT,SAO_TEXT FROM locations";
            SQLiteCommand command = new SQLiteCommand(sqlStatement, _connection);
            command.ExecuteReader();
            using (var reader =  command.ExecuteReader())
            {
                while (reader.Read())
                {
                    AddDocument(writer, reader);
                    importCount++;
                    if (importCount % 1000 == 0 && callback != null) callback(importCount.ToString() + " records added.");
                }
            }

            writer.Optimize();
            writer.Commit(); //Add This
            writer.Close();
           if(callback != null)  callback("Done!");
        }

        private void AddDocument(IndexWriter writer, SQLiteDataReader reader)
        {
            var houseNumber = reader["PAO_START_NUMBER"].ToString();
            var suffix = reader["PAO_START_SUFFIX"].ToString();
            if (!String.IsNullOrWhiteSpace(suffix)) houseNumber += suffix.Trim();
            Document doc = new Document();
            doc.Add(new Field("id", reader["UPRN"].ToString(), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("BuildingName", reader["BUILDING_NAME"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("StartNumber", houseNumber, Field.Store.YES, Field.Index.ANALYZED));
            //doc.Add(new Field("StartSuffix", reader.DataReader["PAO_START_SUFFIX"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Street", reader["STREET_DESCRIPTION"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Town", reader["TOWN_NAME"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("AdminArea", reader["ADMINISTRATIVE_AREA"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("PostcodeLocator", reader["POSTCODE_LOCATOR"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Postcode", reader["POSTCODE"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("PrimaryText", reader["PAO_TEXT"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("SecondaryText", reader["SAO_TEXT"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(doc);
        }
    }
}
