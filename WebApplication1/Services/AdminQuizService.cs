using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Dtos.admin;
using WebApplication1.Models.Entities;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class AdminQuizService: IAdminQuizService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<CreateQuizDto> _quizValidator;
        public AdminQuizService(ApplicationDbContext dbContext, IValidator<CreateQuizDto> quizValidator)
        {
            _dbContext = dbContext;
            _quizValidator = quizValidator;
        }


        //Create new quiz
        public async Task<(bool Success, string Message, QuizDto? Quiz)> CreateQuizAsync(CreateQuizDto dto)
        {
            await _quizValidator.ValidateAndThrowAsync(dto);

            var newQuiz = new Quiz
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.AddAsync(newQuiz);
            await _dbContext.SaveChangesAsync();


            var quizDto = new QuizDto
            {
                Id = newQuiz.Id,
                Title = newQuiz.Title,
                Description = newQuiz.Description,
                CreatedAt = newQuiz.CreatedAt
            };

            return (true, "New quiz added successfully", quizDto);
        }

        //Get all quizess
        public async Task<(bool Success, string Message, List<QuizDto> Quizzes)> GetAllQuizzesAsync()
        {
            var quizzes = await _dbContext.Quizzes
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync();
            if(quizzes.Count == 0)
            {
                return (true, "No quizzes found", []);
            }
            return (true, "The following quizes were found", quizzes);
        }

        //Get quiz by id
        public async Task<(bool Success, string Message, QuizDto? Quiz)> GetQuizByIdAsync(int id)
        {
            //Search for quiz in database
            var quiz = await _dbContext.Quizzes
                .Where(q => q.Id == id)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    CreatedAt = q.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (quiz is null)
            {
                return (false, "Quiz was not found", null);
            }

            return (true, "Quiz was found", quiz);
        }

        //Update quiz
        public async Task<(bool Success, string Message, QuizDto? Quiz)> UpdateQuizAsync(int id, CreateQuizDto dto)
        {
            await _quizValidator.ValidateAndThrowAsync(dto);

            var quiz = await _dbContext.Quizzes.FirstOrDefaultAsync(q => q.Id == id);

            if (quiz is null) {
                return (false, "Quiz was not found", null);
            }

            //change properties
            quiz.Title = dto.Title;
            quiz.Description = dto.Description;

            await _dbContext.SaveChangesAsync();

            //Map to dto
            var quizDto = new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                CreatedAt = quiz.CreatedAt
            };

            return (true, "Quiz updated successfully", quizDto);
        }

        //Delete quiz by id
        public async Task<(bool Success, string Message)> DeleteQuizAsync(int id)
        {
            //Find quiz by id
            var quiz = await _dbContext.Quizzes.FirstOrDefaultAsync(q => q.Id == id);

            if (quiz is null)
            {
                return (false, "Quiz was not found");
            }

            //Delete quiz from database
            _dbContext.Quizzes.Remove(quiz);


            await _dbContext.SaveChangesAsync();

            return (true, "Quiz was deleted");
        }
    }
}
