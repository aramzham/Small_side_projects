using FamousQuoteQuiz.Dal.Interfaces;
using FamousQuoteQuiz.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Dal.Implementations;

public class UserAchievementDal : BaseDal, IUserAchievementDal
{
    public UserAchievementDal(QuizDbContext db) : base(db)
    {
    }

    public async Task Log(UserAchievement userAchievement)
    {
        await _db.UserAchievement.AddAsync(userAchievement);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserAchievementWithDataModel>> GetByUserId(int userId)
    {
        return await _db.UserAchievement
            .Where(x => x.UserId == userId)
            .Join(_db.Quote,
                ua => ua.QuoteId,
                q => q.Id,
                (ua, q) => new UserAchievementWithDataModel()
                {
                    Id = ua.Id,
                    Quote = q.Body,
                    IsAnsweredCorrectly = ua.IsAnsweredCorrectly
                })
            .ToListAsync();
    }
}