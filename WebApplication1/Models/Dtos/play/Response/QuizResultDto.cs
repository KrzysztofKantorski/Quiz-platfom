namespace WebApplication1.Models.Dtos.play.Response
{
    public class QuizResultDto
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public int Score { get; set; } 
        public int MaxScore { get; set; } 
        public DateTime StartedAt { get; set; }
        public DateTime EndTime { get; set; }
    }
}
