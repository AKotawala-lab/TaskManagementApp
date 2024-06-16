using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Domain.Entities;
using TaskManagementApp.Application.Models;
using TaskManagementApp.Application.Exceptions;

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
                return Ok(new { Token = token, User = user });
            }
            catch (CustomException cex)
            {
                return Conflict(new { message = cex.Message });
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
                return Ok(new { Token = token, User = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getusername/{email}")]
        public async Task<IActionResult> GetUsernameByEmail(string email)
        {
            var user = await _userManagementService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Username);
        }          
    }
}
