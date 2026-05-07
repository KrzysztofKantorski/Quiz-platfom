using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Dtos.admin;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/quizzes")]
    [ApiController]
    public class AdminQuizController : ControllerBase
    {
        private readonly IAdminQuizService _adminQuizService;

        public AdminQuizController(IAdminQuizService adminQuizService)
        {
            _adminQuizService = adminQuizService;
        }

        //Create new quiz
        [HttpPost]
        public async Task<IActionResult> CreateQuizAsync(CreateQuizDto dto)
        {
            var result = await _adminQuizService.CreateQuizAsync(dto);
            return CreatedAtAction(nameof(GetQuizById), new { id = result.Quiz!.Id }, new
            {
                message = result.Message,
                quiz = result.Quiz
            });
        }

        //Get all quizes
        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var result = await _adminQuizService.GetAllQuizzesAsync();

            return Ok(result.Quizzes);
        }


        //Get quiz by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizById([FromRoute] int id)
        {
            var result = await _adminQuizService.GetQuizByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(result.Quiz);
        }

        //Update quiz by id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz([FromRoute] int id, [FromBody] CreateQuizDto dto)
        {
            var result = await _adminQuizService.UpdateQuizAsync(id, dto);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message, quiz = result.Quiz });
        }


        //Delete quiz by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz([FromRoute] int id)
        {
            var result = await _adminQuizService.DeleteQuizAsync(id);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
