using System.Collections.Generic;
using System.Threading.Tasks;
using FamousQuoteQuiz.Dal.Models;

namespace FamousQuoteQuiz.Dal.Interfaces;

public interface IQuoteDal : IBaseDal
{
    Task<Quote> GetRandomOne();
    Task<Quote> Update(int id, string body, string authorName);
    Task<Quote> Create(string body, string authorName);
    Task<IEnumerable<Quote>> GetAll();
    Task Delete(int id);
}