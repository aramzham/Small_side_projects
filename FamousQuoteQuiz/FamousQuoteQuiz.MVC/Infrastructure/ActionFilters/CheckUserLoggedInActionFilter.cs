using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FamousQuoteQuiz.MVC.Infrastructure.ActionFilters;

public class CheckUserLoggedInActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var isAuthorized = context.HttpContext.Request.Cookies.ContainsKey("userId");
        if (!isAuthorized)
            context.Result = new RedirectResult("/login");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}