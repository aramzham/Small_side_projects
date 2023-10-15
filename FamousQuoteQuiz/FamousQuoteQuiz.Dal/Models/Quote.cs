namespace FamousQuoteQuiz.Dal.Models;

public class Quote
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }

    public virtual Author Author { get; set; }
}