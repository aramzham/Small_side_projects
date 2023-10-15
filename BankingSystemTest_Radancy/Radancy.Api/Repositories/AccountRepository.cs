using OneOf;
using Radancy.Api.Data;
using Radancy.Api.Models;
using Radancy.Api.Repositories.Contracts;

namespace Radancy.Api.Repositories;

public class AccountRepository : BaseRepository, IAccountRepository
{
    public AccountRepository(RadancyDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<OneOf<ValidationFailed, Account>> Create(int userId)
    {
        var existingUser = await  _dbContext.Users.FindAsync(userId);
        if (existingUser is null)
            return new ValidationFailed(nameof(Account.UserId), "User not found");
        
        var result = await _dbContext.Accounts.AddAsync(new Account()
        {
            UserId = userId
        });

        await _dbContext.SaveChangesAsync();
        
        return result.Entity;
    }

    public async Task<OneOf<ValidationFailed, Account>> Get(int accountId)
    {
        var account = await _dbContext.Accounts.FindAsync(accountId);
        if (account is null)
            return new ValidationFailed(nameof(Account.UserId), "Account not found");

        return account;
    }
}