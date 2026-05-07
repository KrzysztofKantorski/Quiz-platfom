using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Dtos.admin;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin")]
    [ApiController]
    public class AdminQuestionController : ControllerBase
    {
        private readonly IAdminQuestionService _questionService;

        public AdminQuestionController(IAdminQuestionService questionService)
        {
            _questionService = questionService;
        }

        //Add question to quiz
        [HttpPost("quizzes/{quizId}/questions")]
        public async Task<IActionResult> AddQuestionToQuiz([FromRoute] int quizId, [FromBody] CreateQuestionDto dto)
        {
            var result = await _questionService.AddQuestionToQuizAsync(quizId, dto);

            if (!result.Success) {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return CreatedAtAction(nameof(GetQuestionById), new { id = result.Question!.Id }, new
            {
                message = result.Message,
                question = result.Question
            });
        }

        //Get question by id
        [HttpGet("questions/{id}")]
        public async Task<IActionResult> GetQuestionById([FromRoute] int id)
        {
            var result = await _questionService.GetQuestionByIdAsync(id);

            if (!result.Success) return NotFound(new { message = result.Message });

            return Ok(result.Question);
        }

        //Get questions from quiz
        [HttpGet("quizzes/{quizId}/questions")]
        public async Task<IActionResult> GetQuestionsByQuizId([FromRoute] int quizId)
        {
            var result = await _questionService.GetQuestionsByQuizIdAsync(quizId);

            if (!result.Success) {
                return NotFound(new { 
                    message = result.Message 
                });
            } 

            return Ok(result.Questions);
        }

        //Update question by id
        [HttpPut("questions/{id}")]
        public async Task<IActionResult> UpdateQuestion([FromRoute] int id, [FromBody] UpdateQuestionDto dto)
        {
            var result = await _questionService.UpdateQuestionByIdAsync(id, dto);

            if (!result.Success) {
                return NotFound(new { 
                    message = result.Message 
                });
            }
           
            return Ok(new { message = result.Message, question = result.Question });
        }

        //Delete question by id
        [HttpDelete("questions/{id}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int id)
        {
            var result = await _questionService.DeleteQuestionAsync(id);

            if (!result.Success) {
                return NotFound(new { 
                    message = result.Message 
                });
            }
           
            return Ok(new { message = result.Message });
        }
    }
}
