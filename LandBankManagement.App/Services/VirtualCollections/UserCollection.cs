using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class UserCollection : VirtualCollection<UserInfoModel>
    {
        private DataRequest<Data.UserInfo> _dataRequest = null;
        public IUserService UserService { get; }
        public UserCollection(IUserService cashAccountService, ILogService logService) : base(logService)
        {
            UserService = cashAccountService;
        }

        private UserInfoModel _defaultItem = UserInfoModel.CreateEmpty();
        protected override UserInfoModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Data.UserInfo> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await UserService.GetUsersCountAsync(_dataRequest);
                Ranges[0] = await UserService.GetUsersAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<UserInfoModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await UserService.GetUsersAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("UserCollection", "Fetch", ex);
            }
            return null;

        }
    }
}
