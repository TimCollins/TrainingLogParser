using MediatR;
using Newtonsoft.Json;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Logic.Services.Interfaces;

namespace TrainingLogParser.Logic.Command
{
    public class SaveTrainingLogEntriesToJsonCommand : IRequest<string>
    {
        public IEnumerable<TrainingLogEntry> Entries { get; set; }
    }

    public class SaveTrainingLogEntriesToJsonHandler : IRequestHandler<SaveTrainingLogEntriesToJsonCommand, string>
    {
        private readonly ITrainingLogParserService _trainingLogParserService;

        public SaveTrainingLogEntriesToJsonHandler(ITrainingLogParserService trainingLogParserService)
        {
            _trainingLogParserService = trainingLogParserService;
        }

        public Task<string> Handle(SaveTrainingLogEntriesToJsonCommand request, CancellationToken cancellationToken)
        {
            var sanitised = _trainingLogParserService.SanitiseInput(request.Entries);

            var jsonString = JsonConvert.SerializeObject(sanitised);

            return Task.FromResult(jsonString);
        }
    }
}
