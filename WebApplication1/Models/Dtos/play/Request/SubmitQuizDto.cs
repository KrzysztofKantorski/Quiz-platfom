namespace WebApplication1.Models.Dtos.play.Request
{
    public class SubmitQuizDto
    {
        public List<AnswerSubmissionDto> SelectedAnswers { get; set; } = new List<AnswerSubmissionDto>();
    }
}
