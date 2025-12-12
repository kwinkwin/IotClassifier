using IotClassifier.Application.DTOs;
using IotClassifier.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IotClassifier.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger,
                              IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<List<UserDto>>> GetAllEmployeesAsync()
        {
            try
            {
                var result = await _userService.GetAllEmployeesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message} \n InnerException: {ex.InnerException}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateEmployee")]
        public async Task<ActionResult<string>> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            try
            {
                var result = await _userService.CreateEmployeeAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message} \n InnerException: {ex.InnerException}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCurrentUserInfo")]
        public async Task<ActionResult<UserDto>> GetCurrentUserInfoAsync()
        {
            try
            {
                var result = await _userService.GetCurrentUserInfoAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message} \n InnerException: {ex.InnerException}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeactivateEmployee")]
        public async Task<ActionResult<string>> DeactivateEmployeeAsync(Guid idEmployee)
        {
            try
            {
                var result = await _userService.DeactivateEmployeeAsync(idEmployee);
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
