using System.Threading.Tasks;

namespace WalletTask.DAL.Interfaces
{
    public interface IWalletDAL : IBaseDAL
    {
        Task Add(int userId, string currency);
    }
}