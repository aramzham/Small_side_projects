using FamousQuoteQuiz.Dal;
using FamousQuoteQuiz.Dal.Models;
using FamousQuoteQuiz.MVC.Models.RequestModels;
using FamousQuoteQuiz.MVC.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.MVC.Controllers;

public class UserAchievementsController : Controller
{
    private readonly ISqlClient _sqlClient;

    public UserAchievementsController(ISqlClient sqlClient)
    {
        _sqlClient = sqlClient;
    }

    [HttpPost]
    [Route("userachievements/add")]
    public Task AddToAchievements(LogRequestModel requestModel)
    {
        return _sqlClient.UserAchievementDal.Log(new UserAchievement()
        {
            QuoteId = requestModel.QuoteId,
            UserId = requestModel.UserId,
            IsAnsweredCorrectly = requestModel.IsAnsweredCorrectly
        });
    }

    [HttpGet]
    [Route("userachievements/{userId:int}")]
    public async Task<IActionResult> GetLogsByUserId(int userId)
    {
        var user = await _sqlClient.UserDal.GetById(userId);
        if (user is null)
            throw new Exception($"No user was found specified id: {userId}");

        var userAchievements = await _sqlClient.UserAchievementDal.GetByUserId(userId);

        return View(new UserAchievementResponseModel()
        {
            UserName = user.Name, 
            Achievements = userAchievements.Select(x => new AchievementResponseModel()
            {
                Id = x.Id,
                Quote = x.Quote,
                IsAnsweredCorrectly = x.IsAnsweredCorrectly
            })
        });
    }
}