using MediatR;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Logic.Query
{
    public class GetBarbellExercisePRSummaryQuery : IRequest<IEnumerable<TrainingLogEntry>>
    {
    }

    public class GetBarbellExercisePRSummaryHandler : IRequestHandler<GetBarbellExercisePRSummaryQuery, IEnumerable<TrainingLogEntry>>
    {
        private readonly ITrainingLogEntryRepo _trainingLogEntryRepo;

        public GetBarbellExercisePRSummaryHandler(ITrainingLogEntryRepo trainingLogEntryRepo)
        {
            _trainingLogEntryRepo = trainingLogEntryRepo;
        }

        public async Task<IEnumerable<TrainingLogEntry>> Handle(GetBarbellExercisePRSummaryQuery request, CancellationToken cancellationToken)
        {
            var res = await _trainingLogEntryRepo.GetBarbellExercisePRSummaryQuery();

            return res;
        }
    }
}
