using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Dtos.play.Request;
using WebApplication1.Models.Dtos.play.Response;
using WebApplication1.Models.Entities;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class PlayQuizService: IPlayQuizService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<SubmitQuizDto> _submitQuizDtoValidator;
        public PlayQuizService(ApplicationDbContext dbContext, 
            IValidator<SubmitQuizDto> submitQuizDtoValidator)
        {
            _dbContext = dbContext;
            _submitQuizDtoValidator = submitQuizDtoValidator;
        }

        //Get all quizes
        public async Task<(bool Success, string Message, List<QuizListItemDto> Quizzes)> GetAvailableQuizzesAsync()
        {
            var quizzes = await _dbContext.Quizzes
                .Select(q => new QuizListItemDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description
                })
                .ToListAsync();

            return (true, "Quiz list.", quizzes);
        }

        //Get quiz from database, prepare for play
        public async Task<(bool Success, string Message, PlayQuizDto? Quiz)> GetQuizForPlayAsync(int quizId)
        {
            //Get quiz data (questions, answers)
            var quiz = await _dbContext.Quizzes
                .Where(q => q.Id == quizId)
                .Select(q => new PlayQuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Questions = q.Questions.Select(question => new PlayQuestionDto
                    {
                        Id = question.Id,
                        Text = question.Text,
                        Points = question.Points,
                        Answers = question.Answers.Select(answer => new PlayAnswerDto
                        {
                            Id = answer.Id,
                            Text = answer.Text
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return (false, "Quiz does not exist.", null);
            }

            return (true, "Quiz is ready to start.", quiz);
        }

        //Submit quiz
        public async Task<(bool Success, string Message, QuizResultDto? Result)> SubmitQuizAsync(int quizId, int userId, SubmitQuizDto dto)
        {
            //Validation of dots
            await _submitQuizDtoValidator.ValidateAndThrowAsync(dto);

            //Get quiz from db
            var dbQuiz = await _dbContext.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (dbQuiz == null)
            {
                return (false, "Quiz does not exist.", null);
            }


            //Check if user submitted all answers
            if (dto.SelectedAnswers.Count != dbQuiz.Questions.Count)
            {
                return (false, $"Quiz is not completed.", null);
            }


            int score = 0;
            int maxScore = 0;

            //Calculate score
            foreach (var question in dbQuiz.Questions)
            {
                maxScore += question.Points;

                //Find player answers
                var playerAnswer = dto.SelectedAnswers.FirstOrDefault(a => a.QuestionId == question.Id);


                if (playerAnswer == null)
                {
                    //Player did not submit all answers
                    return (false, $"No answer was provided for some questions.", null);
                }

                //Check if user provided answer that belongs to question
                var validAnswer = question.Answers.FirstOrDefault(a => a.Id == playerAnswer.SelectedAnswerId);

                if (validAnswer == null)
                {
                    return (false, $"Answer {playerAnswer.SelectedAnswerId} does not belong to question {question.Id}!", null);
                }
                

                if (validAnswer.IsCorrect)
                {
                    score += question.Points;
                }



            }

            //Save attempt to db
            var attempt = new QuizAttempt
            {
                QuizId = quizId,
                UserId = userId,
                Score = score,
                MaxScore = maxScore,
                EndTime = DateTime.UtcNow
            };

            await _dbContext.QuizAttempts.AddAsync(attempt);
            await _dbContext.SaveChangesAsync();

            //Return info to player
            var resultDto = new QuizResultDto
            {
                QuizId = quizId,
                QuizTitle = dbQuiz.Title,
                Score = score,
                MaxScore = maxScore,
                EndTime = attempt.EndTime
            };

            return (true, "Quiz finshed successfully.", resultDto);
        }

    }
}
