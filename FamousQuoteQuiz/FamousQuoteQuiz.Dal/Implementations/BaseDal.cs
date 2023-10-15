using System;
using FamousQuoteQuiz.Dal.Interfaces;

namespace FamousQuoteQuiz.Dal.Implementations;

public class BaseDal : IBaseDal
{
    protected static Random _random = new();
    
    protected readonly QuizDbContext _db;

    public BaseDal(QuizDbContext db)
    {
        _db = db;
    }
}