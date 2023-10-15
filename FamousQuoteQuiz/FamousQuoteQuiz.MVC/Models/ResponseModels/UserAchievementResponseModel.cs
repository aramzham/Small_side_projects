namespace FamousQuoteQuiz.MVC.Models.ResponseModels;

public class UserAchievementResponseModel
{
    public string UserName { get; set; }
    public IEnumerable<AchievementResponseModel> Achievements { get; set; } = new List<AchievementResponseModel>();
}

public class AchievementResponseModel
{
    public int Id { get; set; }
    public string Quote { get; set; }
    public bool IsAnsweredCorrectly { get; set; }
}