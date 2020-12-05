using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class UserRoleCollection : VirtualCollection<UserRoleModel>
    {
        private DataRequest<UserRole> _dataRequest = null;
        public IUserRoleService UserRoleService { get; }
        public UserRoleCollection(IUserRoleService userRoleService, ILogService logService) : base(logService)
        {
            UserRoleService = userRoleService;
        }

        private UserRoleModel _defaultItem = UserRoleModel.CreateEmpty();
        protected override UserRoleModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<UserRole> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await UserRoleService.GetUserRolesCountAsync(_dataRequest);
        }

        protected override async Task<IList<UserRoleModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            return await UserRoleService.GetUserRolesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
        }
    }
}
