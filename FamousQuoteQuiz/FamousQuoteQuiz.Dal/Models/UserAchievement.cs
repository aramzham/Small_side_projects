namespace FamousQuoteQuiz.Dal.Models;

public class UserAchievement
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuoteId { get; set; }
    public bool IsAnsweredCorrectly { get; set; }
}