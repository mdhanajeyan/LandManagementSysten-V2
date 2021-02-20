using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class RoleCollection : VirtualCollection<RoleModel>
    {
        private DataRequest<Role> _dataRequest = null;
        public IRoleService RoleService { get; }
        public RoleCollection(IRoleService roleService, ILogService logService) : base(logService)
        {
            RoleService = roleService;
        }

        private RoleModel _defaultItem = RoleModel.CreateEmpty();
        protected override RoleModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Role> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await RoleService.GetRolesCountAsync(_dataRequest);
            Ranges[0] = await RoleService.GetRolesAsync(0, RangeSize, _dataRequest);
        }

        protected override async Task<IList<RoleModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            return await RoleService.GetRolesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
        }
    }
}
