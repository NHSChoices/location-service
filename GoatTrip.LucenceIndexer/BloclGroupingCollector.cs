using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Support;

namespace GoatTrip.LucenceIndexer
{

   

    public class GroupedSearcher : IndexSearcher
    {
        private IndexReader[] subReaders;
        private int[] docStarts;

        public GroupedSearcher(FSDirectory path) : base(path)
        {
            subReaders = new[] {base.IndexReader};
            docStarts = new int[this.subReaders.Length];
        }



        public override void Search(Weight weight, Filter filter, Collector collector)
        {

            if (filter == null)
            {
                for (int index = 0; index < this.subReaders.Length; ++index)
                {
                    collector.SetNextReader(this.subReaders[index], this.docStarts[index]);
                    Scorer scorer = weight.Scorer(this.subReaders[index], !collector.AcceptsDocsOutOfOrder, true);
                    if (scorer != null)
                       // scorer.Score(collector);
                     this.SearchWithFilter(this.subReaders[index], weight, scorer, collector);
                }
            }
            else
            {
                for (int index = 0; index < this.subReaders.Length; ++index)
                {
                    collector.SetNextReader(this.subReaders[index], this.docStarts[index]);
                    this.SearchWithFilter(this.subReaders[index], weight, filter, collector);
                }
            }
        }

        private void SearchWithFilter(IndexReader reader, Weight weight, Scorer scorer, Collector collector)
        {
            if (scorer == null)
                return;
            scorer.DocID();
            DocIdSetIterator docIdSetIterator = scorer;
            if (docIdSetIterator == null)
                return;
            int target = docIdSetIterator.NextDoc();
            int num = target;
          //  int num = scorer.Advance(target);
            collector.SetScorer(scorer);
            while (true)
            {
                //while (num != target)
                //{
                //    if (num > target)
                //        target = docIdSetIterator.Advance(num);
                //    else
                //        num = scorer.Advance(target);
                //}
                if (num != DocIdSetIterator.NO_MORE_DOCS && !((BloclGroupingCollector) collector).GroupLimitReached)
                {
                    collector.Collect(num);
                    num = docIdSetIterator.NextDoc();
                    //target = docIdSetIterator.NextDoc();
                    //num = scorer.Advance(target);
                }
                else
                    break;
            }
        }

        private void SearchWithFilter(IndexReader reader, Weight weight, Filter filter, Collector collector)
        {
            DocIdSet docIdSet = filter.GetDocIdSet(reader);
            if (docIdSet == null)
                return;
            Scorer scorer = weight.Scorer(reader, true, false);
            if (scorer == null)
                return;
            scorer.DocID();
            
            DocIdSetIterator docIdSetIterator = docIdSet.Iterator();
            if (docIdSetIterator == null)
                return;
            int target = docIdSetIterator.NextDoc();
            int num = scorer.Advance(target);
            collector.SetScorer(scorer);
            while (true)
            {
                while (num != target)
                {
                    if (num > target)
                        target = docIdSetIterator.Advance(num);
                    else
                        num = scorer.Advance(target);
                }
                if (num != DocIdSetIterator.NO_MORE_DOCS && !((BloclGroupingCollector) collector).GroupLimitReached)
                {
                    collector.Collect(num);
                    target = docIdSetIterator.NextDoc();
                    num = scorer.Advance(target);
                }
                else
                    break;
            }
        }
    }

    public class GroupedDoc
    {
        private string _key;
        private int _count;
      
        public GroupedDoc(string key)
        {
            _key = key;
            _count = 1;
        }

    

        public string Name { get { return _key; } }
        public int Number { get { return _count; } }
        public void AddCount()
        {
            _count ++;
        }
    }
    public  class BloclGroupingCollector : Collector
    {
        public BloclGroupingCollector()
        {
            _docs = new List<Document>();
            _groups = new List<GroupedDoc>();
            _deadLoopCount = 0;
        }
        public bool GroupLimitReached { get; private set; }
        private List<Document> _docs;
        private IndexReader _reader;
        private Scorer _scorer;
        private int _deadLoopCount;
        private List<GroupedDoc> _groups; 

        public override bool AcceptsDocsOutOfOrder
        {
            get { return true; }
        }

        public override void Collect(int doc)
        {
            _docs.Add(_reader[doc]);
            var groupName = _reader[doc].Get("Street") + "," +
                            _reader[doc].Get("Town") + "," + _reader[doc].Get("Postcode");

            var group = _groups.FirstOrDefault(g => g.Name == groupName);
            if (group == default(GroupedDoc))

            {
                if (_groups.Count == 100)
                {
                    GroupLimitReached = true;
                    return;
                }
                _groups.Add(new GroupedDoc(groupName));
               
            }
            else
                group.AddCount();
        }

        public override
            void SetNextReader(Lucene.Net.Index.IndexReader reader, int docBase)
        {
            _reader = reader;
        }

        public override void SetScorer(Scorer scorer)
        {
            _scorer = scorer;
        }

        public List<GroupedDoc> Docs { get { return _groups; } } 
    }
}
