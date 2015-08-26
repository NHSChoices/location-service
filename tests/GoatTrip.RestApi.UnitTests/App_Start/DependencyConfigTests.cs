
using Autofac;
using GoatTrip.DAL;
using GoatTrip.DAL.Lucene;
using GoatTrip.RestApi.Controllers;
using Lucene.Net.Store;
using Moq;
using Xunit;

namespace GoatTrip.RestApi.UnitTests
{
    public class DependencyConfigTests {
        private readonly IContainer _container;
        private readonly Mock<ILuceneDirectoryAdapter> _mockDirectoryAdaptor; 
        public DependencyConfigTests() {
            var builder = new ContainerBuilder();
            _mockDirectoryAdaptor = new Mock<ILuceneDirectoryAdapter>();
            builder.Register(c => new Mock<ILocationRepository>().Object).As<ILocationRepository>(); ;
            builder.Register(c => new Mock<ILocationGroupRepository>().Object).As<ILocationGroupRepository>(); ;
            builder.RegisterModule(new LocationControllerModule());


            _container = builder.Build();
        }

        [Fact]
        public void CanResolveLocationControllerDependencies() {
            LocationController sut;
            Assert.True(_container.TryResolve(out sut));
        }
    }
}
