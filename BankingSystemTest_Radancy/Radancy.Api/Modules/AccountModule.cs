using Carter;
using HashidsNet;
using Mapster;
using Radancy.Api.Models;
using Radancy.Api.Services.Contracts;

namespace Radancy.Api.Modules;

public class AccountModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var accountGroup = app.MapGroup("/account");

        accountGroup.MapPost("/", Create);
        accountGroup.MapPost("/withdraw", Withdraw);
        accountGroup.MapPost("/deposit", Deposit);
    }

    private async Task<IResult> Create(CreateAccountRequestModel requestModel, IAccountService service, IHashids hashids)
    {
        var rawId = hashids.Decode(requestModel.UserId);
        if (rawId.Length == 0)
            return Results.NotFound();
        
        var result = await service.Create(rawId[0]);
        return result.Match(
                f => Results.BadRequest(f.Adapt<ValidationFailedResponseModel>()),
                account =>
                {
                    var response = account.Adapt<AccountResponseModel>() with // this code repetition must be moved to the mapper config somehow
                    {
                        UserId = hashids.Encode(account.UserId),
                        Id = hashids.Encode(account.Id)
                    };
        
                    return Results.Ok(response);
                }
        );
    }

    private async Task<IResult> Withdraw(WithdrawRequestModel requestModel, IAccountService service, IHashids hashids)
    {
        var rawId = hashids.Decode(requestModel.AccountId);
        if (rawId.Length == 0)
            return Results.NotFound();
        
        var result = await service.Withdraw(rawId[0], requestModel.Amount);
        return result.Match(
            f => Results.BadRequest(f.Adapt<ValidationFailedResponseModel>()),
            account =>
            {
                var response = account.Adapt<AccountResponseModel>() with // this code repetition must be moved to the mapper config somehow
                {
                    UserId = hashids.Encode(account.UserId),
                    Id = hashids.Encode(account.Id)
                };
        
                return Results.Ok(response);
            }
        );
    }

    private async Task<IResult> Deposit(DepositRequestModel requestModel, IAccountService service, IHashids hashids)
    {
        var rawId = hashids.Decode(requestModel.AccountId);
        if (rawId.Length == 0)
            return Results.NotFound();
        
        var result = await service.Deposit(rawId[0], requestModel.Amount);
        return result.Match(
            f => Results.BadRequest(f.Adapt<ValidationFailedResponseModel>()),
            account =>
            {
                var response = account.Adapt<AccountResponseModel>() with // this code repetition must be moved to the mapper config somehow
                {
                    UserId = hashids.Encode(account.UserId),
                    Id = hashids.Encode(account.Id)
                };
        
                return Results.Ok(response);
            }
        );
    }
}