using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using System.Globalization;
using TrainingLogParser.Domain.Model;
using TrainingLogParser.Logic.Converters;

namespace TrainingLogParser.Logic.Command
{
    public class ParseTrainingLogCommand : IRequest<List<TrainingLogEntry>>
    {
        public string Filename { get; set; }
    }

    public class ParseTrainingLogHandler : IRequestHandler<ParseTrainingLogCommand, List<TrainingLogEntry>>
    {
        public Task<List<TrainingLogEntry>> Handle(ParseTrainingLogCommand request, CancellationToken cancellationToken)
        {
            // TODO: Add check for missing/null filename

            var options = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                ShouldSkipRecord = args => args.Row.Parser.Record.All(string.IsNullOrWhiteSpace)
            };

            using (var reader = new StreamReader(request.Filename))
            {
                using (var csv = new CsvReader(reader, options))
                {
                    csv.Context.TypeConverterCache.AddConverter<DateTimeOffset>(new CustomDateTimeOffsetConverter());

                    var records = csv.GetRecords<TrainingLogEntry>().ToList();

                    return Task.FromResult(records);
                }
            }
        }
    }
}
