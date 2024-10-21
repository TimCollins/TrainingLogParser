using MediatR;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Logic.Query
{
    public class GetHeaviestBenchPressQuery : IRequest<TrainingLogEntry>
    {
    }

    public class GetHeaviestBenchPressHandler : IRequestHandler<GetHeaviestBenchPressQuery, TrainingLogEntry>
    {
        private readonly ITrainingLogEntryRepo _trainingLogEntryRepo;

        public GetHeaviestBenchPressHandler(ITrainingLogEntryRepo trainingLogEntryRepo)
        {
            _trainingLogEntryRepo = trainingLogEntryRepo;
        }

        public async Task<TrainingLogEntry> Handle(GetHeaviestBenchPressQuery request, CancellationToken cancellationToken)
        {
            var res = await _trainingLogEntryRepo.GetHeaviestSetForExercise("Bench Press");

            return res;
        }
    }
}
