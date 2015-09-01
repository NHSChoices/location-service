using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL.DTOs;
using Lucene.Net.Search;

namespace GoatTrip.DAL.Lucene
{
    public interface IGroupCollector
    {
        void Collect(int doc);
        List<LocationGroup> Groups { get; }
        bool AcceptsDocsOutOfOrder { get; }
        void SetNextReader(global::Lucene.Net.Index.IndexReader reader, int docBase);
        void SetScorer(Scorer scorer);
        bool GroupLimitReached { get; }

    }
}
