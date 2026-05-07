namespace WebApplication1.Models.Dtos.admin
{
    public class QuizDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
