using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using TaskManagementApp.Application.Models;

namespace TaskManagementApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserManagementService _userManagementService;

        public AuthenticationController(IAuthenticationService authenticationService, IUserManagementService userManagementService)
        {
            _authenticationService = authenticationService;
            _userManagementService = userManagementService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                var user = await _authenticationService.RegisterUserAsync(request.Username, request.Email, request.Password, request.AvatarUrl);

                if (user == null)
                {
                    return BadRequest("User registration failed.");
                }

                var token = _authenticationService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            try
            {
                var user = await _authenticationService.LoginUserAsync(request.Username, request.Password);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                var token = _authenticationService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManagementService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UpdateUserProfileRequest request)
        {
            try
            {
                await _userManagementService.UpdateUserProfileAsync(id, request.Username, request.Email, request.AvatarUrl);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
