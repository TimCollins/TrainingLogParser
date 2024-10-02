using Dapper.Contrib.Extensions;

namespace TrainingLogParser.Domain.Model
{
    [Table("TrainingLogEntry")]
    public class TrainingLogEntry
    {
        [Key]
        public int Id { get; set; }

        public DateTimeOffset? Date { get; set; }
        public string Exercise { get; set; }
        public decimal Weight { get; set; }
        public int Reps { get; set; }
        public string Notes { get; set; }
    }
}
