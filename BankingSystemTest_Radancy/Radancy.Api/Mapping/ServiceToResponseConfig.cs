using Mapster;
using Radancy.Api.Models;

namespace Radancy.Api.Mapping;

public class ServiceToResponseConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserModel, UserResponseModel>();
        config.NewConfig<AccountModel, AccountResponseModel>();
        config.NewConfig<ValidationFailed, ValidationFailedResponseModel>();
    }
}