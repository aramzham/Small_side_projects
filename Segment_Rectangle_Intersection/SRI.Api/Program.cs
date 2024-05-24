using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SRI.Api.Models.Request;
using SRI.Api.Services;
using SRI.Api.Services.Interfaces;
using SRI.Common;
using SRI.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<SriContext>(o => o.UseSqlite("Data Source=sri.db"));

builder.Services.AddScoped<ICheckIntersection, IntersectionChecker>();

builder.Services.AddCors(b => b.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

var app = builder.Build();

app.UseCors("AllowAll");

app.MapPost("/intersect", async (HttpContext request, ICheckIntersection intersectionChecker, SriContext dbContext) =>
{
    double x1, y1, x2, y2;
    try
    {
        using var stream = new StreamReader(request.Request.Body);
        var body = await stream.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<SegmentInput>(body, new JsonSerializerOptions(){PropertyNameCaseInsensitive = true});
        x1 = data.X1;
        y1 = data.Y1;
        x2 = data.X2;
        y2 = data.Y2;
    }
    catch (Exception ex)
    {
        return Results.BadRequest("Invalid request body format");
    }

    var rectangles = dbContext.Rectangles;

    if (!await rectangles.AnyAsync())
    {
        return Results.NotFound("No rectangles found");
    }

    var intersectingRectangles = new List<Rectangle>();
    foreach (var rectangle in rectangles)
    {
        var intersecting = intersectionChecker.IsLineIntersectingLineSegment(
            new Point(x1, y1),
            new Point(x2, y2),
            new Point(rectangle.XMin, rectangle.YMin),
            new Point(rectangle.XMax, rectangle.YMax));
        if (intersecting)
        {
            intersectingRectangles.Add(rectangle);
        }
    }

    return Results.Ok(intersectingRectangles);
});

app.Run();