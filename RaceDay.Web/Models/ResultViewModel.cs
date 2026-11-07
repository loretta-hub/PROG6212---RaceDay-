namespace RaceDay.Web.Models
{
    public class ResultViewModel
    {
        public int ResultID { get; set; }

        public int EnrollmentID { get; set; }

        public TimeSpan FinishTime { get; set; }

        public int FinishingPosition { get; set; }

        public DateTime RecordedAt { get; set; }
    }
}
