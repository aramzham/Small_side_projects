using System.ComponentModel.DataAnnotations.Schema;

namespace Radancy.Api.Data;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}