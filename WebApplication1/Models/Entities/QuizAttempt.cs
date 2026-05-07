namespace WebApplication1.Models.Entities
{
    public class QuizAttempt
    {
        public int Id { get; set; }

        //User info
        public int UserId { get; set; }
        public User? User { get; set; }

        //Quiz info
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        //Time tracking
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime EndTime { get; set; } 

        public int Score { get; set; }
        public int MaxScore { get; set; }
    }
}
