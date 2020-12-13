using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IRolePermissionService
    {
        Task<RolePermissionModel> AddRolePermissionAsync(RolePermissionModel model);
        Task<RolePermissionModel> GetRolePermissionAsync(long id);
        Task<IList<RolePermissionModel>> GetRolePermissionsAsync(DataRequest<RolePermission> request);
        Task<IList<RolePermissionModel>> GetRolePermissionsAsync(int skip, int take, DataRequest<RolePermission> request);
        Task<int> GetRolePermissionsCountAsync(DataRequest<RolePermission> request);
        Task<RolePermissionModel> UpdateRolePermissionAsync(RolePermissionModel model);
        Task<int> DeleteRolePermissionAsync(RolePermissionModel model);
        Task<ObservableCollection<RolePermissionModel>> GetRolePermissionsByRoleIDAsync(int roleId);

        Task<int> AddRolePermissionsAsync(ObservableCollection<RolePermissionModel> models);
    }
}
