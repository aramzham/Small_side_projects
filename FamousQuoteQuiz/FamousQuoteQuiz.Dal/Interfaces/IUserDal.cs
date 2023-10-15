using System.Collections.Generic;
using System.Threading.Tasks;
using FamousQuoteQuiz.Dal.Models;
using FamousQuoteQuiz.Dal.Models.UpdateModels;

namespace FamousQuoteQuiz.Dal.Interfaces;

public interface IUserDal : IBaseDal
{
    Task<User> GetByName(string name);
    Task<User> Add(User user);
    Task<User> GetById(int id);
    Task<User> Update(int id, UserUpdateModel updateModel);
    Task<IEnumerable<User>> GetAll();
    Task Delete(int id);
}