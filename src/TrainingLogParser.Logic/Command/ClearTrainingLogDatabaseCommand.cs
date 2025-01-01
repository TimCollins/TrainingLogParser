using MediatR;
using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Logic.Command
{
    public class ClearTrainingLogDatabaseCommand : IRequest<Unit>
    {
    }

    public class ClearTrainingLogDatabaseHandler : IRequestHandler<ClearTrainingLogDatabaseCommand, Unit>
    {
        private readonly ITrainingLogEntryRepo _trainingLogEntryRepo;

        public ClearTrainingLogDatabaseHandler(ITrainingLogEntryRepo trainingLogEntryRepo)
        {
            _trainingLogEntryRepo = trainingLogEntryRepo;
        }

        public async Task<Unit> Handle(ClearTrainingLogDatabaseCommand request, CancellationToken cancellationToken)
        {
            await _trainingLogEntryRepo.DeleteAllEntries();

            return Unit.Value;
        }
    }
}
