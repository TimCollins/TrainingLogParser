using MediatR;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Logic.Query
{
    public class RetrieveTrainingLogEntriesByDateQuery : IRequest<IEnumerable<TrainingLogEntry>>
    {
        public DateTimeOffset Date { get; set; }
    }

    public class RetrieveTrainingLogEntriesByDateHandler : IRequestHandler<RetrieveTrainingLogEntriesByDateQuery, IEnumerable<TrainingLogEntry>>
    {
        private readonly ITrainingLogEntryRepo _trainingLogEntryRepo;

        public RetrieveTrainingLogEntriesByDateHandler(ITrainingLogEntryRepo trainingLogEntryRepo)
        {
            _trainingLogEntryRepo = trainingLogEntryRepo;
        }

        public async Task<IEnumerable<TrainingLogEntry>> Handle(RetrieveTrainingLogEntriesByDateQuery request, CancellationToken cancellationToken)
        {
            var entries = await _trainingLogEntryRepo.GetEntriesForDate(request.Date);

            return entries;
        }
    }
}
