namespace WebApplication1.Models.Dtos.play.Response
{
    public class QuizHistoryItemDto
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public DateTime EndTime { get; set; }
        public string TimeSpent { get; set; } = string.Empty;

    }
}
