using FamousQuoteQuiz.Dal;
using FamousQuoteQuiz.Dal.Models;
using FamousQuoteQuiz.Dal.Models.UpdateModels;
using FamousQuoteQuiz.MVC.Infrastructure;
using FamousQuoteQuiz.MVC.Infrastructure.ActionFilters;
using FamousQuoteQuiz.MVC.Models.RequestModels;
using FamousQuoteQuiz.MVC.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.MVC.Controllers;

public class UserController : Controller
{
    private readonly ISqlClient _sqlClient;

    public UserController(ISqlClient sqlClient)
    {
        _sqlClient = sqlClient;
    }

    [HttpGet]
    [Route("/settings")]
    public IActionResult Settings()
    {
        return View();
    }

    [HttpPost]
    [Route("/savesettings")]
    public async Task<IActionResult> SaveSettings(UserPreferenceRequestModel requestModel)
    {
        await _sqlClient.UserDal.Update(requestModel.UserId,
            new UserUpdateModel() { QuestionType = requestModel.QuestionType });

        return Ok();
    }

    [HttpGet]
    [Route("/users")]
    [ServiceFilter(typeof(CheckAdminActionFilter))]
    public async Task<IActionResult> ListOfUsers()
    {
        var users = await _sqlClient.UserDal.GetAll();

        return View(users);
    }

    [HttpPost]
    [Route("/user/add")]
    public async Task<IActionResult> Add(AddUserRequestModel requestModel)
    {
        var newUser = await _sqlClient.UserDal.Add(new User()
            { Name = requestModel.Name, QuestionType = (QuestionType)requestModel.QuestionType });

        return Ok(new UserResponseModel()
        {
            Id = newUser.Id,
            Name = newUser.Name,
            QuestionType = newUser.QuestionType.ToString(),
            CreatedAt = newUser.CreatedAt
        });
    }

    [HttpPut]
    [Route("/user/update")]
    public async Task<IActionResult> Update(UpdateUserRequestModel requestModel)
    {
        var updatedUser = await _sqlClient.UserDal.Update(requestModel.UserId,
            new UserUpdateModel()
                { Name = requestModel.Name, QuestionType = (QuestionType?)requestModel.QuestionType });

        return Ok(new UserResponseModel()
        {
            Id = updatedUser.Id,
            Name = updatedUser.Name,
            QuestionType = updatedUser.QuestionType.ToString(),
            CreatedAt = updatedUser.CreatedAt
        });
    }

    [HttpDelete]
    [Route("/user/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _sqlClient.UserDal.Delete(id);

        return Ok();
    }
}