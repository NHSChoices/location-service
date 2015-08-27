using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Lucene.Net.Store;

namespace GoatTrip.DAL.Lucene
{
    public class LuceneIndexModule : Module
    {
        public LuceneIndexModule(string lucenceIndexDirectory)
        {
            _lucenceIndexDirectory = lucenceIndexDirectory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            FSDirectory directory = FSDirectory.Open(_lucenceIndexDirectory);
            builder.Register(c => new GroupedIndexSearcher(directory)).As<IGroupIndexSearcher>().SingleInstance();

            builder.Register(
                c => new LocationGroupRepository(c.Resolve<IGroupIndexSearcher>(), c.Resolve<ILocationGroupBuilder>())).As<ILocationGroupRepository>();
        }
        private readonly string _lucenceIndexDirectory;
    }
}
