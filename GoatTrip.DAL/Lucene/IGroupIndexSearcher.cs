using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Search;

namespace GoatTrip.DAL.Lucene
{
    public interface IGroupIndexSearcher
    {
        void Search(Weight weight, Filter filter, Collector collector);
        void Search(Query query, Collector results);
    }
}
