using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TrainingLogParser.Tests.Infrastructure;
using Xunit;
using TrainingLogParser.Logic.Command;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Logic.Query;
using System.Globalization;

namespace TrainingLogParser.Tests
{
    public class ParserTests : IClassFixture<TestFixture>
    {
        private readonly IMediator _mediator;

        public ParserTests(TestFixture fixture)
        {
            _mediator = fixture.ServiceProvider.GetService<IMediator>();
        }

        [Fact]
        public async Task GivenSimpleLinesOfData_RowsParsedSuccessfully()
        {
            var request = GetParseRequest("simple.csv");

            var res = await _mediator.Send(request);

            res.ShouldNotBeNull();

            res.Count.ShouldBe(2);
            var first = res.First();

            var dateOnly = new DateTime(2024, 9, 29);
            var expectedDate = new DateTimeOffset(dateOnly);

            first.Date.ShouldBe(expectedDate);
            first.Notes.ShouldBe("Top set");
            first.Reps.ShouldBe(5);
            first.Weight.ShouldBe(131);
        }

        [Fact]
        public async Task GivenMultipleDaysOfData_RowsParsedAndWrittenToDatabaseSuccessfully()
        {
            var entries = await GetMultipleDayData();

            entries.ShouldNotBeNull();
            entries.Count.ShouldBe(23);

            var saveCmd = new SaveTrainingLogEntriesCommand
            {
                Entries = entries
            };

            var res = await _mediator.Send(saveCmd);
            res.ShouldBe(Unit.Value);

            // Retrieve data for a given date
            var dateOnly = new DateTime(2024, 10, 1);
            var expectedDate = new DateTimeOffset(dateOnly);

            var query = new RetrieveTrainingLogEntriesByDateQuery
            {
                Date = expectedDate
            };
            var retrievedEntries = await _mediator.Send(query);

            // Assert the saved data based on the content of the original CSV
            retrievedEntries.Count().ShouldBe(12);

            // Delete data for given dates per CSV content
            var dates = new List<string>
            {
                "29/09/2024 00:00:00 +01:00",
                "01/10/2024 00:00:00 +01:00",
            };
            const string format = "dd/MM/yyyy HH:mm:ss zzz";

            foreach (var inputDate in dates)
            {
                var dateTimeOffset = DateTimeOffset.ParseExact(inputDate, format, CultureInfo.InvariantCulture);

                var command = new DeleteTrainingLogEntriesByDateCommand
                {
                    Date = dateTimeOffset
                };

                await _mediator.Send(command);

                query = new RetrieveTrainingLogEntriesByDateQuery
                {
                    Date = dateTimeOffset
                };

                retrievedEntries = await _mediator.Send(query);
                retrievedEntries.Count().ShouldBe(0);
            }
        }

        private async Task<List<TrainingLogEntry>> GetMultipleDayData()
        {
            var request = GetParseRequest("multiple-days.csv");

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
    }
}
