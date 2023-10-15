using System.Threading.Tasks;
using WalletTask.BL.Interfaces;
using WalletTask.DAL;

namespace WalletTask.BL.Implementations
{
    public class BaseBL : IBaseBL
    {
        protected WalletTaskDAL _dal;

        public BaseBL(WalletTaskDAL dal)
        {
            _dal = dal;
        }

        protected void SaveChanges()
        {
            _dal.SaveChanges();
        }

        protected async Task SaveChangesAsync()
        {
            await _dal.SaveChangesAsync();
        }
    }
}