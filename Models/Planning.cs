namespace ServerMCP.Models
{
    public class ForecastShiftByProjectRequest
    {
        public string ProjectId { get; set; } = null!;
        public string PlanType { get; set; } = null!;
        public int? Version { get; set; } = null!;

        public int SourceYear { get; set; }
        public int TargetYear { get; set; }

        public int SourcePeriod { get; set; }
        public int TargetPeriod { get; set; }

        public decimal Percentage { get; set; } = 0;

        public string PeriodType { get; set; } = "Monthly";
    }
}
