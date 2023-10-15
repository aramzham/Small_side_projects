using FamousQuoteQuiz.Dal.Models;

namespace FamousQuoteQuiz.MVC.Models.RequestModels;

public class AddUserRequestModel
{
    public string Name { get; set; }
    public int QuestionType { get; set; }
}