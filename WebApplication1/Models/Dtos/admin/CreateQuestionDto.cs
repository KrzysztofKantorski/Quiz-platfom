namespace WebApplication1.Models.Dtos.admin
{
    public class CreateQuestionDto
    {
        public string Text { get; set; } = string.Empty;
        public int Points { get; set; }
        public List<CreateAnswerDto> Answers { get; set; } = new List<CreateAnswerDto>();
    }
}
