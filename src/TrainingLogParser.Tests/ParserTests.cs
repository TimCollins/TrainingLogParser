using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Globalization;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Logic.Command;
using TrainingLogParser.Tests.Infrastructure;
using Xunit;

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
        public void CanReadCsvFile()
        {
            var csvFile = Path.Combine(AppContext.BaseDirectory, "TestData", "simple.csv");

            File.Exists(csvFile).ShouldBeTrue();

            List<TrainingLogEntry> records;

            using (var reader = new StreamReader(csvFile))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var options = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        PrepareHeaderForMatch = args => args.Header.ToLower(),
                    };

                    csv.Context.TypeConverterCache.AddConverter<DateTimeOffset>(new CustomDateTimeOffsetConverter());

                    records = csv.GetRecords<TrainingLogEntry>().ToList();
                }
            }

            records.Count.ShouldBe(1);
            var first = records.First();

            var dateOnly = new DateTime(2024, 9, 29);
            var expectedDate = new DateTimeOffset(dateOnly);
            
            first.Date.ShouldBe(expectedDate);
            first.Notes.ShouldBe("Top set");
            first.Reps.ShouldBe(5);
            first.Weight.ShouldBe(131);
        }

        [Fact]
        public async Task CanCallCommand()
        {   
            var request = new ParseTrainingLogCommand
            {
                Filename = Path.Combine(AppContext.BaseDirectory, "TestData", "simple.csv")
            };

            var res = await _mediator.Send(request);

            res.ShouldNotBeNull();
        }
    }

    public class CustomDateTimeOffsetConverter : DefaultTypeConverter
    {
        private const string DateFormat = "dd/MM/yyyy";

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (DateTimeOffset.TryParseExact(text, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}
