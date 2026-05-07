namespace WebApplication1.Models.Dtos.play.Request
{
    public class PlayQuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<PlayQuestionDto> Questions { get; set; } = new List<PlayQuestionDto>();
    }
}
