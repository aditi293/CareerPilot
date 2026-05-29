namespace CareerPilot.API.DTOs
{
    public class ResumeUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }

    public class ResumeAnalyseDto
    {
        public int ResumeId { get; set; }
        public string JobDescription { get; set; } 
            = string.Empty;
    }

    public class ResumeAnalysisResultDto
    {
        public int Id { get; set; }
        public int OverallScore { get; set; }
        public List<string> Strengths { get; set; } 
            = new();
        public List<string> Weaknesses { get; set; } 
            = new();
        public List<string> MissingKeywords { get; set; } 
            = new();
        public List<string> Suggestions { get; set; } 
            = new();
        public int JDMatchScore { get; set; }
        public DateTime AnalysedAt { get; set; }
    }

    public class ResumeHistoryDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public int LatestScore { get; set; }
    }
}