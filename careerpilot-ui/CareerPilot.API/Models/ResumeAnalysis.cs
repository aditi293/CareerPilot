namespace CareerPilot.API.Models
{
    public class ResumeAnalysis
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string JobDescription { get; set; } = string.Empty;
        public int OverallScore { get; set; }
        public string Strengths { get; set; } = string.Empty;
        public string Weaknesses { get; set; } = string.Empty;
        public string MissingKeywords { get; set; } = string.Empty;
        public string Suggestions { get; set; } = string.Empty;
        public int JDMatchScore { get; set; }
        public DateTime AnalysedAt { get; set; } = DateTime.UtcNow;

        public Resume Resume { get; set; } = null!;
    }
}