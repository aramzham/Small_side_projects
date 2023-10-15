namespace Radancy.Api.Models;

public class AccountModel
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required decimal Balance { get; set; }
}