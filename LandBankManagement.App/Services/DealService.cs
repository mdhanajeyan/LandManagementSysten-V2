using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public class DealService:IDealService
    {
        public DealService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public async Task<DealModel> GetDealAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetDealsAsync(dataService, id);
            }
        }
        static private async Task<DealModel> GetDealsAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetDealAsync(id);
            if (item != null)
            {
                return await CreateDealModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<DealModel>> GetDealsAsync(DataRequest<Deal> request)
        {
            var collection = new DealCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<DealModel>> GetDealsAsync(int skip, int take, DataRequest<Deal> request)
        {
            var models = new List<DealModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetDealsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateDealModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetDealCountAsync(DataRequest<Deal> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetDealsCountAsync(request);
            }
        }

        public async Task<int> AddDealAsync(DealModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var deal = new Deal();

                UpdateDealFromModel(deal, model);
                if (model.DealParties != null && model.DealParties.Count > 0)
                {
                    var list = new List<DealParties>();
                    foreach (var obj in model.DealParties)
                    {
                        if (obj.DealPartyId > 0)
                            continue;
                        var dealParty = new DealParties();
                        UpdateDealPartiesFromModel(dealParty, obj);
                        dealParty.DealPartyId = 0;
                        list.Add(dealParty);
                    }
                    deal.DealParties = list;
                }

                if (model.DealPaySchedules != null && model.DealPaySchedules.Count > 0)
                {
                    var list = new List<DealPaySchedule>();
                    foreach (var obj in model.DealPaySchedules)
                    {
                        if (obj.DealPayScheduleId > 0)
                            continue;
                        var pay = new DealPaySchedule();
                        UpdateDealPayScheculeFromModel(pay, obj);
                        pay.DealPayScheduleId = 0;
                        list.Add(pay);
                    }
                    deal.DealPaySchedules = list;
                }

                var dealId = await dataService.AddDealAsync(deal);
                model.Merge(await GetDealsAsync(dataService, dealId));
                return dealId;
            }
        }

        public async Task<int> UpdateDealAsync(DealModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var deal = new Deal();

                UpdateDealFromModel(deal, model);
                if (model.DealParties != null && model.DealParties.Count > 0)
                {
                    var list = new List<DealParties>();
                    foreach (var obj in model.DealParties)
                    {
                        if (obj.DealPartyId > 0)
                            continue;
                        var dealParty = new DealParties();
                        UpdateDealPartiesFromModel(dealParty, obj);
                        dealParty.DealPartyId = 0;
                        list.Add(dealParty);
                    }
                    deal.DealParties = list;
                }

                if (model.DealPaySchedules != null && model.DealPaySchedules.Count > 0)
                {
                    var list = new List<DealPaySchedule>();
                    foreach (var obj in model.DealPaySchedules)
                    {
                        if (obj.DealPayScheduleId > 0)
                            continue;
                        var pay = new DealPaySchedule();
                        UpdateDealPayScheculeFromModel(pay, obj);
                        pay.DealPayScheduleId = 0;
                        list.Add(pay);
                    }
                    deal.DealPaySchedules = list;
                }

               await dataService.UpdateDealAsync(deal);
                model.Merge(await GetDealsAsync(dataService, deal.DealId));
                return 0;
            }
        }


        public async Task<int> DeleteDealAsync(DealModel model)
        {
            var deal = new Deal { DealId=model.DealId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteDealAsync(deal);
            }
        }

        public async Task<ObservableCollection<DealPartiesModel>> GetDealParties(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetDealParties(id);
                var parties = new ObservableCollection<DealPartiesModel>();
                foreach (var obj in items) {
                    parties.Add( new DealPartiesModel
                    {
                        DealId = obj.DealId,
                        DealPartyId = obj.DealPartyId,
                        PartyId = obj.PartyId,
                        PartyName = obj.PartyName
                    });
                }
               
                return parties;
            }
        }

        public async Task<int> DeleteDealPartiesAsync(int dealPartyId)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteDealPartiesAsync(dealPartyId);
            }
        }

        public async Task<int> DeleteDealPayScheduleAsync(int dealPayId)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteDealPayScheduleAsync(dealPayId);
            }
        }

        static public async Task<DealModel> CreateDealModelAsync(Deal source, bool includeAllFields)
        {
            var model = new DealModel()
            {
                PropertyMergeId = source.PropertyMergeId,
                DealId = source.DealId,
                DealName = source.DealName,
                CompanyId = source.CompanyId,
                SaleValue1 = source.SaleValue1,
                SaleValue2 = source.SaleValue2,
                Amount1 = source.Amount1,
                Amount2 = source.Amount2,
            };
            if (source.DealParties != null && source.DealParties.Count > 0)
            {
                model.DealParties = new ObservableCollection<DealPartiesModel>();
                foreach (var obj in source.DealParties)
                {
                    model.DealParties.Add(new DealPartiesModel
                    {
                        DealId=obj.DealId,
                        DealPartyId=obj.DealPartyId,
                        PartyId=obj.PartyId,
                        PartyName=obj.PartyName
                    });

                }
            }
            if (source.DealPaySchedules != null && source.DealPaySchedules.Count > 0)
            {
                model.DealPaySchedules = new ObservableCollection<DealPayScheduleModel>();
                foreach (var obj in source.DealPaySchedules)
                {
                    model.DealPaySchedules.Add(new DealPayScheduleModel
                    {
                        DealId = obj.DealId,
                        DealPayScheduleId = obj.DealPayScheduleId,
                        Description = obj.Description,
                        ScheduleDate =obj.ScheduleDate,
                        Amount1=obj.Amount1,
                        Amount2=obj.Amount2
                    });

                }
            }

            return model;
        }

        private void UpdateDealFromModel(Deal target, DealModel source)
        {
            target.DealId = source.DealId;
            target.PropertyMergeId = source.PropertyMergeId;
            target.CompanyId = source.CompanyId;            
            target.SaleValue1 = source.SaleValue1;
            target.SaleValue2 = source.SaleValue2;           
        }

        private void UpdateDealPartiesFromModel(DealParties target, DealPartiesModel source)
        {
            target.DealId = source.DealId;
            target.DealPartyId = source.DealPartyId;
            target.PartyId = source.PartyId;
        }
        private void UpdateDealPayScheculeFromModel(DealPaySchedule target, DealPayScheduleModel source)
        {
            target.DealId = source.DealId;
            target.DealPayScheduleId = source.DealPayScheduleId;
            target.Description = source.Description;
            target.ScheduleDate = source.ScheduleDate.UtcDateTime;
            target.Amount1 = source.Amount1;
            target.Amount2 = source.Amount2;
        }


    }
}
