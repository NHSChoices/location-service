
using Autofac;
using GoatTrip.DAL;
using GoatTrip.RestApi.Controllers;
using Moq;
using Xunit;

namespace GoatTrip.RestApi.UnitTests
{
    public class DependencyConfigTests {
        private readonly IContainer _container;

        public DependencyConfigTests() {
            var builder = new ContainerBuilder();

            builder.Register(c => new Mock<ILocationRepository>().Object).As<ILocationRepository>(); ;
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
