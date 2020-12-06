using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IUserService
    {
        Task<int> AddUserAsync(UserInfoModel model);
        Task<UserInfoModel> GetUserAsync(long id);
        Task<IList<UserInfoModel>> GetUsersAsync(DataRequest<Data.UserInfo> request);
        Task<IList<UserInfoModel>> GetUsersAsync(int skip, int take, DataRequest<Data.UserInfo> request);
        Task<int> GetUsersCountAsync(DataRequest<Data.UserInfo> request);
        Task<int> UpdateUserAsync(UserInfoModel model);
        Task<int> DeleteUserInfoAsync(UserInfoModel model);
    }
}
