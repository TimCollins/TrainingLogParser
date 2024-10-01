using MediatR;
using TrainingLogParser.Domain.Model;

namespace TrainingLogParser.Logic.Command
{
    public class ParseTrainingLogCommand : IRequest<IEnumerable<TrainingLogEntry>>
    {
        public string Filename { get; set; }
    }

    public class ParseTrainingLogHandler : IRequestHandler<ParseTrainingLogCommand, IEnumerable<TrainingLogEntry>>
    {
        public Task<IEnumerable<TrainingLogEntry>> Handle(ParseTrainingLogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }


}
