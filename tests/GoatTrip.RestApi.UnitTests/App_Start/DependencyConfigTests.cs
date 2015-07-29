
namespace GoatTrip.RestApi.UnitTests
{
    using Autofac;
    using RestApi.Controllers;
    using Xunit;

    public class DependencyConfigTests {
        private readonly IContainer _container;

        public DependencyConfigTests() {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new LocationControllerModule(""));

            _container = builder.Build();
        }

        [Fact]
        public void CanResolveLocationControllerDependencies() {
            LocationController sut;
            Assert.True(_container.TryResolve(out sut));
        }
    }
}
