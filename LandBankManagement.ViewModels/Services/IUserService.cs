using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IUserService
    {
        Task<int> AddUserAsync(UserModel model);
        Task<UserModel> GetUserAsync(long id);
        Task<IList<UserModel>> GetUsersAsync(DataRequest<User> request);
        Task<IList<UserModel>> GetUsersAsync(int skip, int take, DataRequest<User> request);
        Task<int> GetUsersCountAsync(DataRequest<User> request);
        Task<int> UpdateUserAsync(UserModel model);
        Task<int> DeleteUserAsync(UserModel model);
    }
}
