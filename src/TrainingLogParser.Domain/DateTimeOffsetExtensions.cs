namespace TrainingLogParser.Domain
{
    public static class DateTimeOffsetExtensions
    {
        public static string ToSqliteDateFormat(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(TrainingLogParserConstants.IsoDateFormat);
        }
    }
}
