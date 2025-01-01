using MediatR;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Logic.Services.Interfaces;
using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Logic.Command
{
    public class SaveTrainingLogEntriesCommand : IRequest<Unit>
    {
        public IEnumerable<TrainingLogEntry> Entries { get; set; }
    }

    public class SaveTrainingLogEntriesHandler : IRequestHandler<SaveTrainingLogEntriesCommand, Unit>
    {
        private readonly ITrainingLogEntryRepo _trainingLogEntryRepo;
        private readonly ITrainingLogParserService _trainingLogParserService;

        public SaveTrainingLogEntriesHandler(ITrainingLogEntryRepo trainingLogEntryRepo,
            ITrainingLogParserService trainingLogParserService)
        {
            _trainingLogEntryRepo = trainingLogEntryRepo;
            _trainingLogParserService = trainingLogParserService;
        }

        public async Task<Unit> Handle(SaveTrainingLogEntriesCommand request, CancellationToken cancellationToken)
        {
            var sanitised = _trainingLogParserService.SanitiseInput(request.Entries);

            foreach (var entry in sanitised)
            {
                await _trainingLogEntryRepo.Create(entry);
            }

            return Unit.Value;
        }
    }
}
