using Radancy.Api.Data;
using Radancy.Api.Repositories.Contracts;

namespace Radancy.Api.Repositories;

public class BaseRepository : IBaseRepository, IAsyncDisposable
{
    protected readonly RadancyDbContext _dbContext;

    public BaseRepository(RadancyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChanges()
    {
        return _dbContext.SaveChangesAsync();
    }

    public ValueTask DisposeAsync()
    {
        return _dbContext.DisposeAsync();
    }
}