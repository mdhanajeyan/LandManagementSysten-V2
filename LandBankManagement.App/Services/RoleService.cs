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
    public class RoleService : IRoleService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public RoleService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddRoleAsync(RoleModel model)
        {
            long id = model.RoleId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var documentType = new Role();
                if (documentType != null)
                {
                    UpdateRoleFromModel(documentType, model);
                    await dataService.AddRoleAsync(documentType);
                    model.Merge(await GetRoleAsync(dataService, documentType.RoleId));
                }
                return 0;
            }
        }

        static private async Task<RoleModel> GetRoleAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetRoleAsync(id);
            if (item != null)
            {
                return CreateRoleModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public static RoleModel CreateRoleModelAsync(Role source, bool includeAllFields)
        {
            var model = new RoleModel()
            {
                RoleId = source.RoleId,
                Name = source.Name,
                Created = source.Created,
                CreatedBy = source.CreatedBy,
                Updated = source.Updated,
                UpdatedBy = source.UpdatedBy,
            };

            return model;
        }

        private void UpdateRoleFromModel(Role target, RoleModel source)
        {
            target.RoleId = source.RoleId;
            target.Name = source.Name;
            target.Created = source.Created;
            target.CreatedBy = source.CreatedBy;
            target.Updated = source.Updated;
            target.UpdatedBy = source.UpdatedBy;
        }

        public async Task<RoleModel> GetRoleAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetRoleAsync(dataService, id);
            }
        }

        public async Task<IList<RoleModel>> GetRolesAsync(DataRequest<Role> request)
        {
            var collection = new RoleCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<RoleModel>> GetRolesAsync(int skip, int take, DataRequest<Role> request)
        {
            var models = new List<RoleModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetRolesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateRoleModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetRolesCountAsync(DataRequest<Role> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetRolesCountAsync(request);
            }
        }

        public async Task<int> UpdateRoleAsync(RoleModel model)
        {
            long id = model.RoleId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var documentType = id > 0 ? await dataService.GetRoleAsync(model.RoleId) : new Role();
                if (documentType != null)
                {
                    UpdateRoleFromModel(documentType, model);
                    await dataService.UpdateRoleAsync(documentType);
                    model.Merge(await GetRoleAsync(dataService, documentType.RoleId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteRoleAsync(RoleModel model)
        {
            var documentType = new Role { RoleId = model.RoleId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteRoleAsync(documentType);
            }
        }
    }
}
