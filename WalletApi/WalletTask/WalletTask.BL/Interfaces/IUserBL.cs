using System.Collections.Generic;
using System.Threading.Tasks;

namespace WalletTask.BL.Interfaces
{
    public interface IUserBL : IBaseBL
    {
        Task Add(string name);
        Task<Dictionary<string, decimal>> GetBalance(int userId);
    }
}