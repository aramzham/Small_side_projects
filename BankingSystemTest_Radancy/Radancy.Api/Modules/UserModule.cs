using Carter;
using HashidsNet;
using Mapster;
using MapsterMapper;
using Radancy.Api.Models;
using Radancy.Api.Services.Contracts;

namespace Radancy.Api.Modules;

public class UserModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/user");

        userGroup.MapPost("/", Create);
    }

    private async Task<IResult> Create(IUserService userService, IMapper mapper, IHashids hashids)
    {
        var user = await userService.Create();
        var userResponse = user.Adapt<UserResponseModel>() with
        {
            Id = hashids.Encode(user.Id)
        };
        return Results.Ok(userResponse);
    }
}