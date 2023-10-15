using System.Threading.Tasks;
using WalletTask.DAL.Models;

namespace WalletTask.DAL.Interfaces
{
    public interface IUserDAL : IBaseDAL
    {
        Task Add(string name);
        Task<User> Get(int userId);
    }
}