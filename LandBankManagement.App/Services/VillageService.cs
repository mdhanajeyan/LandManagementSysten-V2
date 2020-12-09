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
    public class VillageService : IVillageService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public VillageService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }
        public async Task<VillageModel> AddVillageAsync(VillageModel model)
        {
            long id = model.VillageId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var village = new Village();
                if (village != null)
                {
                    UpdateVillageFromModel(village, model);
                    village.VillageGuid = Guid.NewGuid();
                    await dataService.AddVillageAsync(village);
                    model.Merge(await GetVillageAsync(dataService, village.VillageId));
                }
                return model;
            }
        }

        public async Task<VillageModel> GetVillageAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetVillageAsync(dataService, id);
            }
        }

        static private async Task<VillageModel> GetVillageAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetVillageAsync(id);
            if (item != null)
            {
                return  CreateVillageModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<VillageModel>> GetVillagesAsync(DataRequest<Village> request)
        {
            var collection = new VillageCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<VillageModel>> GetVillagesAsync(int skip, int take, DataRequest<Village> request)
        {
            var models = new List<VillageModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetVillagesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateVillageModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetVillagesCountAsync(DataRequest<Village> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetVillagesCountAsync(request);
            }
        }

        public async Task<VillageModel> UpdateVillageAsync(VillageModel model)
        {
            long id = model.VillageId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var village =  new Village();
                if (village != null)
                {
                    UpdateVillageFromModel(village, model);
                    await dataService.UpdateVillageAsync(village);
                    model.Merge(await GetVillageAsync(dataService, village.VillageId));
                }
                return model;
            }
        }

        public async Task<int> DeleteVillageAsync(VillageModel model)
        {
            var village = new Village { VillageId = model.VillageId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteVillageAsync(village);
            }
        }

        public static  VillageModel CreateVillageModelAsync(Village source, bool includeAllFields)
        {
            var model = new VillageModel()
            {
                VillageId = source.VillageId,
                VillageGuid = source.VillageGuid,
                TalukId = source.TalukId,
                HobliId = source.HobliId,
                VillageName = source.VillageName,
                VillageGMapLink = source.VillageGMapLink,
                VillageIsActive = source.VillageIsActive,
                TalukName=source.TalukName,
                HobliName=source.HobliName
            };

            return model;
        }

        private void UpdateVillageFromModel(Village target, VillageModel source)
        {
            target.VillageId = source.VillageId;
            target.VillageGuid = source.VillageGuid;
            target.TalukId = source.TalukId;
            target.HobliId = source.HobliId;
            target.VillageName = source.VillageName;
            target.VillageGMapLink = source.VillageGMapLink;
            target.VillageIsActive = source.VillageIsActive;
            target.TalukName = source.TalukName;
            target.HobliName = source.HobliName;
        }

        
    }
}
