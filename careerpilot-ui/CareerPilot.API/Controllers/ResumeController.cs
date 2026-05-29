using CareerPilot.API.DTOs;
using CareerPilot.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerPilot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;

        public ResumeController(
            IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            IFormFile file)
        {
            var userId = int.Parse(User.FindFirst(
                ClaimTypes.NameIdentifier)!.Value);

            var resumeId = await _resumeService
                .UploadResume(file, userId);

            return Ok(new { resumeId });
        }

        [HttpPost("analyse")]
        public async Task<IActionResult> Analyse(
            ResumeAnalyseDto dto)
        {
            var userId = int.Parse(User.FindFirst(
                ClaimTypes.NameIdentifier)!.Value);

            var result = await _resumeService
                .AnalyseResume(dto, userId);

            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = int.Parse(User.FindFirst(
                ClaimTypes.NameIdentifier)!.Value);

            var history = await _resumeService
                .GetHistory(userId);

            return Ok(history);
        }
    }
}