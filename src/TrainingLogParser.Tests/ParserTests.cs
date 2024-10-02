using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TrainingLogParser.Tests.Infrastructure;
using Xunit;
using TrainingLogParser.Logic.Command;

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
        public async Task GivenMultipleDaysOfData_RowsParsedSuccessfully()
        {
            var request = GetParseRequest("multiple-days.csv");

            var res = await _mediator.Send(request);

            res.ShouldNotBeNull();
            res.Count.ShouldBe(23);
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
