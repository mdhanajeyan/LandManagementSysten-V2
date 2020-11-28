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
    public class CheckListService : ICheckListService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public CheckListService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddCheckListAsync(CheckListModel model)
        {
            long id = model.CheckListId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var documentType = new CheckList();
                if (documentType != null)
                {
                    UpdateCheckListFromModel(documentType, model);
                    documentType.CheckListGuid = Guid.NewGuid();
                    await dataService.AddCheckListAsync(documentType);
                    model.Merge(await GetCheckListAsync(dataService, documentType.CheckListId));
                }
                return 0;
            }
        }

        static private async Task<CheckListModel> GetCheckListAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetCheckListAsync(id);
            if (item != null)
            {
                return CreateCheckListModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public static CheckListModel CreateCheckListModelAsync(CheckList source, bool includeAllFields)
        {
            var model = new CheckListModel()
            {
                CheckListId = source.CheckListId,
                CheckListGuid = source.CheckListGuid,
                CheckListName = source.CheckListName,
                CheckListIsActive=source.CheckListIsActive
            };

            return model;
        }

        private void UpdateCheckListFromModel(CheckList target, CheckListModel source)
        {
            target.CheckListId = source.CheckListId;
            target.CheckListGuid = source.CheckListGuid;
            target.CheckListName = source.CheckListName;
            target.CheckListIsActive = source.CheckListIsActive;
        }

        public async Task<CheckListModel> GetCheckListAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetCheckListAsync(dataService, id);
            }
        }

        public async Task<IList<CheckListModel>> GetCheckListsAsync(DataRequest<CheckList> request)
        {
            var collection = new CheckListCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<CheckListModel>> GetCheckListsAsync(int skip, int take, DataRequest<CheckList> request)
        {
            var models = new List<CheckListModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCheckListsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateCheckListModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetCheckListsCountAsync(DataRequest<CheckList> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetCheckListsCountAsync(request);
            }
        }

        public async Task<int> UpdateCheckListAsync(CheckListModel model)
        {
            long id = model.CheckListId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var documentType = id > 0 ? await dataService.GetCheckListAsync(model.CheckListId) : new CheckList();
                if (documentType != null)
                {
                    UpdateCheckListFromModel(documentType, model);
                    await dataService.UpdateCheckListAsync(documentType);
                    model.Merge(await GetCheckListAsync(dataService, documentType.CheckListId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteCheckListAsync(CheckListModel model)
        {
            var documentType = new CheckList { CheckListId = model.CheckListId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteCheckListAsync(documentType);
            }
        }
    }
}
