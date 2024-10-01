using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using TrainingLogParser.Logic.Command;

namespace TrainingLogParser.Tests.Infrastructure
{
    public class TestFixture
    {
        public Container Container { get; private set; }
        public ServiceProvider ServiceProvider { get; }

        public TestFixture()
        {
            var services = new ServiceCollection();
            services.AddMediatR(typeof(ParseTrainingLogCommand));

            ServiceProvider = services.BuildServiceProvider();

            Container = new Container();
            Container.Options.ResolveUnregisteredConcreteTypes = true;

            Container.Register(() => new ServiceFactory(Container.GetInstance), Lifestyle.Singleton);

            Container.RegisterSingleton<IMediator, Mediator>();

            Container.Verify();
        }
    }
}
