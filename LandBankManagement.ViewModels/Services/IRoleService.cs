using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IRoleService
    {
        Task<int> AddRoleAsync(RoleModel model);
        Task<RoleModel> GetRoleAsync(long id);
        Task<IList<RoleModel>> GetRolesAsync(DataRequest<Role> request);
        Task<IList<RoleModel>> GetRolesAsync(int skip, int take, DataRequest<Role> request);
        Task<int> GetRolesCountAsync(DataRequest<Role> request);
        Task<int> UpdateRoleAsync(RoleModel model);
        Task<int> DeleteRoleAsync(RoleModel model);
    }
}
