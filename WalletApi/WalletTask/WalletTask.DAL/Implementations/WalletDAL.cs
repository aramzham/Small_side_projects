using System.Threading.Tasks;
using WalletTask.DAL.Interfaces;
using WalletTask.DAL.Models;

namespace WalletTask.DAL.Implementations
{
    public class WalletDAL : BaseDAL, IWalletDAL
    {
        public WalletDAL(WalletTaskContext dbContext) : base(dbContext)
        {
        }

        public async Task Add(int userId, string currency)
        {
            await _db.Wallets.AddAsync(new Wallet() {UserId = userId, Currency = currency});
        }
    }
}