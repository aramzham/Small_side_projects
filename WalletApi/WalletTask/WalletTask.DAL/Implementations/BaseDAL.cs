using WalletTask.DAL.Interfaces;
using WalletTask.DAL.Models;

namespace WalletTask.DAL.Implementations
{
    public class BaseDAL : IBaseDAL
    {
        protected WalletTaskContext _db;

        public BaseDAL(WalletTaskContext dbContext)
        {
            _db = dbContext;
        }
    }
}