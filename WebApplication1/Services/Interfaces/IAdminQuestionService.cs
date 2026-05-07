using WebApplication1.Models.Dtos.admin;

namespace WebApplication1.Services.Interfaces
{
    public interface IAdminQuestionService
    {
        //Add question to quiz
        Task<(bool Success, string Message, QuestionDto? Question)> AddQuestionToQuizAsync(int quizId, CreateQuestionDto dto);

        //Get quiz questions
        Task<(bool Success, string Message, List<QuestionDto> Questions)> GetQuestionsByQuizIdAsync(int quizId);

        //Get question by id
        Task<(bool Success, string Message, QuestionDto? Question)> GetQuestionByIdAsync(int questionId);

        //Update question by id
        Task<(bool Success, string Message, QuestionDto? Question)> UpdateQuestionByIdAsync(int questionId, UpdateQuestionDto dto);

        //Delete question by id
        Task<(bool Success, string Message)> DeleteQuestionAsync(int questionId);
    }
}
