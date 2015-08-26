using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;

namespace GoatTrip.DAL.Lucene
{
    public interface ILocationIndexManager
    {
        IGroupIndexSearcher IndexSearcher { get; }
        Query GenerateLuceneQuery(string query, ILocationGroupingStrategy groupingStrategy);
    }
}
