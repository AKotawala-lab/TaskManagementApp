using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Application.Models;
using TaskManagementApp.Application.Exceptions;
using TaskManagementApp.Application.Services;

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

                var authResponse = _authenticationService.GenerateToken(user);
                return Ok(new { AccessToken = authResponse.AccessToken, RefreshToken = authResponse.RefreshToken, User = user });
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

                var authResponse = _authenticationService.GenerateToken(user);
                return Ok(new { AccessToken = authResponse.AccessToken, RefreshToken = authResponse.RefreshToken, User = user });
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

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            string errMessage = "Invalid access token or refresh token. Please Logout and Login again.";
            var principal = TokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return Conflict(new { message = errMessage });
            }

            var username = principal.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var user = await _userManagementService.GetUserByUsernameAsync(username);

            if (user == null)   //|| user.RefreshToken != request.RefreshToken
            {
                return Conflict(new { message = errMessage });
            }

            var newToken = _authenticationService.GenerateToken(user);

            return Ok(new { AccessToken = newToken.AccessToken, RefreshToken = newToken.RefreshToken, User = user });
        }     
    }
}
