using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/quizzes/history")]
    [ApiController]
    public class QuizHistoryContoller : ControllerBase
    {
        private readonly IQuizHistoryService _quizHistoryService;

        public QuizHistoryContoller(IQuizHistoryService quizHistoryService)
        {
            _quizHistoryService = quizHistoryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetUserHistory()
        {
            //Get user id from jwt
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //Verify id
            if (userIdClaim == null)
            {
                return Unauthorized(new
                {
                    message = "Unanable to identify user"
                });
            }

            int userId = int.Parse(userIdClaim);

            var result = await _quizHistoryService.GetUserQuizzesAsync(userId);

            if (!result.Success) {
                return BadRequest(new
                    {
                    message = result.Message,
                });
            }

            return Ok(result.Quizzes);
        }
    }
}
