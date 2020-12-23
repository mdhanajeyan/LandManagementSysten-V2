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
    public class PropertyMergeService : IPropertyMergeService
    {
        public PropertyMergeService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public async Task<PropertyMergeModel> GetPropertyMergeAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetPropertyMergeAsync(dataService, id);
            }
        }
        static private async Task<PropertyMergeModel> GetPropertyMergeAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetPropertyMergeAsync(id);
            if (item != null)
            {
                return await CreatePropertyMergeModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<PropertyMergeModel>> GetPropertyMergeAsync(DataRequest<PropertyMerge> request)
        {
            var collection = new PropertyMergeCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<PropertyMergeModel>> GetPropertyMergeAsync(int skip, int take, DataRequest<PropertyMerge> request)
        {
            var models = new List<PropertyMergeModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPropertyMergeAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreatePropertyMergeModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetPropertyMergeCountAsync(DataRequest<PropertyMerge> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetPropertyMergeCountAsync(request);
            }
        }

        public async Task<PropertyMergeListModel> GetPropertyListItemForProeprty(int id) {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var obj = await dataService.GetPropertyListItemForProeprty(id);
                var model = new PropertyMergeListModel
                {
                    PropertyMergeListId = obj.PropertyMergeListId,
                    PropertyMergeGuid = obj.PropertyMergeGuid,
                    PropertyGuid = obj.PropertyGuid,
                    PropertyName = obj.PropertyName,
                    Party = obj.Party,
                    Village = obj.Village,
                    SurveyNo = obj.SurveyNo,
                    LandArea = obj.LandArea,
                    AKarab = obj.AKarab,
                    BKarab = obj.BKarab,
                    SaleValue1 = obj.SaleValue1,
                    SaleValue2 = obj.SaleValue2,
                    Amount1 = obj.Amount1,
                    Amount2 = obj.Amount2
                };
                return model;
            }
        }

        public async Task<int> AddPropertyMergeAsync(PropertyMergeModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var propertyMerge = new PropertyMerge();
               
                    UpdatePropertyMergeFromModel(propertyMerge, model);
                propertyMerge.PropertyMergeGuid= Guid.NewGuid();
                    if (model.propertyMergeLists != null && model.propertyMergeLists.Count > 0)
                    {
                        var list = new List<PropertyMergeList>();
                        foreach (var obj in model.propertyMergeLists)
                        {
                            if (obj.PropertyMergeListId > 0)
                                continue;
                            var propertyMergelist = new PropertyMergeList();
                        UpdatePropertyMergeListFromModel(propertyMergelist, obj);
                        propertyMergelist.PropertyMergeListId = 0;
                        propertyMergelist.PropertyMergeGuid = propertyMerge.PropertyMergeGuid;
                        list.Add(propertyMergelist);
                        }
                    propertyMerge.propertyMergeLists = list;

                    }
                    var mergeId = await dataService.AddPropertyMergeAsync(propertyMerge);
                    model.Merge(await GetPropertyMergeAsync(dataService, mergeId));
                    return mergeId;               
            }
        }

        public async Task<int> UpdatePropertyMergeAsync(PropertyMergeModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var propertyMerge = new PropertyMerge();

                UpdatePropertyMergeFromModel(propertyMerge, model);
                if (model.propertyMergeLists != null && model.propertyMergeLists.Count > 0)
                {
                    var list = new List<PropertyMergeList>();
                    foreach (var obj in model.propertyMergeLists)
                    {
                        if (obj.PropertyMergeListId > 0)
                            continue;
                        var propertyMergelist = new PropertyMergeList();
                        UpdatePropertyMergeListFromModel(propertyMergelist, obj);
                        propertyMergelist.PropertyMergeListId = 0;
                        propertyMergelist.PropertyMergeGuid = propertyMerge.PropertyMergeGuid;
                        list.Add(propertyMergelist);
                    }
                    propertyMerge.propertyMergeLists = list;

                }

                await dataService.UpdatePropertyMergeAsync(propertyMerge);
                model.Merge(await GetPropertyMergeAsync(dataService, propertyMerge.PropertyMergeId));

                return 0;
            }
        }

        public async Task<int> DeletePropertyMergeAsync(PropertyMergeModel model)
        {
            var propertyMerge = new PropertyMerge { PropertyMergeId = model.PropertyMergeId,PropertyMergeGuid=model.PropertyMergeGuid };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePropertyMergeAsync(propertyMerge);
            }
        }

        public async Task<int> DeletePropertyMergeItemAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePropertyMergeItemAsync(id);
            }
        }
               
        static public async Task<PropertyMergeModel> CreatePropertyMergeModelAsync(PropertyMerge source, bool includeAllFields)
        {
            var model = new PropertyMergeModel()
            {
                PropertyMergeId = source.PropertyMergeId,
                PropertyMergeGuid = source.PropertyMergeGuid,
                PropertyMergeDealName = source.PropertyMergeDealName,
                MergedTotalArea = source.MergedTotalArea,
                MergedSaleValue1 = source.MergedSaleValue1,
                MergedSaleValue2 = source.MergedSaleValue2,
                MergedAmountPaid1 = source.MergedAmountPaid1,
                MergedAmountPaid2 = source.MergedAmountPaid2,
                MergedBalancePayable1 = source.MergedBalancePayable1,
                MergedBalancePayable2 = source.MergedBalancePayable2,
                ForProposal = source.ForProposal,
            };
            if (source.propertyMergeLists != null && source.propertyMergeLists.Count > 0)
            {
                model.propertyMergeLists = new ObservableCollection<PropertyMergeListModel>();
                foreach (var obj in source.propertyMergeLists)
                {
                    model.propertyMergeLists.Add(new PropertyMergeListModel
                    {
                        PropertyMergeListId = obj.PropertyMergeListId,
                        PropertyMergeGuid = obj.PropertyMergeGuid,
                        PropertyGuid = obj.PropertyGuid,
                        PropertyName = obj.PropertyName,
                        Party = obj.Party,
                        Village = obj.Village,
                        SurveyNo = obj.SurveyNo,
                        LandArea = obj.LandArea,
                        AKarab = obj.AKarab,
                        BKarab = obj.BKarab,
                        SaleValue1 = obj.SaleValue1,
                        SaleValue2 = obj.SaleValue2,
                        Amount1 = obj.Amount1,
                        Amount2 = obj.Amount2
                    });

                }
            }

            return model;
        }

        private void UpdatePropertyMergeListFromModel(PropertyMergeList target, PropertyMergeListModel source)
        {
            target.PropertyMergeListId = source.PropertyMergeListId;
            target.PropertyMergeGuid = source.PropertyMergeGuid;
            target.PropertyGuid = source.PropertyGuid;
            target.PropertyName = source.PropertyName;
            target.Party = source.Party;
            target.Village = source.Village;
            target.SurveyNo = source.SurveyNo;
            target.LandArea = source.LandArea;
            target.AKarab = source.AKarab;
            target.BKarab = source.BKarab;
            target.SaleValue1 = source.SaleValue1;
            target.SaleValue2 = source.SaleValue2;
            target.Amount1 = source.Amount1;
            target.Amount2 = source.Amount2;
        }

        private void UpdatePropertyMergeFromModel(PropertyMerge target, PropertyMergeModel source)
        {
            target.PropertyMergeId = source.PropertyMergeId;
            target.PropertyMergeGuid = source.PropertyMergeGuid;
            target.PropertyMergeDealName = source.PropertyMergeDealName;
            target.MergedTotalArea = source.MergedTotalArea;
            target.MergedSaleValue1 = source.MergedSaleValue1;
            target.MergedSaleValue2 = source.MergedSaleValue2;
            target.MergedAmountPaid1 = source.MergedAmountPaid1;
            target.MergedAmountPaid2 = source.MergedAmountPaid2;
            target.MergedBalancePayable1 = source.MergedBalancePayable1;
            target.MergedBalancePayable2 = source.MergedBalancePayable2;
            target.ForProposal = source.ForProposal;
        }
    }
}

