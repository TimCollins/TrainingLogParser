using TrainingLogParser.Domain.Model;

namespace TrainingLogParser.Logic.Services.Interfaces
{
    public interface ITrainingLogParserService
    {
        List<TrainingLogEntry> SanitiseInput(IEnumerable<TrainingLogEntry> entries);
    }
}
