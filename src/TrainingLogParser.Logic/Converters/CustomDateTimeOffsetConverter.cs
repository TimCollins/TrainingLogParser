using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;

namespace TrainingLogParser.Logic.Converters
{
    internal class CustomDateTimeOffsetConverter : DefaultTypeConverter
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
