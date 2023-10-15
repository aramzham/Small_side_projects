using Mapster;
using Radancy.Api.Models;
using Radancy.Api.Repositories.Contracts;
using Radancy.Api.Services.Contracts;

namespace Radancy.Api.Services;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserModel> Create()
    {
        var user = await _userRepository.Create();
        return user.Adapt<UserModel>();
    }
}