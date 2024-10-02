using Dapper;
using Dapper.Contrib.Extensions;
using System.Data.SQLite;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Repo.Implementation
{
    public class TrainingLogEntryRepo : SqliteRepoBase<TrainingLogEntry, int>, ITrainingLogEntryRepo
    {
        // Local folder is <project>>\src\TrainingLogParser.Tests\bin\Debug\net8.0\
        private readonly string _connectionString = "Data Source=traininglog-int.db;Version=3;";

        public async Task Create(TrainingLogEntry entry)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                await connection.InsertAsync(entry);
            }
        }

        public async Task<IEnumerable<TrainingLogEntry>> GetEntriesForDate(DateTimeOffset inputDate)
        {
            // TODO: Fix this exception when including the date column
            //System.Data.DataException : Error parsing column 1(Date = 01/10/2024 00:00:00 + 01:00 - String)
            //---- System.InvalidCastException : Invalid cast from 'System.String' to 'System.DateTimeOffset'.
            var query = "SELECT Exercise, Weight, Reps FROM TrainingLogEntry " +
                "WHERE Date = @date";
            var parameters = new
            {
                // Needs to be in this format: '01/10/2024 00:00:00 +01:00'
                date = inputDate.ToString()
            };

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<TrainingLogEntry>(query, parameters);
            }
        }

        public async Task DeleteEntriesForDate(DateTimeOffset inputDate)
        {
            var query = "DELETE FROM TrainingLogEntry " +
                "WHERE Date = @date";
            var parameters = new
            {
                // Needs to be in this format: '01/10/2024 00:00:00 +01:00'
                date = inputDate.ToString()
            };

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
