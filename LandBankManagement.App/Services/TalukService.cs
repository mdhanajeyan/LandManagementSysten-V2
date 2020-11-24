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
    public class TalukService : ITalukService
    {

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public TalukService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddTalukAsync(TalukModel model)
        {
            long id = model.TalukId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var taluk = new Taluk();
                if (taluk != null)
                {
                    UpdateTalukFromModel(taluk, model);
                    taluk.TalukGuid = Guid.NewGuid();
                    await dataService.AddTalukAsync(taluk);
                    model.Merge(await GetTalukAsync(dataService, taluk.TalukId));
                }
                return 0;
            }
        }

        static private async Task<TalukModel> GetTalukAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetTalukAsync(id);
            if (item != null)
            {
                return await CreateTalukModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<TalukModel> GetTalukAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetTalukAsync(dataService, id);
            }
        }

        public async Task<IList<TalukModel>> GetTaluksAsync(DataRequest<Taluk> request)
        {
            var collection = new TalukCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<TalukModel>> GetTaluksAsync(int skip, int take, DataRequest<Taluk> request)
        {
            var collection = new TalukCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<int> GetTaluksCountAsync(DataRequest<Taluk> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetTaluksCountAsync(request);
            }
        }

        public async Task<int> UpdateTalukAsync(TalukModel model)
        {
            long id = model.TalukId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var taluk = id > 0 ? await dataService.GetTalukAsync(model.TalukId) : new Taluk();
                if (taluk != null)
                {
                    UpdateTalukFromModel(taluk, model);
                    await dataService.UpdateTalukAsync(taluk);
                    model.Merge(await GetTalukAsync(dataService, taluk.TalukId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteTalukAsync(TalukModel model)
        {
            var taluk = new Taluk { TalukId = model.TalukId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteTalukAsync(taluk);
            }
        }


        static public async Task<TalukModel> CreateTalukModelAsync(Taluk source, bool includeAllFields)
        {
            var model = new TalukModel()
            {
                TalukId = source.TalukId,
                TalukGuid = source.TalukGuid,
                TalukName = source.TalukName,
                TalukGMapLink = source.TalukGMapLink,
                TalukIsActive = source.TalukIsActive,
        };

            return model;
        }

        private void UpdateTalukFromModel(Taluk target, TalukModel source)
        {
            target.TalukGuid = source.TalukGuid;
            target.TalukName = source.TalukName;
            target.TalukGMapLink = source.TalukGMapLink;
            target.TalukIsActive = source.TalukIsActive;
        }


    }
}
