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
        private readonly IUserManagementService _userManagementService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IUserManagementService userManagementService, ITokenService tokenService)
        {
            _userManagementService = userManagementService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var user = await _userManagementService.RegisterUserAsync(request.Username, request.Password);

            if (user == null)
            {
                return BadRequest("User registration failed.");
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var user = await _userManagementService.AuthenticateUserAsync(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
