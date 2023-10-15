using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuoteQuiz.Dal.Interfaces;
using FamousQuoteQuiz.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Dal.Implementations;

public class AuthorDal : BaseDal, IAuthorDal
{
    public AuthorDal(QuizDbContext db) : base(db)
    {
    }

    public async Task<IEnumerable<Author>> GetAnswers(int correctAuthorId, int numberOfOtherAnswers)
    {
        var correctAnswer = await _db.Author.FirstAsync(x => x.Id == correctAuthorId);
        var otherAnswers = await _db.Author
            .Where(x => x.Id != correctAuthorId)
            .OrderBy(x => Guid.NewGuid())
            .Take(2)
            .ToListAsync();

        var randomIndex = _random.Next(0, otherAnswers.Count + 1);
        otherAnswers.Insert(randomIndex, correctAnswer);

        return otherAnswers;
    }
}