namespace WebApplication1.Models.Dtos.admin
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Points { get; set; } = 1;
        public int QuizId { get; set; } 

        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }
}
