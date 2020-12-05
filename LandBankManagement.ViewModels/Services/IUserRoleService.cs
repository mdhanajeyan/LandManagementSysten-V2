using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IUserRoleService
    {
        Task<int> AddUserRoleAsync(UserRoleModel model);
        Task<UserRoleModel> GetUserRoleAsync(long id);
        Task<IList<UserRoleModel>> GetUserRolesAsync(DataRequest<UserRole> request);
        Task<IList<UserRoleModel>> GetUserRolesAsync(int skip, int take, DataRequest<UserRole> request);
        Task<int> GetUserRolesCountAsync(DataRequest<UserRole> request);
        Task<int> UpdateUserRoleAsync(UserRoleModel model);
        Task<int> DeleteUserRoleAsync(UserRoleModel model);
    }
}
