namespace FamousQuoteQuiz.MVC.Models.RequestModels;

public class LogRequestModel
{
    public int QuoteId { get; set; }
    public int UserId { get; set; }
    public bool IsAnsweredCorrectly { get; set; }
}