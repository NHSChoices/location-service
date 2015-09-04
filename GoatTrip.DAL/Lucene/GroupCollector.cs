using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL.DTOs;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace GoatTrip.DAL.Lucene
{
    public class GroupCollector : Collector, IGroupCollector
    {
        private List<Document> _docs;
        private IndexReader _reader;
        private Scorer _scorer;
        private IEnumerable<LocationQueryField> _groupByFields;
        private ILocationGroupBuilder _groupBuilder;
        private List<LocationGroup> _groups;

        public GroupCollector(ILocationGroupBuilder groupBuilder, IEnumerable<LocationQueryField> groupByFields)
        {
            _groupBuilder = groupBuilder;
            _groupByFields = groupByFields;
            _docs = new List<Document>();
            _groups = new List<LocationGroup>();
        }

        public List<LocationGroup> Groups
        {
            get { return _groups; }
        }

        public override bool AcceptsDocsOutOfOrder
        {
           get { return true; }
        }

        public override void Collect(int doc)
        {
            _docs.Add(_reader[doc]);
            var group = _groups.FirstOrDefault(g => g.GroupDescription == _groupBuilder.GenerateGroupDescription(_reader[doc], _groupByFields));
            if (group == default(LocationGroup))
            {
                if (_groups.Count == 50)
                {
                    GroupLimitReached = true;
                    return;
                }
                _groups.Add(_groupBuilder.Build(_reader[doc], _groupByFields));

            }
            else
            {
                group.LocationsCount++;
                group.UPRN = 0;
            }
        }

        public override void SetNextReader(global::Lucene.Net.Index.IndexReader reader, int docBase)
        {
            _reader = reader;
        }

        public override void SetScorer(Scorer scorer)
        {
            _scorer = scorer;
        }

          public bool GroupLimitReached { get; private set; }

    }
}
