using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Dtos.play.Request;
using WebApplication1.Models.Dtos.play.Response;
using WebApplication1.Services.Interfaces;
namespace WebApplication1.Services
{
    public class QuizHistoryService : IQuizHistoryService
    {
        private readonly ApplicationDbContext _dbContext;
        public QuizHistoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Get quiz history for user
        public async Task<(bool Success, string Message, List<QuizHistoryItemDto> Quizzes)> GetUserQuizzesAsync(int userId)
        {
            //Get user
            var user = await _dbContext.Users.AnyAsync(u => u.Id == userId);

            if(!user)
            {
                return (false, "User was not found", []);
            }


            //find user quizzes
            var historyDb = await _dbContext.QuizAttempts
            .Where(a => a.UserId == userId)
            .Include(a => a.Quiz)
            .OrderByDescending(a => a.EndTime)
            .Select(a => new
            {
                AttemptId = a.Id,
                QuizId = a.QuizId,
                QuizTitle = a.Quiz!.Title,
                QuizDescription = a.Quiz!.Description,
                Score = a.Score,
                MaxScore = a.MaxScore,
                StartTime = a.StartTime,
                EndTime = a.EndTime
            })
            .ToListAsync();

            var finalHistory = historyDb.Select(a => {
                TimeSpan duration = a.EndTime - a.StartTime;

                return new QuizHistoryItemDto
                {
                    AttemptId = a.AttemptId,
                    QuizId = a.QuizId,
                    QuizTitle = a.QuizTitle,
                    Score = a.Score,
                    MaxScore = a.MaxScore,
                    EndTime = a.EndTime,
                    //Calculate duration
                    TimeSpent = $"{(int)duration.TotalMinutes:D2}:{duration.Seconds:D2}"
                };
            }).ToList();

            return (true, "Quiz history.", finalHistory);
        }
    }
}
