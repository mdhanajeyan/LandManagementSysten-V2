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
    public class RolePermissionService : IRolePermissionService
    {

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public RolePermissionService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<RolePermissionModel> AddRolePermissionAsync(RolePermissionModel model)
        {
            long id = model.RolePermissionId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var rolePermission = new RolePermission();
                if (rolePermission != null)
                {
                    UpdateRolePermissionFromModel(rolePermission, model);
                    await dataService.AddRolePermissionAsync(rolePermission);
                    model.Merge(await GetRolePermissionAsync(dataService, rolePermission.RolePermissionId));
                }
                return model;
            }
        }

        static private async Task<RolePermissionModel> GetRolePermissionAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetRolePermissionAsync(id);
            if (item != null)
            {
                return await CreateRolePermissionModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<RolePermissionModel> GetRolePermissionAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetRolePermissionAsync(dataService, id);
            }
        }

        public async Task<IList<RolePermissionModel>> GetRolePermissionsAsync(DataRequest<RolePermission> request)
        {
            var collection = new RolePermissionCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<RolePermissionModel>> GetRolePermissionsAsync(int skip, int take, DataRequest<RolePermission> request)
        {
            var models = new List<RolePermissionModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetRolePermissionsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateRolePermissionModelAsync(item, includeAllFields: false));
                }
                return models;
            }
           
        }

        public async Task<int> GetRolePermissionsCountAsync(DataRequest<RolePermission> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetRolePermissionsCountAsync(request);
            }
        }

        public async Task<RolePermissionModel> UpdateRolePermissionAsync(RolePermissionModel model)
        {
            long id = model.RolePermissionId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var rolePermission = new RolePermission();
                if (rolePermission != null)
                {
                    UpdateRolePermissionFromModel(rolePermission, model);
                    await dataService.UpdateRolePermissionAsync(rolePermission);
                    model.Merge(await GetRolePermissionAsync(dataService, rolePermission.RolePermissionId));
                }
                return model;
            }
        }

        public async Task<int> DeleteRolePermissionAsync(RolePermissionModel model)
        {
            var rolePermission = new RolePermission { RolePermissionId = model.RolePermissionId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteRolePermissionAsync(rolePermission);
            }
        }


        static public async Task<RolePermissionModel> CreateRolePermissionModelAsync(RolePermission source, bool includeAllFields)
        {
            var model = new RolePermissionModel()
            {
                RolePermissionId = source.RolePermissionId,
                RoleInfoId = source.RoleInfoId,
                ScreenId = source.ScreenId,
                OptionId = source.OptionId,
                CanView = source.CanView,
        };

            return model;
        }

        private void UpdateRolePermissionFromModel(RolePermission target, RolePermissionModel source)
        {
            target.RolePermissionId = source.RolePermissionId;
            target.RoleInfoId = source.RoleInfoId;
            target.ScreenId = source.ScreenId;
            target.OptionId = source.OptionId;
            target.CanView = source.CanView;
        }

       
    }
}
