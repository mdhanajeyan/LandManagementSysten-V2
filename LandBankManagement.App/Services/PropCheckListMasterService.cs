using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public class PropCheckListMasterService : IPropCheckListMasterService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public PropCheckListMasterService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddPropCheckListMasterAsync(PropCheckListMasterModel model)
        {
            long id = model.PropCheckListMasterId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var propCheckListMaster = new PropCheckListMaster();
                if (propCheckListMaster != null)
                {
                    UpdatePropCheckListMasterFromModel(propCheckListMaster, model);
                    propCheckListMaster.PropCheckListMasterGuid = Guid.NewGuid();
                    propCheckListMaster.PropCheckListMasterId = await dataService.AddPropCheckListMasterAsync(propCheckListMaster);
                    model.Merge(await GetPropCheckListMasterAsync(dataService, propCheckListMaster.PropCheckListMasterId));
                }
                return 0;
            }
        }

        static private async Task<PropCheckListMasterModel> GetPropCheckListMasterAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetPropCheckListMasterAsync(id);
            if (item != null)
            {
                return CreatePropCheckListMasterModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<PropCheckListMasterModel> GetPropCheckListMasterAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetPropCheckListMasterAsync(dataService, id);
            }
        }

        public async Task<IList<PropCheckListMasterModel>> GetPropCheckListMastersAsync(DataRequest<PropCheckListMaster> request)
        {
            var collection = new PropCheckListMasterCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<PropCheckListMasterModel>> GetPropCheckListMastersAsync(int skip, int take, DataRequest<PropCheckListMaster> request)
        {
            var models = new List<PropCheckListMasterModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPropCheckListMastersAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreatePropCheckListMasterModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetPropCheckListMastersCountAsync(DataRequest<PropCheckListMaster> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetPropCheckListMastersCountAsync(request);
            }
        }

        public async Task<int> UpdatePropCheckListMasterAsync(PropCheckListMasterModel model)
        {
            long id = model.PropCheckListMasterId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var propCheckListMaster = id > 0 ? await dataService.GetPropCheckListMasterAsync(model.PropCheckListMasterId) : new PropCheckListMaster();
                if (propCheckListMaster != null)
                {
                    UpdatePropCheckListMasterFromModel(propCheckListMaster, model);
                    await dataService.UpdatePropCheckListMasterAsync(propCheckListMaster);
                    model.Merge(await GetPropCheckListMasterAsync(dataService,propCheckListMaster.PropCheckListMasterId));
                }
                return 0;
            }
        }

        public async Task<int> DeletePropCheckListMasterAsync(PropCheckListMasterModel model)
        {
            var propCheckListMaster = new PropCheckListMaster { PropCheckListMasterId = model.PropCheckListMasterId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePropCheckListMasterAsync(propCheckListMaster);
            }
        }


        static public PropCheckListMasterModel CreatePropCheckListMasterModelAsync(PropCheckListMaster source, bool includeAllFields)
        {
            var model = new PropCheckListMasterModel()
            {
                PropCheckListMasterId = source.PropCheckListMasterId,
                PropCheckListMasterGuid = source.PropCheckListMasterGuid,
                PropCheckListMasterDescription = source.PropCheckListMasterDescription,

            };

            return model;
        }

        private void UpdatePropCheckListMasterFromModel(PropCheckListMaster target, PropCheckListMasterModel source)
        {
            target.PropCheckListMasterId = source.PropCheckListMasterId;
            target.PropCheckListMasterGuid = source.PropCheckListMasterGuid;
            target.PropCheckListMasterDescription = source.PropCheckListMasterDescription;

        }
       
    }
}
