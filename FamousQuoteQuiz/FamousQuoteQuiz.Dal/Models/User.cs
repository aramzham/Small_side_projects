using System;

namespace FamousQuoteQuiz.Dal.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public QuestionType QuestionType { get; set; }
    public DateTime CreatedAt { get; set; }
}