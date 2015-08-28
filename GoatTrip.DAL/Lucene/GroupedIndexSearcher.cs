using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace GoatTrip.DAL.Lucene
{
    public class GroupedIndexSearcher : IndexSearcher, IGroupIndexSearcher
    {
        private IndexReader[] subReaders;
        private int[] docStarts;


        public GroupedIndexSearcher(Directory path)
            : this(path, true) {}

        public GroupedIndexSearcher(Directory path, bool readOnly)
            : base(path, readOnly)
        {
            subReaders = new[] { base.IndexReader };
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
                        this.SearchWithScorer(this.subReaders[index], weight, scorer, collector);
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
                if (num != DocIdSetIterator.NO_MORE_DOCS && !((GroupCollector)collector).GroupLimitReached)
                {
                    collector.Collect(num);
                    target = docIdSetIterator.NextDoc();
                    num = scorer.Advance(target);
                }
                else
                    break;
            }
        }

        private void SearchWithScorer(IndexReader reader, Weight weight, Scorer scorer, Collector collector)
        {
            if (scorer == null)
                return;
            scorer.DocID();

            int num = scorer.NextDoc(); ;
            collector.SetScorer(scorer);
            while (true)
            {
               
                if (num != DocIdSetIterator.NO_MORE_DOCS && !((GroupCollector)collector).GroupLimitReached)
                {
                    collector.Collect(num);
                    num = scorer.NextDoc();
                }
                else
                    break;
            }
        }
    }
}
