namespace Radancy.Api.Models;

public record CreateAccountRequestModel(string UserId);

public record WithdrawRequestModel(string AccountId, decimal Amount);

public record DepositRequestModel(string AccountId, decimal Amount);