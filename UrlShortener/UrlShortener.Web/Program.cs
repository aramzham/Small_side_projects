using Microsoft.EntityFrameworkCore;
using UrlShortener.Web.Data;
using UrlShortener.Web.Entities;
using UrlShortener.Web.Extensions;
using UrlShortener.Web.Models;
using UrlShortener.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("Database")));
// this is scoped because we use DbContext inside which is scoped by itself
builder.Services.AddScoped<UrlShorteningService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.ApplyMigrations();

app.MapPost("api/shorten", async (ShortenUrlRequest request, UrlShorteningService service, ApplicationDbContext dbContext, HttpContext httpContext) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        return Results.BadRequest("The specified URL is invalid.");
    
    var code = await service.GenerateUniqueCode();

    var shortenedUrl = new ShortenedUrl()
    {
        Code = code,
        Id = Guid.NewGuid(),
        LongUrl = request.Url,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}"
    };
    
    await dbContext.ShortenedUrls.AddAsync(shortenedUrl);
    await dbContext.SaveChangesAsync();
    
    return Results.Ok(shortenedUrl.ShortUrl);
});

app.MapGet("api/{code}", async (string code, ApplicationDbContext dbContext) =>
{
    // add distributed cache for better performance
    var shortenedUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(u => u.Code == code);
    
    return shortenedUrl is null 
        ? Results.NotFound() 
        : Results.Redirect(shortenedUrl.LongUrl);
});

app.Run();
