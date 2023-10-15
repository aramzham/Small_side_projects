using Radancy.Api.Data;

namespace Radancy.Api.Repositories.Contracts;

public interface IUserRepository
{
    Task<User> Create();
}