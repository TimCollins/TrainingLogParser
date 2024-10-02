using MediatR;
using TrainingLogParser.Domain.Model;
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

        public SaveTrainingLogEntriesHandler(ITrainingLogEntryRepo trainingLogEntryRepo)
        {
            _trainingLogEntryRepo = trainingLogEntryRepo;
        }

        public async Task<Unit> Handle(SaveTrainingLogEntriesCommand request, CancellationToken cancellationToken)
        {
            var sanitised = SanitiseInput(request.Entries);

            foreach (var entry in sanitised)
            {
                await _trainingLogEntryRepo.Create(entry);
            }

            return Unit.Value;
        }

        private List<TrainingLogEntry> SanitiseInput(IEnumerable<TrainingLogEntry> entries)
        {
            var sanitised = new List<TrainingLogEntry>();

            DateTimeOffset? currentDate = null;

            foreach (var entry in entries)
            {
                // First time through if there is no current date then take it from the first row
                // which SHOULD have a date
                if (currentDate == null)
                {
                    currentDate = entry.Date;
                }

                // Set current date for entries which don't have a date 
                if (entry.Date == null)
                {
                    entry.Date = currentDate;
                }
                else if (currentDate != entry.Date)
                {
                    // When the date changes the new date should be set to currentDate
                    currentDate = entry.Date;
                }

                sanitised.Add(entry);
            }

            return sanitised;
        }
    }
}
