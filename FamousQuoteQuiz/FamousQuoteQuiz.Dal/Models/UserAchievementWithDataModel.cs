namespace FamousQuoteQuiz.Dal.Models;

public class UserAchievementWithDataModel
{
    public int Id { get; set; }
    public string Quote { get; set; }
    public bool IsAnsweredCorrectly { get; set; }
}