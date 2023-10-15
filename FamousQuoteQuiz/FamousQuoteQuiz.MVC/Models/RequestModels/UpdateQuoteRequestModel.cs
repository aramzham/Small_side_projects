namespace FamousQuoteQuiz.MVC.Models.RequestModels;

public class UpdateQuoteRequestModel
{
    public int QuoteId { get; set; }
    public string Body { get; set; }
    public string AuthorName { get; set; }
}