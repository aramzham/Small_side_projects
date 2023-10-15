namespace FamousQuoteQuiz.MVC.Models.RequestModels;

public class UpdateUserRequestModel
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public int? QuestionType { get; set; }
}