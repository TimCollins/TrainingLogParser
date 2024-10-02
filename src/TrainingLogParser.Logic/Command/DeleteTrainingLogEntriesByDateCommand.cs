using MediatR;
using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Logic.Command
{
    public class DeleteTrainingLogEntriesByDateCommand : IRequest<Unit>
    {
        public DateTimeOffset Date { get; set; }
    }

    public class DeleteTrainingLogEntriesByDateHandler : IRequestHandler<DeleteTrainingLogEntriesByDateCommand, Unit>
    {
        private readonly ITrainingLogEntryRepo _trainingLogEntryRepo;

        public DeleteTrainingLogEntriesByDateHandler(ITrainingLogEntryRepo trainingLogEntryRepo)
        {
            _trainingLogEntryRepo = trainingLogEntryRepo;
        }

        public async Task<Unit> Handle(DeleteTrainingLogEntriesByDateCommand request, CancellationToken cancellationToken)
        {
            await _trainingLogEntryRepo.DeleteEntriesForDate(request.Date);

            return Unit.Value;
        }
    }
}
