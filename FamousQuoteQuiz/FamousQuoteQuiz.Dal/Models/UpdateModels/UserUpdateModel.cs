namespace FamousQuoteQuiz.Dal.Models.UpdateModels;

public class UserUpdateModel
{
    public string Name { get; set; }
    public QuestionType? QuestionType { get; set; }
}