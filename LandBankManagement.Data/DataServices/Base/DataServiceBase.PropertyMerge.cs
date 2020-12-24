using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPropertyMergeAsync(PropertyMerge model)
        {
            if (model == null)
                return 0;
            try
            {
                var entity = new PropertyMerge()
                {
                    PropertyMergeGuid = model.PropertyMergeGuid,
                    PropertyMergeDealName = model.PropertyMergeDealName,
                    MergedTotalArea = model.MergedTotalArea,
                    MergedSaleValue1 = model.MergedSaleValue1,
                    MergedSaleValue2 = model.MergedSaleValue2,
                    MergedAmountPaid1 = model.MergedAmountPaid1,
                    MergedAmountPaid2 = model.MergedAmountPaid2,
                    MergedBalancePayable1 = model.MergedBalancePayable1,
                    MergedBalancePayable2 = model.MergedBalancePayable2,
                    ForProposal = model.ForProposal
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                int res = await _dataSource.SaveChangesAsync();

                foreach (var item in model.propertyMergeLists)
                {
                    _dataSource.Entry(item).State = EntityState.Added;
                }
                await _dataSource.SaveChangesAsync();

                return entity.PropertyMergeId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PropertyMergeList> GetPropertyListItemForProeprty(int id)
        {
            var partyname = await (from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == id)
                                   from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId)
                                  select party.PartyFirstName).ToListAsync();
           

            var item = await (from pt in _dataSource.Properties.Where(x => x.PropertyId == id)
                              from v in _dataSource.Villages.Where(x => x.VillageId == pt.VillageId).DefaultIfEmpty()
                              
                              select new PropertyMergeList
                              {
                                  PropertyGuid = pt.PropertyGuid,
                                  PropertyName = pt.PropertyName,
                                  Village = v.VillageName,
                                  SurveyNo = pt.SurveyNo,
                                  LandArea = pt.LandAreaInputAcres + "-" + pt.LandAreaInputGuntas + "-" + pt.LandAreaInputAanas,
                                  SaleValue1 = pt.SaleValue1.ToString(),
                                  SaleValue2 = pt.SaleValue2.ToString(),
                                  Amount1 = _dataSource.PropPaySchedules.Where(x => x.PropertyId == pt.PropertyId).Sum(x => x.Amount1).ToString(),
                                  Amount2 = _dataSource.PropPaySchedules.Where(x => x.PropertyId == pt.PropertyId).Sum(x => x.Amount1).ToString(),
                                    Party=string.Join(",",partyname)
                              }).FirstOrDefaultAsync();
            return item;

        }
              

        public async Task<PropertyMerge> GetPropertyMergeAsync(long id)
        {
            var merge = await _dataSource.PropertyMerge.Where(r => r.PropertyMergeId == id).FirstOrDefaultAsync();

            if (_dataSource.PropertyMergeList.Where(x => x.PropertyMergeGuid == merge.PropertyMergeGuid).Count() > 0)
            {

                var list = (from pm in _dataSource.PropertyMergeList.Where(x => x.PropertyMergeGuid == merge.PropertyMergeGuid)
                            from pt in _dataSource.Properties.Where(x => x.PropertyGuid == pm.PropertyGuid).DefaultIfEmpty()
                            from c in _dataSource.Companies.Where(x => x.CompanyID == pt.CompanyID).DefaultIfEmpty()
                            from v in _dataSource.Villages.Where(x => x.VillageId == pt.VillageId).DefaultIfEmpty()

                            select new PropertyMergeList
                            {
                                PropertyMergeListId = pm.PropertyMergeListId,
                                PropertyMergeGuid = merge.PropertyMergeGuid,
                                PropertyGuid = pm.PropertyGuid,
                                PropertyName = pt.PropertyName,
                                Village = v.VillageName,
                                SurveyNo = pt.SurveyNo,
                                LandArea = pt.LandAreaInputAcres + "-" + pt.LandAreaInputGuntas + "-" + pt.LandAreaInputAanas,
                                AKarab = pt.AKarabAreaInputAcres + "-" + pt.AKarabAreaInputGuntas + "-" + pt.AKarabAreaInputAanas,
                                BKarab = pt.BKarabAreaInputAcres + "-" + pt.BKarabAreaInputGuntas + "-" + pt.BKarabAreaInputAanas,
                                SaleValue1 = pt.SaleValue1.ToString(),
                                SaleValue2 = pt.SaleValue2.ToString(),
                                Amount1 = _dataSource.PropPaySchedules.Where(x => x.PropertyId == pt.PropertyId).Sum(x => x.Amount1).ToString(),
                                Amount2 = _dataSource.PropPaySchedules.Where(x => x.PropertyId == pt.PropertyId).Sum(x => x.Amount1).ToString()
                            }).ToList();

                foreach (var item in list) {
                    var partyname = await (from p in _dataSource.Properties.Where(x=>x.PropertyGuid==item.PropertyGuid)
                        from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == p.PropertyId)
                        from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId)
                        select party.PartyFirstName).ToListAsync();
                    item.Party = string.Join(",", partyname);
                }

                merge.propertyMergeLists = list;
            }
            return merge;
        }

        public async Task<IList<PropertyMerge>> GetPropertyMergeAsync(DataRequest<PropertyMerge> request)
        {
            IQueryable<PropertyMerge> items = GetPropertyMerge(request);
            return await items.ToListAsync();
        }

        public async Task<IList<PropertyMerge>> GetPropertyMergeAsync(int skip, int take, DataRequest<PropertyMerge> request)
        {
            IQueryable<PropertyMerge> items = GetPropertyMerge(request);
            var records = await items.Skip(skip).Take(take)
                .Select(x => x)
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<PropertyMerge> GetPropertyMerge(DataRequest<PropertyMerge> request)
        {

            IQueryable<PropertyMerge> items = _dataSource.PropertyMerge;
            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            }

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            // Order By
            if (request.OrderBy != null)
            {
                items = items.OrderBy(request.OrderBy);
            }
            if (request.OrderByDesc != null)
            {
                items = items.OrderByDescending(request.OrderByDesc);
            }
            return items;
        }

        public async Task<int> GetPropertyMergeCountAsync(DataRequest<PropertyMerge> request)
        {

            IQueryable<PropertyMerge> items = _dataSource.PropertyMerge;
            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.BuildSearchTerms().Contains(request.Query.ToLower()));
            }

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            return await items.CountAsync();
        }

        public async Task<int> UpdatePropertyMergeAsync(PropertyMerge model)
        {
            try
            {
                _dataSource.Entry(model).State = EntityState.Modified;
                int res = await _dataSource.SaveChangesAsync();

                foreach (var pay in model.propertyMergeLists)
                {
                    _dataSource.Entry(pay).State = EntityState.Added;
                }
                await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
            }

        public async Task<int> DeletePropertyMergeAsync(PropertyMerge model)
        {
            try
            {
                var entities = _dataSource.PropertyMergeList.Where(x => x.PropertyMergeGuid == model.PropertyMergeGuid);
                if (entities != null)
                {
                    _dataSource.PropertyMergeList.RemoveRange(entities);
                     await _dataSource.SaveChangesAsync();
                }

                _dataSource.PropertyMerge.Remove(model);
                return await _dataSource.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> DeletePropertyMergeItemAsync(int id)
        {
            try
            {
                var entity = _dataSource.PropertyMergeList.Where(x => x.PropertyMergeListId == id).FirstOrDefault();
                if (entity != null)
                {
                    _dataSource.PropertyMergeList.Remove(entity);
                    return await _dataSource.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

    }

}
