using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class RolePermissionCollection : VirtualCollection<RolePermissionModel>
    {
        private DataRequest<RolePermission> _dataRequest = null;
        public IRolePermissionService RolePermissionService { get; }
        public RolePermissionCollection(IRolePermissionService bankAccountService, ILogService logService) : base(logService)
        {
            RolePermissionService = bankAccountService;
        }

        private RolePermissionModel _defaultItem = RolePermissionModel.CreateEmpty();
        protected override RolePermissionModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<RolePermission> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await RolePermissionService.GetRolePermissionsCountAsync(_dataRequest);
                Ranges[0] = await RolePermissionService.GetRolePermissionsAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<RolePermissionModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await RolePermissionService.GetRolePermissionsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("RolePermissionCollection", "Fetch", ex);
            }
            return null;

        }

    }
}
