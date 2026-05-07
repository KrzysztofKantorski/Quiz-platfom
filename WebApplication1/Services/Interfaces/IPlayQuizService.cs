using WebApplication1.Models.Dtos.play.Request;
using WebApplication1.Models.Dtos.play.Response;

namespace WebApplication1.Services.Interfaces
{
    public interface IPlayQuizService
    {
        //Get all quizes
        Task<(bool Success, string Message, List<QuizListItemDto> Quizzes)> GetAvailableQuizzesAsync();

        //Get quiz
        Task<(bool Success, string Message, PlayQuizDto? Quiz)> GetQuizForPlayAsync(int quizId);

        //Calculate user score 
        Task<(bool Success, string Message, QuizResultDto? Result)> SubmitQuizAsync(int quizId, int userId, SubmitQuizDto dto);
    }
}
