using Radancy.Api.Models;

namespace Radancy.Api.Services.Contracts;

public interface IUserService
{
    Task<UserModel> Create();
    // other methods regarding user
}