using TrainingLogParser.Domain.Model;
using TrainingLogParser.Domain;
using TrainingLogParser.Logic.Services.Interfaces;

namespace TrainingLogParser.Logic.Services.Impl
{
    public class TrainingLogParserService : ITrainingLogParserService
    {
        public List<TrainingLogEntry> SanitiseInput(IEnumerable<TrainingLogEntry> entries)
        {
            var sanitised = new List<TrainingLogEntry>();

            DateTimeOffset? currentDate = null;

            foreach (var entry in entries)
            {
                // First time through if there is no current date then take it from the first row
                // which SHOULD have a date
                if (currentDate == null)
                {
                    currentDate = DateTimeOffset.Parse(entry.Date);
                    entry.Date = DateTimeOffset.Parse(entry.Date).ToString(TrainingLogParserConstants.IsoDateFormat);
                }

                // Set current date for entries which don't have a date 
                if (string.IsNullOrEmpty(entry.Date))
                {
                    entry.Date = currentDate.Value.ToString(TrainingLogParserConstants.IsoDateFormat);
                }
                else if (currentDate != DateTimeOffset.Parse(entry.Date))
                {
                    // When the date changes the new date should be set to currentDate
                    currentDate = DateTimeOffset.Parse(entry.Date);
                    entry.Date = DateTimeOffset.Parse(entry.Date).ToString(TrainingLogParserConstants.IsoDateFormat);
                }

                sanitised.Add(entry);
            }

            return sanitised;
        }
    }
}
