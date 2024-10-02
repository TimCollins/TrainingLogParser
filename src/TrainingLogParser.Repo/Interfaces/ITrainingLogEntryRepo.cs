using TrainingLogParser.Domain.Model;

namespace TrainingLogParser.Repo.Interfaces
{
    public interface ITrainingLogEntryRepo : ISqliteRepo<TrainingLogEntry, int>
    {
        Task Create(TrainingLogEntry entry);
        Task DeleteEntriesForDate(DateTimeOffset date);
        Task<IEnumerable<TrainingLogEntry>> GetEntriesForDate(DateTimeOffset date);
    }
}
