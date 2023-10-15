using WalletTask.BL.Implementations;
using WalletTask.BL.Interfaces;
using WalletTask.DAL;

namespace WalletTask.BL
{
    public class WalletTaskBL : IWalletTaskBL
    {
        private string _connectionString;

        public WalletTaskBL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private WalletTaskDAL _dal;
        protected WalletTaskDAL DAL => _dal ?? (_dal = new WalletTaskDAL(_connectionString));

        private IUserBL _userBL;
        public IUserBL UserBL => _userBL ?? (_userBL = new UserBL(DAL));

        private IWalletBL _walletBL;
        public IWalletBL WalletBL => _walletBL ?? (_walletBL = new WalletBL(DAL));
    }

    public interface IWalletTaskBL
    {
    }
}