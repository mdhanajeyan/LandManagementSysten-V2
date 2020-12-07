using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class UserService : IUserService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public UserService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddUserAsync(UserInfoModel model)
        {
            long id = model.UserInfoId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var user = new Data.UserInfo();
                int userid = 0;
                if (user != null)
                {
                    UpdateUserFromModel(user, model);
                    userid = await dataService.AddUserInfoAsync(user);
                    model.Merge(await GetUserAsync(dataService, user.UserInfoId));
                }

                return userid;
            }
        }

        static private async Task<UserInfoModel> GetUserAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetUserInfoAsync(id);
            if (item != null)
            {
                return CreateUserModelAsync(item, includeAllFields: true);
            }

            return null;
        }

        public async Task<UserInfoModel> GetUserAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetUserAsync(dataService, id);
            }
        }

        public async Task<IList<UserInfoModel>> GetUsersAsync(DataRequest<Data.UserInfo> request)
        {
            var collection = new UserCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<UserInfoModel>> GetUsersAsync(int skip, int take, DataRequest<Data.UserInfo> request)
        {
            var models = new List<UserInfoModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetUserInfosAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateUserModelAsync(item, includeAllFields: false));
                }

                return models;
            }
        }

        public async Task<int> GetUsersCountAsync(DataRequest<Data.UserInfo> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetUserInfosCountAsync(request);
            }
        }

        public async Task<int> UpdateUserAsync(UserInfoModel model)
        {
            long id = model.UserInfoId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var user = new Data.UserInfo();
                if (user != null)
                {
                    UpdateUserFromModel(user, model);
                    await dataService.UpdateUserInfoAsync(user);
                    model.Merge(await GetUserAsync(dataService, user.UserInfoId));
                }

                return 0;
            }
        }

        public async Task<int> DeleteUserInfoAsync(UserInfoModel model)
        {
            var bankAcc = new Data.UserInfo { UserInfoId = model.UserInfoId};
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteUserInfoAsync(bankAcc);
            }
        }

        static public UserInfoModel CreateUserModelAsync(Data.UserInfo source, bool includeAllFields)
        {
            var model = new UserInfoModel()
            {
                UserInfoId = source.UserInfoId,
                UserName = source.UserName,
                loginName = source.loginName,
                UserPassword = source.UserPassword,
                Email = source.Email,
                MobileNo = source.MobileNo,
                IsActive = source.IsActive,
                IsAdmin = source.IsAdmin,
                Created = source.Created,
                CreatedBy = source.CreatedBy,
                Updated = source.Updated,
                UpdatedBy = source.UpdatedBy,
            };
            return model;
        }

        private void UpdateUserFromModel(Data.UserInfo target, UserInfoModel source)
        {
            target.UserInfoId = source.UserInfoId;
            target.UserName = source.UserName;
            target.loginName = source.loginName;
            target.UserPassword = source.UserPassword;
            target.Email = source.Email;
            target.MobileNo = source.MobileNo;
            target.IsActive = source.IsActive;
            target.IsAdmin = source.IsAdmin;
            target.Created = source.Created;
            target.CreatedBy = source.CreatedBy;
            target.Updated = source.Updated;
            target.UpdatedBy = source.UpdatedBy;
        }
    }

}

