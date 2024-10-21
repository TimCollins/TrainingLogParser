using Dapper.Contrib.Extensions;

namespace TrainingLogParser.Domain.Model
{
    [Table("TrainingLogEntry")]
    public class TrainingLogEntry
    {
        [Key]
        public int Id { get; set; }

        public string Date { get; set; }
        public string Exercise { get; set; }
        public decimal? Weight { get; set; }
        public int Reps { get; set; }
        public string Notes { get; set; }

        [Computed]
        public DateTimeOffset DateOffset
        {
            get => DateTimeOffset.Parse(Date);
            set => Date = value.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
        }
    }
}
