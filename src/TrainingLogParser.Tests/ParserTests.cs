using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Globalization;
using TrainingLogParser.Domain;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Logic.Command;
using TrainingLogParser.Logic.Query;
using TrainingLogParser.Tests.Infrastructure;
using Xunit;

namespace TrainingLogParser.Tests
{
    public class ParserTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly IMediator _mediator;

        public ParserTests(TestFixture fixture)
        {
            _mediator = fixture.ServiceProvider.GetService<IMediator>();
        }

        [Fact]
        public async Task GivenSimpleLinesOfData_RowsParsedSuccessfully()
        {
            const string fileName = "simple.csv";

            var entries = await GetEntriesForFile(fileName);

            entries.ShouldNotBeNull();

            entries.Count.ShouldBe(2);
            var first = entries.First();

            var dateOnly = new DateTime(2024, 9, 29);
            var expectedDate = new DateTimeOffset(dateOnly);

            //first.Date.ShouldBe(expectedDate);
            first.Notes.ShouldBe("Top set");
            first.Reps.ShouldBe(5);
            first.Weight.ShouldBe(131);
        }

        [Fact]
        public async Task GivenMultipleDaysOfData_RowsParsedAndWrittenToDatabaseSuccessfully()
        {
            const string fileName = "multiple-days.csv";

            var res = await SaveToDatabase(fileName);
            res.ShouldBe(Unit.Value);

            var dateOnly = new DateTime(2024, 10, 1);
            var expectedDate = new DateTimeOffset(dateOnly);

            var query = new RetrieveTrainingLogEntriesByDateQuery
            {
                Date = expectedDate
            };
            var retrievedEntries = await _mediator.Send(query);

            // Assert the saved data based on the content of the original CSV
            retrievedEntries.Count().ShouldBe(12);
        }

        [Fact]
        public async Task GivenExerciseWithNoWeight_RowsParsedSuccessfully()
        {
            const string fileName = "no-weight.csv";

            var entries = await GetEntriesForFile(fileName);

            entries.ShouldNotBeNull();
        }

        //[Fact(Skip = "Takes 40 seconds to run")]
        [Fact]
        public async Task GivenFullCsvOfData_RowsParsedAndWrittenToDatabaseSuccessfully()
        {
            const string fileName = "full.csv";

            var entries = await GetEntriesForFile(fileName);

            entries.ShouldNotBeNull();

            var saveCmd = new SaveTrainingLogEntriesCommand
            {
                Entries = entries
            };

            await _mediator.Send(saveCmd);
        }

        [Fact]
        public async Task GivenDataInDatabase_CanGetHeaviestBenchPressSet()
        {
            const string fileName = "multiple-days.csv";

            await SaveToDatabase(fileName);

            var query = new GetHeaviestBenchPressQuery();

            var res = await _mediator.Send(query);

            // Check against the CSV for the correct data
            res.Weight.ShouldBe(105);
            res.Reps.ShouldBe(6);
        }
        
        #region Helpers
        private async Task<List<TrainingLogEntry>> GetEntriesForFile(string fileName)
        {
            var request = GetParseRequest(fileName);

            var res = await _mediator.Send(request);

            return res;
        }

        private ParseTrainingLogCommand GetParseRequest(string filename)
        {
            return new ParseTrainingLogCommand
            {
                Filename = Path.Combine(AppContext.BaseDirectory, "TestData", filename)
            };
        }

        private async Task<Unit> SaveToDatabase(string fileName)
        {
            var entries = await GetEntriesForFile(fileName);

            var saveCmd = new SaveTrainingLogEntriesCommand
            {
                Entries = entries
            };

            var res = await _mediator.Send(saveCmd);

            return res;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            var clearDatabaseCommand = new ClearTrainingLogDatabaseCommand();
            Task.Run(async () => await _mediator.Send(clearDatabaseCommand)).Wait();
        }
        #endregion
    }
}
