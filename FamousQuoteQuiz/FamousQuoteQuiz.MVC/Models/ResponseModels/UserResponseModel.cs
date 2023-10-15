namespace FamousQuoteQuiz.MVC.Models.ResponseModels;

public class UserResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string QuestionType { get; set; }
    public DateTime CreatedAt { get; set; }
}