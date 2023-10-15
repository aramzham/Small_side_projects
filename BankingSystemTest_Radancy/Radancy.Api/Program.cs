using Carter;
using HashidsNet;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Radancy.Api.Data;
using Radancy.Api.Middlewares;
using Radancy.Api.Repositories;
using Radancy.Api.Repositories.Contracts;
using Radancy.Api.Services;
using Radancy.Api.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// db context
builder.Services.AddDbContext<RadancyDbContext>(options => options.UseInMemoryDatabase("RadancyInMemoryDatabase"));
// repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
// services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountService, AccountService>();
// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// mapster
builder.Services.AddMapster();
// carter
builder.Services.AddCarter();
// hashids
builder.Services.AddSingleton<IHashids>(_ => new Hashids("my_salt_keep_it_somewhere_secure", 11)); // 11 like in YouTube

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapCarter();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }