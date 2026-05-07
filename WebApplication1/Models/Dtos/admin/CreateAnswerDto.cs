namespace WebApplication1.Models.Dtos.admin
{
    public class CreateAnswerDto
    {
        public required string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
