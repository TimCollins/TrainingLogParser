using Dapper;
using Dapper.Contrib.Extensions;
using System.Data.SQLite;
using System.Text;
using TrainingLogParser.Domain;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Repo.Implementation
{
    public class TrainingLogEntryRepo : SqliteRepoBase<TrainingLogEntry, int>, ITrainingLogEntryRepo
    {
        // Local folder is <project>\src\TrainingLogParser.Tests\bin\Debug\net8.0\
        private readonly string _connectionString = "Data Source=traininglog-int.db;Version=3;";

        public async Task Create(TrainingLogEntry entry)
        {
            // Trying to add an ISO date string. Probably need to just set Date as astring in the Model
            // and have a DTO with a DateTimeOffset.
            var obj = new
            {
                //Date = entry.Date.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffK"),
                Exercise = entry.Exercise,
                Weight = entry.Weight,
                Reps = entry.Reps,
                Notes = entry.Notes
            };

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                await connection.InsertAsync(entry);
            }
        }

        public async Task<IEnumerable<TrainingLogEntry>> GetEntriesForDate(DateTimeOffset inputDate)
        {
            var query = "SELECT Exercise, Weight, Reps FROM TrainingLogEntry " +
                "WHERE Date = @date";

            var parameters = new
            {
                date = inputDate.ToSqliteDateFormat()
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
                date = inputDate.ToSqliteDateFormat()
            };

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<TrainingLogEntry> GetHeaviestSetForExercise(string exercise)
        {
            var query = "SELECT * FROM TrainingLogEntry " +
                "WHERE Exercise = @exercise " +
                "ORDER BY Weight DESC " +
                "LIMIT 1";

            var parameters = new
            {
                exercise
            };

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var res = await connection.QuerySingleAsync<TrainingLogEntry>(query, parameters);

                return res;
            }
        }

        public async Task<IEnumerable<TrainingLogEntry>> GetBarbellExercisePRSummaryQuery()
        {
            var queryPart = "SELECT * FROM ( " +
                "SELECT * FROM TrainingLogEntry " +
                "WHERE Exercise = '{0}' " +
                "ORDER BY Weight DESC " +
                "LIMIT 1 )";

            var exercises = new List<string>
            {
                TrainingLogParserConstants.Exercises.BackSquat,
                TrainingLogParserConstants.Exercises.BenchPress,
                TrainingLogParserConstants.Exercises.Deadlift,
                TrainingLogParserConstants.Exercises.OverheadPress
            };

            var query = new StringBuilder();

            for (var i = 0; i < exercises.Count; i++)
            {
                var exercise = exercises[i];

                query.Append(string.Format(queryPart, exercise));

                var isNotLast = i < exercises.Count - 1;

                if (isNotLast)
                {
                    query.Append(" UNION ALL ");
                }
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var res = await connection.QueryAsync<TrainingLogEntry>(query.ToString());

                return res;
            }
        }

        public async Task DeleteAllEntries()
        {
            var query = "DELETE FROM TrainingLogEntry";

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(query);
            }
        }
    }
}
