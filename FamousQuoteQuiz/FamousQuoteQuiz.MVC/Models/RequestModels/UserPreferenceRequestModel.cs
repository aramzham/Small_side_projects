using FamousQuoteQuiz.Dal.Models;

namespace FamousQuoteQuiz.MVC.Models.RequestModels;

public class UserPreferenceRequestModel
{
    public int UserId { get; set; }
    public QuestionType QuestionType { get; set; }
}