using System;
using System.Threading.Tasks;
using WalletTask.DAL.Implementations;
using WalletTask.DAL.Interfaces;
using WalletTask.DAL.Models;

namespace WalletTask.DAL
{
    public class WalletTaskDAL : IDisposable
    {
        private readonly string _connectionString;

        private WalletTaskContext _db;

        protected WalletTaskContext DB => _db ?? (_db = new WalletTaskContext(_connectionString));

        public WalletTaskDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public WalletTaskDAL(WalletTaskContext dbContext)
        {
            _db = dbContext;
        }

        #region DALs

        private IUserDAL _userDAL;
        public IUserDAL UserDAL => _userDAL ?? (_userDAL = new UserDAL(DB));

        private IWalletDAL _walletDAL;
        public IWalletDAL WalletDAL => _walletDAL ?? (_walletDAL = new WalletDAL(DB));

        #endregion

        #region SaveChanges

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        #endregion

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}