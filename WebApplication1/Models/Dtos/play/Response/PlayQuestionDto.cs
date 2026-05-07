namespace WebApplication1.Models.Dtos.play.Request
{
    public class PlayQuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Points { get; set; }
        public List<PlayAnswerDto> Answers { get; set; } = new List<PlayAnswerDto>();
    }
}
