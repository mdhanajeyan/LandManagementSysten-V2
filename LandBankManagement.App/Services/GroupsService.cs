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
   public class GroupsService : IGroupsService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public GroupsService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddGroupsAsync(GroupsModel model)
        {
            long id = model.GroupId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var groups = new Groups();
                if (groups != null)
                {
                    UpdateGroupsFromModel(groups, model);

                    await dataService.AddGroupsAsync(groups);
                    model.Merge(await GetGroupsAsync(dataService, groups.GroupId));
                }
                return 0;
            }
        }

        static private async Task<GroupsModel> GetGroupsAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetGroupsAsync(id);
            if (item != null)
            {
                return CreateGroupsModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public static GroupsModel CreateGroupsModelAsync(Groups source, bool includeAllFields)
        {
            var model = new GroupsModel()
            {
                GroupId = source.GroupId,
                GroupName = source.GroupName,
                GroupType=source.GroupType.ToString(),
                IsActive = source.IsActive,
                GroupTypeName=(source.GroupType==1)?"Party":"Vendor"
            };

            return model;
        }

        private void UpdateGroupsFromModel(Groups target, GroupsModel source)
        {
            target.GroupId = source.GroupId;
            target.GroupName = source.GroupName;
            target.GroupType = Convert.ToInt32(source.GroupType);
            target.IsActive = source.IsActive;
        }

        public async Task<GroupsModel> GetGroupsAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetGroupsAsync(dataService, id);
            }
        }

        public async Task<IList<GroupsModel>> GetGroupsAsync(DataRequest<Groups> request)
        {
            var collection = new GroupsCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<GroupsModel>> GetGroupsAsync(int skip, int take, DataRequest<Groups> request)
        {
            var models = new List<GroupsModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetGroupsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateGroupsModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetGroupsCountAsync(DataRequest<Groups> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetGroupsCountAsync(request);
            }
        }

        public async Task<int> UpdateGroupsAsync(GroupsModel model)
        {
            long id = model.GroupId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var Groups = id > 0 ? await dataService.GetGroupsAsync(model.GroupId) : new Groups();
                if (Groups != null)
                {
                    UpdateGroupsFromModel(Groups, model);
                    await dataService.UpdateGroupsAsync(Groups);
                    model.Merge(await GetGroupsAsync(dataService, Groups.GroupId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteGroupsAsync(GroupsModel model)
        {
            var Groups = new Groups { GroupId = model.GroupId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteGroupsAsync(Groups);
            }
        }
    }
}
