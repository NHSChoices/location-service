using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Store;


namespace GoatTrip.DAL.Lucene
{
    public class LuceneDirectoryAdapter : ILuceneDirectoryAdapter
    {

        public LuceneDirectoryAdapter(string directoryPath)
        {
            _directoryPath = directoryPath;

        }
        public Directory GeDirectory()
        {
            return FSDirectory.Open(_directoryPath);
        }

        private readonly string _directoryPath;
    }
}
