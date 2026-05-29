using CareerPilot.API.Data;
using CareerPilot.API.DTOs;
using CareerPilot.API.Models;
using Microsoft.EntityFrameworkCore;
using Mscc.GenerativeAI;
using System.Text.Json;
using UglyToad.PdfPig;

namespace CareerPilot.API.Services
{
    public class ResumeService : IResumeService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public ResumeService(
            AppDbContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Step 1: Upload and extract text from PDF
        public async Task<int> UploadResume(
            IFormFile file, int userId)
        {
            // Extract text from PDF
            var extractedText = ExtractTextFromPdf(file);

            // Save to database
            var resume = new Resume
            {
                UserId = userId,
                FileName = file.FileName,
                ExtractedText = extractedText
            };

            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            return resume.Id;
        }

        // Step 2: Analyse resume with AI
        public async Task<ResumeAnalysisResultDto> 
            AnalyseResume(ResumeAnalyseDto dto, int userId)
        {
            // Get resume from DB
            var resume = await _context.Resumes
                .FirstOrDefaultAsync(r => 
                    r.Id == dto.ResumeId && 
                    r.UserId == userId);

            if (resume == null)
                throw new Exception("Resume not found");

            // Call Gemini AI
            var aiResult = await CallGeminiAI(
                resume.ExtractedText, 
                dto.JobDescription);

            // Save analysis to DB
            var analysis = new ResumeAnalysis
            {
                ResumeId = resume.Id,
                JobDescription = dto.JobDescription,
                OverallScore = aiResult.OverallScore,
                Strengths = JsonSerializer
                    .Serialize(aiResult.Strengths),
                Weaknesses = JsonSerializer
                    .Serialize(aiResult.Weaknesses),
                MissingKeywords = JsonSerializer
                    .Serialize(aiResult.MissingKeywords),
                Suggestions = JsonSerializer
                    .Serialize(aiResult.Suggestions),
                JDMatchScore = aiResult.JDMatchScore
            };

            _context.ResumeAnalyses.Add(analysis);
            await _context.SaveChangesAsync();

            aiResult.Id = analysis.Id;
            aiResult.AnalysedAt = analysis.AnalysedAt;

            return aiResult;
        }

        // Step 3: Get history
        public async Task<List<ResumeHistoryDto>> 
            GetHistory(int userId)
        {
            var resumes = await _context.Resumes
                .Where(r => r.UserId == userId)
                .Include(r => r.Analyses)
                .OrderByDescending(r => r.UploadedAt)
                .ToListAsync();

            return resumes.Select(r => new ResumeHistoryDto
            {
                Id = r.Id,
                FileName = r.FileName,
                UploadedAt = r.UploadedAt,
                LatestScore = r.Analyses
                    .OrderByDescending(a => a.AnalysedAt)
                    .FirstOrDefault()?.OverallScore ?? 0
            }).ToList();
        }

        // Extract text from PDF using PdfPig
        private string ExtractTextFromPdf(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var pdf = PdfDocument.Open(stream);

            var text = string.Join(" ", 
                pdf.GetPages()
                   .Select(p => p.Text));

            return text;
        }

        // Call Gemini AI
        private async Task<ResumeAnalysisResultDto> 
            CallGeminiAI(
            string resumeText, 
            string jobDescription)
        {
            var apiKey = _config["GoogleAI:ApiKey"];
            var googleAI = new GoogleAI(apiKey);
            var model = googleAI.GenerativeModel(
                        "gemini-2.5-flash");

            var prompt = $@"
Analyse this resume and return ONLY a JSON object.
No extra text, no markdown, just pure JSON.

Resume:
{resumeText}

Job Description:
{jobDescription}

Return exactly this JSON structure:
{{
  ""overallScore"": <number 0-100>,
  ""strengths"": [""strength1"", ""strength2""],
  ""weaknesses"": [""weakness1"", ""weakness2""],
  ""missingKeywords"": [""keyword1"", ""keyword2""],
  ""suggestions"": [""suggestion1"", ""suggestion2""],
  ""jdMatchScore"": <number 0-100>
}}";

            var response = await model
                .GenerateContent(prompt);
            var jsonText = response.Text ?? "{}";

            // Clean response if needed
            jsonText = jsonText
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            var result = JsonSerializer
                .Deserialize<ResumeAnalysisResultDto>(
                jsonText, 
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new ResumeAnalysisResultDto();

            return result;
        }
    }
}