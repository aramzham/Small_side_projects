using OneOf;
using Radancy.Api.Data;
using Radancy.Api.Models;

namespace Radancy.Api.Repositories.Contracts;

public interface IAccountRepository : IBaseRepository
{
    Task<OneOf<ValidationFailed, Account>> Create(int userId);
    Task<OneOf<ValidationFailed, Account>> Get(int accountId);
}