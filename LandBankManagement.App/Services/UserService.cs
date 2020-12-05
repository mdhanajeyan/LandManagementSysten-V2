using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<int> AddUserAsync(UserModel model)
        {
            long id = model.UserId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var user = new User();
                if (user != null)
                {
                    UpdateUserFromModel(user, model);
                    await dataService.AddUserAsync(user);
                    model.Merge(await GetUserAsync(dataService, user.UserId));
                }

                return 0;
            }
        }

        static private async Task<UserModel> GetUserAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetUserAsync(id);
            if (item != null)
            {
                return CreateUserModelAsync(item, includeAllFields: true);
            }

            return null;
        }

        public async Task<UserModel> GetUserAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetUserAsync(dataService, id);
            }
        }

        public async Task<IList<UserModel>> GetUsersAsync(DataRequest<User> request)
        {
            var collection = new UserCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<UserModel>> GetUsersAsync(int skip, int take, DataRequest<User> request)
        {
            var models = new List<UserModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetUsersAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateUserModelAsync(item, includeAllFields: false));
                }

                return models;
            }
        }

        public async Task<int> GetUsersCountAsync(DataRequest<User> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetUsersCountAsync(request);
            }
        }

        public async Task<int> UpdateUserAsync(UserModel model)
        {
            long id = model.UserId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var user = new User();
                if (user != null)
                {
                    UpdateUserFromModel(user, model);
                    await dataService.UpdateUserAsync(user);
                    model.Merge(await GetUserAsync(dataService, user.UserId));
                }

                return 0;
            }
        }

        public async Task<int> DeleteUserAsync(UserModel model)
        {
            var bankAcc = new User {UserId = model.UserId};
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteUserAsync(bankAcc);
            }
        }

        static public UserModel CreateUserModelAsync(User source, bool includeAllFields)
        {
            var model = new UserModel()
            {
                UserId = source.UserId,
                UserName = source.UserName,
                loginName = source.loginName,
                UserPassword = source.UserPassword,
                Code = source.Code,
                Email = source.Email,
                MobileNo = source.MobileNo,
                FromDate = source.FromDate,
                ToDate = source.ToDate,
                IsActive = source.IsActive,
                IsAgent = source.IsAgent,
                IsAdmin = source.IsAdmin,
                Created = source.Created,
                CreatedBy = source.CreatedBy,
                Updated = source.Updated,
                UpdatedBy = source.UpdatedBy,
            };
            return model;
        }

        private void UpdateUserFromModel(User target, UserModel source)
        {
            target.UserId = source.UserId;
            target.UserName = source.UserName;
            target.loginName = source.loginName;
            target.UserPassword = source.UserPassword;
            target.Code = source.Code;
            target.Email = source.Email;
            target.MobileNo = source.MobileNo;
            target.FromDate = source.FromDate;
            target.ToDate = source.ToDate;
            target.IsActive = source.IsActive;
            target.IsAgent = source.IsAgent;
            target.IsAdmin = source.IsAdmin;
            target.Created = source.Created;
            target.CreatedBy = source.CreatedBy;
            target.Updated = source.Updated;
            target.UpdatedBy = source.UpdatedBy;
        }
    }

}

