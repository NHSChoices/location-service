using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace GoatTrip.DAL.Lucene
{
    public class AddressQueryParser : MultiFieldQueryParser
    {
        public AddressQueryParser(): base(Version.LUCENE_29, new string[2], new KeywordAnalyzer() )
        {
            
        }

        protected override Query GetFieldQuery(string field, string queryText)
        {
            return base.GetFieldQuery(field, queryText);
        }
    }
}
