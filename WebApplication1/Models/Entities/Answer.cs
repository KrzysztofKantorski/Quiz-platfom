namespace WebApplication1.Models.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public bool IsCorrect { get; set; } 

        //Foreign key
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
