using System;
using System.Threading.Tasks;
using FamousQuoteQuiz.Dal.Implementations;
using FamousQuoteQuiz.Dal.Interfaces;

namespace FamousQuoteQuiz.Dal;

public class SqlClient : ISqlClient, IAsyncDisposable
{
    private readonly string _connectionString;

    private QuizDbContext _db;

    protected QuizDbContext DB => _db ??= new QuizDbContext(_connectionString);

    public SqlClient()
    {
        
    }

    public SqlClient(QuizDbContext db)
    {
        _db = db;
    }

    public SqlClient(string connectionString)
    {
        _connectionString = connectionString;
    }

    #region Dals

    private IUserDal _userDal;
    public IUserDal UserDal => _userDal ??= new UserDal(DB);
    
    private IAuthorDal _authorDal;
    public IAuthorDal AuthorDal => _authorDal ??= new AuthorDal(DB);
    
    private IQuoteDal _quoteDal;
    public IQuoteDal QuoteDal => _quoteDal ??= new QuoteDal(DB);
    
    private IUserAchievementDal _userAchievementDal;
    public IUserAchievementDal UserAchievementDal => _userAchievementDal ??= new UserAchievementDal(DB);

    #endregion
    
    public ValueTask DisposeAsync()
    {
        return _db?.DisposeAsync() ?? ValueTask.CompletedTask;
    }
}