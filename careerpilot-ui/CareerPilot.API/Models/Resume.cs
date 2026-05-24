namespace CareerPilot.API.Models
{
    public class Resume
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ExtractedText { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public ICollection<ResumeAnalysis> Analyses { get; set; } 
            = new List<ResumeAnalysis>();
    }
}