using Microsoft.Extensions.DependencyInjection;
using TrainingLogParser.Logic.Command;
using TrainingLogParser.Repo.Implementation;
using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Tests.Infrastructure
{
    public class TestFixture
    {
        public ServiceProvider ServiceProvider;

        public TestFixture()
        {
            var services = new ServiceCollection();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ParseTrainingLogCommand).Assembly);
            });

            services.AddTransient<ITrainingLogEntryRepo, TrainingLogEntryRepo>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
