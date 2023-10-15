using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuoteQuiz.Dal.Interfaces;
using FamousQuoteQuiz.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Dal.Implementations;

public class QuoteDal : BaseDal, IQuoteDal
{
    public QuoteDal(QuizDbContext db) : base(db)
    {
    }

    public async Task<Quote> GetRandomOne()
    {
        var count = await _db.Quote.CountAsync();
        var randomNumber = _random.Next(0, count);

        return await _db.Quote.Include(x => x.Author).Skip(randomNumber).FirstAsync();
    }

    public async Task<Quote> Create(string body, string authorName)
    {
        if (string.IsNullOrEmpty(body))
            throw new Exception("Please provide a quote body");

        if (string.IsNullOrEmpty(authorName))
            throw new Exception("Please provide an author name");
        
        var author = await GetOrCreateAuthor(authorName);

        var newQuote = new Quote() { Body = body, AuthorId = author.Id };
        await _db.Quote.AddAsync(newQuote);
        await _db.SaveChangesAsync();

        return newQuote;
    }

    private async Task<Author> GetOrCreateAuthor(string authorName)
    {
        var author = await _db.Author.FirstOrDefaultAsync(x => x.Name == authorName);
        if (author is not null) 
            return author;
        
        author = new Author() { Name = authorName };
        
        await _db.Author.AddAsync(author);
        await _db.SaveChangesAsync();

        return author;
    }

    public async Task<Quote> Update(int id, string body, string authorName)
    {
        var quote = await _db.Quote.FirstOrDefaultAsync(x => x.Id == id);
        if (quote is null)
            throw new Exception($"No quote exists with specified id: {id}");

        if (body != null) quote.Body = body;
        if (authorName != null) quote.AuthorId = (await GetOrCreateAuthor(authorName)).Id;

        await _db.SaveChangesAsync();

        return quote;
    }

    public async Task<IEnumerable<Quote>> GetAll()
    {
        return await _db.Quote.Include(x => x.Author).ToListAsync();
    }

    public async Task Delete(int id)
    {
        var quote = await _db.Quote.FirstOrDefaultAsync(x => x.Id == id);
        if (quote is null)
            throw new Exception($"No quote exists with specified id: {id}");

        _db.Quote.Remove(quote);

        await _db.SaveChangesAsync();
    }
}