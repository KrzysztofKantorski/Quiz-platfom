namespace WebApplication1.Models.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public int Points { get; set; } = 1; 

        //Foreign key
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        //Relation
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
