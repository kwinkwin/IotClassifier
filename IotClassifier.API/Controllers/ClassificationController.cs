using IotClassifier.Application.DTOs;
using IotClassifier.Application.Interfaces;
using IotClassifier.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IotClassifier.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {
        private readonly ILogger<ClassificationController> _logger;
        private readonly IClassificationService _classificationService;

        public ClassificationController(ILogger<ClassificationController> logger,
                              IClassificationService classificationService)
        {
            _logger = logger;
            _classificationService = classificationService;
        }

        [HttpGet("GetTodayStatistics")]
        public async Task<ActionResult<List<DashboardStatDto>>> GetTodayStatisticsAsync()
        {
            try
            {
                var result = await _classificationService.GetTodayStatisticsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message} \n InnerException: {ex.InnerException}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetTodayStatistics")]
        public async Task<ActionResult<string>> AddClassificationLogAsync(CreateClassificationLogDto request)
        {
            try
            {
                var result = await _classificationService.AddClassificationLogAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message} \n InnerException: {ex.InnerException}");
                return BadRequest(ex.Message);
            }
        }
    }
}
