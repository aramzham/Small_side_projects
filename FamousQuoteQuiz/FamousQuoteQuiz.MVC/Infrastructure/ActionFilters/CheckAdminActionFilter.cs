using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FamousQuoteQuiz.MVC.Infrastructure.ActionFilters;

public class CheckAdminActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var isAuthorized = context.HttpContext.Request.Cookies.ContainsKey("isAdmin");
        if (!isAuthorized)
            context.Result = new UnauthorizedResult();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}