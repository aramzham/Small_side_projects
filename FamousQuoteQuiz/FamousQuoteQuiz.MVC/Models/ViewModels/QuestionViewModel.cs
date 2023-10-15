using FamousQuoteQuiz.Dal.Models;

namespace FamousQuoteQuiz.MVC.Models.ViewModels;

public class QuestionViewModel
{
    public QuoteViewModel Quote { get; set; }
    public AuthorViewModel CorrectAnswer { get; set; }
    public IEnumerable<AuthorViewModel> Answers { get; set; }
    public int UserId { get; set; }
    public QuestionType QuestionType { get; set; }
}