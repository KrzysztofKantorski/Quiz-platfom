using WebApplication1.Models.Dtos.admin;

namespace WebApplication1.Services.Interfaces
{
    public interface IAdminQuizService
    {
        //Create new Quiz
        Task<(bool Success, string Message, QuizDto? Quiz)> CreateQuizAsync(CreateQuizDto dto);

        //Get all quizes
        Task<(bool Success, string Message, List<QuizDto> Quizzes)> GetAllQuizzesAsync();

        //Get quiz by id
        Task<(bool Success, string Message, QuizDto? Quiz)> GetQuizByIdAsync(int id);

        //Update quiz by id
        Task<(bool Success, string Message, QuizDto? Quiz)> UpdateQuizAsync(int id, CreateQuizDto dto);

        //Delete quiz
        Task<(bool Success, string Message)> DeleteQuizAsync(int id);
    }
}
