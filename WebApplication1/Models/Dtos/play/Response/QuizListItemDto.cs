namespace WebApplication1.Models.Dtos.play.Request
{
    public class QuizListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
