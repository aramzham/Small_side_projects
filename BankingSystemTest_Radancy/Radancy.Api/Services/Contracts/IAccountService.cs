using OneOf;
using Radancy.Api.Models;

namespace Radancy.Api.Services.Contracts;

public interface IAccountService
{
    Task<OneOf<ValidationFailed, AccountModel>> Create(int userId);
    Task<OneOf<ValidationFailed, AccountModel>> Withdraw(int accountId, decimal amount);
    Task<OneOf<ValidationFailed, AccountModel>> Deposit(int accountId, decimal amount);
}