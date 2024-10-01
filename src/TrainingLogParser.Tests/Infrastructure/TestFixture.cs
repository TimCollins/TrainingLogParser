using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using TrainingLogParser.Logic.Command;

namespace TrainingLogParser.Tests.Infrastructure
{
    public class TestFixture
    {
        public Container Container { get; private set; }
        public ServiceProvider ServiceProvider;

        public TestFixture()
        {
            var services = new ServiceCollection();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ParseTrainingLogCommand).Assembly);
            });

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
