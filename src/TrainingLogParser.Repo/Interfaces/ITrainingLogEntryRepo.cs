using TrainingLogParser.Domain.Model;

namespace TrainingLogParser.Repo.Interfaces
{
    public interface ITrainingLogEntryRepo : ISqliteRepo<TrainingLogEntry, int>
    {
        Task Create(TrainingLogEntry entry);
        Task DeleteAllEntries();
        Task DeleteEntriesForDate(DateTimeOffset date);
        Task<IEnumerable<TrainingLogEntry>> GetBarbellExercisePRSummaryQuery();
        Task<IEnumerable<TrainingLogEntry>> GetEntriesForDate(DateTimeOffset date);
        Task<TrainingLogEntry> GetHeaviestSetForExercise(string v);
    }
}
