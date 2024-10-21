using CsvHelper.Configuration;
using System.Globalization;
using TrainingLogParser.Domain.Model;

namespace TrainingLogParser.Logic.ClassMaps
{
    internal class TrainingLogEntryClassMap : ClassMap<TrainingLogEntry>
    {
        public TrainingLogEntryClassMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Id).Ignore();
            Map(m => m.DateOffset).Ignore();
        }
    }
}
