﻿using Microsoft.EntityFrameworkCore;
using UrlShortener.Web.Data;

namespace UrlShortener.Web.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
}