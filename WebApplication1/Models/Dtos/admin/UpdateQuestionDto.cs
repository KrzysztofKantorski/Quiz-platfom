namespace WebApplication1.Models.Dtos.admin
{
    public class UpdateQuestionDto
    {
        public string Text { get; set; } = string.Empty;
        public int Points { get; set; } = 1;
        public List<CreateAnswerDto> Answers { get; set; } = new List<CreateAnswerDto>();
    }
}
