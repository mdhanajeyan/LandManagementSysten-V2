using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class UserRoleService : IUserRoleService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public UserRoleService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddUserRoleAsync(UserRoleModel model)
        {
            long id = model.UserRoleId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var documentType = new UserRole();
                if (documentType != null)
                {
                    UpdateUserRoleFromModel(documentType, model);
                    await dataService.AddUserRoleAsync(documentType);
                    model.Merge(await GetUserRoleAsync(dataService, documentType.UserRoleId));
                }
                return 0;
            }
        }

        public async Task<int> AddUserRoleForUserAsync(List<UserRoleModel> models, int userId) {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var roleList = new List<UserRole>();
                foreach(var model in models)
                {
                    var role = new UserRole();
                    UpdateUserRoleFromModel(role, model);
                    roleList.Add(role);                   
                }
                return await dataService.AddUserRoleForUserAsync(roleList, userId);
            }
        }

        static private async Task<UserRoleModel> GetUserRoleAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetUserRoleAsync(id);
            if (item != null)
            {
                return CreateUserRoleModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<ObservableCollection<UserRoleModel>> GetUserRolesForUserAsync(int userId) {

            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetUserRolesForUserAsync(userId);
                ObservableCollection<UserRoleModel> userRoles = new ObservableCollection<UserRoleModel>();
                foreach (var  item in items)
                {
                    userRoles.Add( CreateUserRoleModelAsync(item, includeAllFields: true));
                }
                return userRoles;
            }
           
        }

        public static UserRoleModel CreateUserRoleModelAsync(UserRole source, bool includeAllFields)
        {
            var model = new UserRoleModel()
            {
                UserRoleId = source.UserRoleId,
                UserInfoId = source.UserInfoId,
                RoleId = source.RoleId,
                Created = source.Created,
                CreatedBy = source.CreatedBy,
                Updated = source.Updated,
                UpdatedBy = source.UpdatedBy,
                Name=source.Name,
                IsSelected=source.IsSelected
        };

            return model;
        }

        private void UpdateUserRoleFromModel(UserRole target, UserRoleModel source)
        {
            target.UserRoleId = source.UserRoleId;
            target.UserInfoId = source.UserInfoId;
            target.RoleId = source.RoleId;
            target.Created = source.Created;
            target.CreatedBy = source.CreatedBy;
            target.Updated = source.Updated;
            target.UpdatedBy = source.UpdatedBy;
            target.IsSelected = source.IsSelected;
        }

        public async Task<UserRoleModel> GetUserRoleAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetUserRoleAsync(dataService, id);
            }
        }

        public async Task<IList<UserRoleModel>> GetUserRolesAsync(DataRequest<UserRole> request)
        {
            var collection = new UserRoleCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<UserRoleModel>> GetUserRolesAsync(int skip, int take, DataRequest<UserRole> request)
        {
            var models = new List<UserRoleModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetUserRolesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateUserRoleModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetUserRolesCountAsync(DataRequest<UserRole> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetUserRolesCountAsync(request);
            }
        }

        public async Task<int> UpdateUserRoleAsync(UserRoleModel model)
        {
            long id = model.UserRoleId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var documentType = id > 0 ? await dataService.GetUserRoleAsync(model.UserRoleId) : new UserRole();
                if (documentType != null)
                {
                    UpdateUserRoleFromModel(documentType, model);
                    await dataService.UpdateUserRoleAsync(documentType);
                    model.Merge(await GetUserRoleAsync(dataService, documentType.UserRoleId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteUserRoleAsync(UserRoleModel model)
        {
            var documentType = new UserRole { UserRoleId = model.UserRoleId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteUserRoleAsync(documentType);
            }
        }
    }
}
