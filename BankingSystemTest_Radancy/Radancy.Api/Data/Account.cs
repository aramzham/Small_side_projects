using System.ComponentModel.DataAnnotations.Schema;

namespace Radancy.Api.Data;

public class Account
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [ForeignKey("UserId")]
    public required int UserId { get; set; }
    public decimal Balance { get; set; } = 100m; // An account cannot have less than $100 at any time in an account.

    public virtual User? AccountHolder { get; set; }
    // add other properties
}