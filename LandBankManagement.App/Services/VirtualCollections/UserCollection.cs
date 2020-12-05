using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class UserCollection : VirtualCollection<UserModel>
    {
        private DataRequest<User> _dataRequest = null;
        public IUserService UserService { get; }
        public UserCollection(IUserService cashAccountService, ILogService logService) : base(logService)
        {
            UserService = cashAccountService;
        }

        private UserModel _defaultItem = UserModel.CreateEmpty();
        protected override UserModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<User> dataRequest)
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

        protected override async Task<IList<UserModel>> FetchDataAsync(int rangeIndex, int rangeSize)
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
