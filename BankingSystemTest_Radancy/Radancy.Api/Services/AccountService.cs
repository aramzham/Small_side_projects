using Mapster;
using OneOf;
using Radancy.Api.Models;
using Radancy.Api.Repositories.Contracts;
using Radancy.Api.Services.Contracts;

namespace Radancy.Api.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<OneOf<ValidationFailed, AccountModel>> Create(int userId)
    {
        var result = await _accountRepository.Create(userId);
        return result.Match<OneOf<ValidationFailed, AccountModel>>(
            f => f, 
            a => a.Adapt<AccountModel>());
    }

    public async Task<OneOf<ValidationFailed, AccountModel>> Withdraw(int accountId, decimal amount)
    {
        var result = await _accountRepository.Get(accountId);
        if (result.IsT0)
            return result.AsT0;

        var account = result.AsT1;
        if (amount < 0)
            return new ValidationFailed(nameof(amount), "Withdrawal amount cannot be less than 0.");
        
        // A user cannot withdraw more than 90% of their total balance from an account in a single transaction.
        if (amount > account.Balance * 0.9m)
            return new ValidationFailed(nameof(amount),
                "Withdrawal amount is greater than 90% of the account balance.");

        // An account cannot have less than $100 at any time in an account.
        if (account.Balance - amount < 100m)
            return new ValidationFailed(nameof(amount), "Account balance cannot be less than 100.");

        account.Balance -= amount;

        await _accountRepository.SaveChanges();

        return account.Adapt<AccountModel>();
    }

    public async Task<OneOf<ValidationFailed, AccountModel>> Deposit(int accountId, decimal amount)
    {
        var result = await _accountRepository.Get(accountId);
        if (result.IsT0)
            return result.AsT0;
        
        var account = result.AsT1;
        // A user cannot deposit more than $10,000 in a single transaction.
        if (amount > 10000m)
            return new ValidationFailed(nameof(amount), "Deposit amount is greater than 10000.");
        
        account.Balance += amount;
        
        await _accountRepository.SaveChanges();
        
        return account.Adapt<AccountModel>();
    }
}