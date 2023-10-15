using FamousQuoteQuiz.Dal.Interfaces;

namespace FamousQuoteQuiz.Dal;

public interface ISqlClient
{
    public IUserDal UserDal { get; }
    public IAuthorDal AuthorDal { get; }
    public IQuoteDal QuoteDal { get; }
    public IUserAchievementDal UserAchievementDal { get; }
}