using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletTask.DAL.Interfaces;
using WalletTask.DAL.Models;

namespace WalletTask.DAL.Implementations
{
    public class UserDAL : BaseDAL, IUserDAL
    {
        public UserDAL(WalletTaskContext dbContext) : base(dbContext)
        {
        }

        public async Task Add(string name)
        {
            await _db.Users.AddAsync(new User() { Name = name });
        }

        public async Task<User> Get(int userId)
        {
            return await _db.Users.Include(x => x.Wallets).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}