using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Store;

namespace GoatTrip.DAL.Lucene
{
    public interface ILuceneDirectoryAdapter
    {
        Directory GeDirectory();
    }
}
