using System.Text.Json;

namespace FamousQuoteQuiz.MVC.Models.ResponseModels;

public class ErrorDetailsResponseModel
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}