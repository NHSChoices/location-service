using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL.Lucene;
using Lucene.Net.Index;
using Moq;
using Xunit;
namespace GoatTrip.DAL.Lucene.Tests
{
    public class GroupCollectorTests
    {

        private Mock<IndexReader> _mockIndexReader;

        public GroupCollectorTests()
        {
            _mockIndexReader = new Mock<IndexReader>();
            //_mockIndexReader.Setup(m => m[int])
        }

        public void GroupCollectorTest()
        {
            Assert.True(false, "not implemented yet");
        }

        [Fact()]
        public void CollectTest()
        {
            Assert.True(false, "not implemented yet");
        }

    }
}
