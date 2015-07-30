
using GoatTrip.DAL;
using Moq;

namespace GoatTrip.RestApi.UnitTests
{
    using Autofac;
    using RestApi.Controllers;
    using Xunit;

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
