using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Dtos.admin;
using WebApplication1.Models.Dtos.auth;
using WebApplication1.Models.Entities;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class AdminQuestionService: IAdminQuestionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<CreateQuestionDto> _createQuestionValidator;
        private readonly IValidator<UpdateQuestionDto> _updateQuestionValidator;
        public AdminQuestionService(ApplicationDbContext dbContext, IValidator<CreateQuestionDto> createQuestionValidator, IValidator<UpdateQuestionDto> updateQuestionValidator)
        {
            _dbContext = dbContext;
            _createQuestionValidator = createQuestionValidator;
            _updateQuestionValidator = updateQuestionValidator;
        }


        //Add question to quiz
        public async Task<(bool Success, string Message, QuestionDto? Question)> AddQuestionToQuizAsync(int quizId, CreateQuestionDto dto) {

            //Validation
            await _createQuestionValidator.ValidateAndThrowAsync(dto);

            var questionExists = await _dbContext.Quizzes.AnyAsync(q => q.Id == quizId);
            if (!questionExists) {
                return (false, "Quiz does not exist ", null);
            }

            //Create new question with answer
            var newQuestion = new Question
            {
                Text = dto.Text,
                Points = dto.Points,
                QuizId = quizId,
                Answers = dto.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            //Save question
            await _dbContext.AddAsync(newQuestion);
            await _dbContext.SaveChangesAsync();

            //Map to dto
            var questionDto = MapToDto(newQuestion);

            return (true, "Question added successfully", questionDto);

        }


        //Get questions from quiz
        public async Task<(bool Success, string Message, List<QuestionDto> Questions)> GetQuestionsByQuizIdAsync(int quizId)
        {
            var questionExists = await _dbContext.Quizzes.AnyAsync(q => q.Id == quizId);
            if (!questionExists)
            {
                return (false, "Quiz does not exist ", []);
            }

            //Find questions and answers
            var questions = await _dbContext.Questions
                .Where(q => q.QuizId == quizId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Points = q.Points,
                    QuizId = q.QuizId,
                    Answers = q.Answers.Select(a => new AnswerDto
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                })
                .ToListAsync();

            return (true, "Question list was found", questions);
        }


        //Get question by id
        public async Task<(bool Success, string Message, QuestionDto? Question)> GetQuestionByIdAsync(int questionId)
        {
            var question = await _dbContext.Questions
                .Where(q => q.Id == questionId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Points = q.Points,
                    QuizId = q.QuizId,
                    Answers = q.Answers.Select(a => new AnswerDto
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if(question is null)
            {
                return (false, "Question does not exist.", null);
            }

            return (true, "Question was found", question);
        }


        //Update question
        public async Task<(bool Success, string Message, QuestionDto? Question)> UpdateQuestionByIdAsync(int questionId, UpdateQuestionDto dto)
        {
            //Validation
            await _updateQuestionValidator.ValidateAndThrowAsync(dto);

            //Find question with answer
            var question = await _dbContext.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q=> q.Id == questionId);

            if (question is null)
            {
                return (false, "Question does not exist.", null);
            }

            question.Text = dto.Text;
            question.Points = dto.Points;

            //Clean answears
            _dbContext.Answers.RemoveRange(question.Answers);

            //Create new answer
            question.Answers = dto.Answers.Select(a => new Answer
            {
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList();

            //Save to db
            await _dbContext.SaveChangesAsync();

            //Map to dto
            var questionDto = MapToDto(question);
            return (true, "Question was updated", questionDto);
        }


        //Delete question by id
        public async Task<(bool Success, string Message)> DeleteQuestionAsync(int questionId)
        {
            var question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
            {
                return (false, "Pytanie nie istnieje.");
            }

            //Remove questions and answers (cascade)
            _dbContext.Questions.Remove(question);
            await _dbContext.SaveChangesAsync();

            return (true, "Question was removed");
        }



        private QuestionDto MapToDto(Question q)
        {
            return new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                Points = q.Points,
                QuizId = q.QuizId,
                Answers = q.Answers.Select(a => new AnswerDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };
        }



    }
}
