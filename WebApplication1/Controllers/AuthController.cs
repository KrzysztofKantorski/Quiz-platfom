using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models.Dtos.auth;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if(!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _authService.GetAllUsersAsync();
            
            return Ok(new 
            { 
                message = result.Message,
                users = result.Users
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success) {
                return Unauthorized(new {
                    message= result.Message
                });
            }

           
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            //Save cookie
            Response.Cookies.Append("jwt", result.Token!, cookieOptions);

            return Ok(
                    new
                    {
                        message = result.Message
                    }
                );

        }


        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            //Get user id from token
            var userTokenId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(string.IsNullOrEmpty(userTokenId) || !int.TryParse(userTokenId, out int userId))
            {
                return Unauthorized(new
                {
                    message = "Incorrect token"
                });
            }

            var result = await _authService.GetUserByIdAsync(userId);

            if (!result.Success) {
                return NotFound(new
                {
                    message = "User was not found"
                });
            }

            return Ok(result.dto);
        }


        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            //Remove cookie
            Response.Cookies.Delete("jwt");
            return Ok(
                new
                {
                    message = "Logged out successfully"
                }
            );
        }
    }
}
