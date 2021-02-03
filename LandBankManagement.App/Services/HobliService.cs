using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Services
{
   public class HobliService:IHobliService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public HobliService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<HobliModel> AddHobliAsync(HobliModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var hobli = new Hobli();
                if (hobli != null)
                {
                    UpdateHobliFromModel(hobli, model);
                    hobli.HobliGuid = Guid.NewGuid();
                    hobli.HobliId = await dataService.AddHobliAsync(hobli);
                    model.Merge(await GetHobliAsync(dataService, hobli.HobliId));
                }
                return model;
            }
        }

        static private async Task<HobliModel> GetHobliAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetHobliAsync(id);
            if (item != null)
            {
                return await CreateHobliModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<HobliModel> GetHobliAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetHobliAsync(dataService, id);
            }
        }

        public async Task<IList<HobliModel>> GetHoblisAsync(DataRequest<Hobli> request)
        {
            var collection = new HobliCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<HobliModel>> GetHoblisAsync(int skip, int take, DataRequest<Hobli> request)
        {
            var models = new List<HobliModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetHoblisAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateHobliModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetHoblisCountAsync(DataRequest<Hobli> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetHoblisCountAsync(request);
            }
        }

        public async Task<HobliModel> UpdateHobliAsync(HobliModel model)
        {
            long id = model.HobliId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var hobli =  new Hobli();
                if (hobli != null)
                {
                    UpdateHobliFromModel(hobli, model);
                    await dataService.UpdateHobliAsync(hobli);
                    model.Merge(await GetHobliAsync(dataService, hobli.HobliId));
                }
                return model;
            }
        }

        public async Task<Result> DeleteHobliAsync(HobliModel model)
        {
            var hobli = new Hobli { HobliId = model.HobliId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                try
                {
                    await dataService.DeleteHobliAsync(hobli);
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && (sqlException.Number == 547))
                {
                    return Result.Error("Hobli is already in use");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Result.Ok();
            }
        }


        static public async Task<HobliModel> CreateHobliModelAsync(Hobli source, bool includeAllFields)
        {
            var model = new HobliModel()
            {
                HobliId = source.HobliId,
                HobliGuid = source.HobliGuid,
                HobliName = source.HobliName,
                HobliGMapLink = source.HobliGMapLink,
                HobliIsActive = source.HobliIsActive,
                TalukId=source.TalukId.ToString(),
                TalukName=source.TalukName
            };

            return model;
        }

        private void UpdateHobliFromModel(Hobli target, HobliModel source)
        {
            target.HobliId = source.HobliId;
            target.HobliGuid = source.HobliGuid;
            target.HobliName = source.HobliName;
            target.HobliGMapLink = source.HobliGMapLink;
            target.HobliIsActive = source.HobliIsActive;
            target.TalukId =Convert.ToInt32( source.TalukId);
            target.TalukName = source.TalukName;
        }
       
    }
}
