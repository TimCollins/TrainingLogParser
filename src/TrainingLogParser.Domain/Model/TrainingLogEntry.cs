namespace TrainingLogParser.Domain.Model
{
    public class TrainingLogEntry
    {
        public DateTimeOffset? Date { get; set; }
        public string Exercise { get; set; }
        public decimal Weight { get; set; }
        public int Reps { get; set; }
        public string Notes { get; set; }
    }
}
