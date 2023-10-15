using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WalletTask.BL.Interfaces;
using WalletTask.DAL;

namespace WalletTask.BL.Implementations
{
    public class UserBL : BaseBL, IUserBL
    {
        public UserBL(WalletTaskDAL dal) : base(dal)
        {
        }

        public async Task Add(string name)
        {
            await _dal.UserDAL.Add(name);
            await _dal.SaveChangesAsync();
        }

        public async Task<Dictionary<string, decimal>> GetBalance(int userId)
        {
            var user = await _dal.UserDAL.Get(userId);
            if (user is null)
                return null;

            var walletBalance = user.Wallets.ToDictionary(k => k.Currency, v => Math.Round(v.Amount, 2));
            return walletBalance;
        }
    }
}