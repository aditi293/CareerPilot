using CareerPilot.API.DTOs;

namespace CareerPilot.API.Services
{
    public interface IResumeService
    {
        Task<int> UploadResume(
            IFormFile file, int userId);
        Task<ResumeAnalysisResultDto> AnalyseResume(
            ResumeAnalyseDto dto, int userId);
        Task<List<ResumeHistoryDto>> GetHistory(
            int userId);
    }
}