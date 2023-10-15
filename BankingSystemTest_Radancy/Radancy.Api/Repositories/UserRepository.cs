using Radancy.Api.Data;
using Radancy.Api.Repositories.Contracts;

namespace Radancy.Api.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(RadancyDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User> Create()
    {
        var result = await _dbContext.AddAsync(new User());
        
        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }
}