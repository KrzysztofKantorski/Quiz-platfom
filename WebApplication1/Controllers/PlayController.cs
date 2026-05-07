using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models.Dtos.play.Request;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize] 
    [Route("api/quizzes")]
    [ApiController]
    public class PlayController : ControllerBase
    {
        private readonly IPlayQuizService _playService;

        public PlayController(IPlayQuizService playService)
        {
            _playService = playService;
        }

        //Get all quizes (like for display to user)
        [HttpGet]

        //Allow all users to only browse courses
        [AllowAnonymous] 
        public async Task<IActionResult> GetQuizzes()
        {
            var result = await _playService.GetAvailableQuizzesAsync();
            return Ok(result.Quizzes);
        }

        //Start quiz -> Json response with quiz and answers to client
        [HttpGet("{id}/play")]
        public async Task<IActionResult> GetQuizForPlay(int id)
        {
            var result = await _playService.GetQuizForPlayAsync(id);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(result.Quiz);
        }



        //Submit quiz
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitQuiz(int id, [FromBody] SubmitQuizDto dto)
        {
            //Get user id from jwt token 
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized(new 
                {
                    message = "Unanable to identify user" 
                });
            }

            int userId = int.Parse(userIdClaim);

            var result = await _playService.SubmitQuizAsync(id, userId, dto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(result.Result);
        }
    }
}
