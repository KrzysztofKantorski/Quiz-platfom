using WebApplication1.Models.Dtos.play.Request;
using WebApplication1.Models.Dtos.play.Response;

namespace WebApplication1.Services.Interfaces
{
    public interface IQuizHistoryService
    {
        //Get quiz history of current user
        Task<(bool Success, string Message, List<QuizHistoryItemDto> Quizzes)> GetUserQuizzesAsync(int userId);
    }
}
