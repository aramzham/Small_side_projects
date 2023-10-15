using FamousQuoteQuiz.Dal;
using FamousQuoteQuiz.Dal.Models;
using FamousQuoteQuiz.MVC.Infrastructure;
using FamousQuoteQuiz.MVC.Infrastructure.ActionFilters;
using FamousQuoteQuiz.MVC.Models.RequestModels;
using FamousQuoteQuiz.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.MVC.Controllers;

public class QuoteController : Controller
{
    private readonly ISqlClient _sqlClient;

    public QuoteController(ISqlClient sqlClient)
    {
        _sqlClient = sqlClient;
    }

    [HttpGet]
    [Route("/quote")]
    [ServiceFilter(typeof(CheckUserLoggedInActionFilter))]
    public async Task<IActionResult> Quote()
    {
        var quote = await _sqlClient.QuoteDal.GetRandomOne();
        var authors = await _sqlClient.AuthorDal.GetAnswers(quote.AuthorId, 2);

        // question type
        var userPreference = HttpContext.Request.Cookies.TryGetValue("userPreference", out var up)
            ? up
            : "binary";
        var questionType = Enum.TryParse<QuestionType>(userPreference, true, out var result)
            ? result
            : QuestionType.Binary;

        // user id
        var userId = HttpContext.Request.Cookies.TryGetValue("userId", out var ui) ? int.Parse(ui) : 1;

        return View(new QuestionViewModel()
        {
            Quote = new QuoteViewModel()
            {
                Id = quote.Id,
                Body = quote.Body
            },
            Answers = authors.Select(x => new AuthorViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }),
            CorrectAnswer = new AuthorViewModel()
            {
                Id = quote.Author.Id,
                Name = quote.Author.Name
            },
            UserId = userId,
            QuestionType = questionType
        });
    }

    [HttpGet]
    [Route("/quotes")]
    [ServiceFilter(typeof(CheckAdminActionFilter))]
    public async Task<IActionResult> ListOfQuotes()
    {
        var quotes = await _sqlClient.QuoteDal.GetAll();

        return View(quotes);
    }

    [HttpPost]
    [Route("/quote/add")]
    public async Task<IActionResult> Add(AddQuoteRequestModel requestModel)
    {
        var newQuote = await _sqlClient.QuoteDal.Create(requestModel.Body, requestModel.AuthorName);

        return Ok(newQuote);
    }

    [HttpPut]
    [Route("/quote/update")]
    public async Task<IActionResult> Update(UpdateQuoteRequestModel requestModel)
    {
        var updatedQuote =
            await _sqlClient.QuoteDal.Update(requestModel.QuoteId, requestModel.Body, requestModel.AuthorName);

        return Ok(updatedQuote);
    }

    [HttpDelete]
    [Route("/quote/delete/{quoteId:int}")]
    public async Task<IActionResult> Delete(int quoteId)
    {
        await _sqlClient.QuoteDal.Delete(quoteId);

        return Ok();
    }
}