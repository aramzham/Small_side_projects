using FamousQuoteQuiz.Dal;
using FamousQuoteQuiz.MVC;
using FamousQuoteQuiz.MVC.Infrastructure;
using FamousQuoteQuiz.MVC.Infrastructure.ActionFilters;
using FamousQuoteQuiz.MVC.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddTransient<ISqlClient, SqlClient>(_ => new SqlClient(builder.Configuration["SQL_CONN_STRING"]));

builder.Services.AddTransient<CheckAdminActionFilter>();
builder.Services.AddTransient<CheckUserLoggedInActionFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();