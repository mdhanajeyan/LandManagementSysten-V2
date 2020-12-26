using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddDealAsync(Deal model)
        {
            if (model == null)
                return 0;
            try
            {
                var entity = new Deal()
                {
                    PropertyMergeId = model.PropertyMergeId,
                    CompanyId = model.CompanyId,
                    SaleValue1 = model.SaleValue1,
                    SaleValue2 = model.SaleValue2,
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                await _dataSource.SaveChangesAsync();
                int res = entity.DealId;

                foreach (var item in model.DealParties)
                {
                    item.DealId = entity.DealId;
                    _dataSource.Entry(item).State = EntityState.Added;
                }
                await _dataSource.SaveChangesAsync();

                foreach (var item in model.DealPaySchedules)
                {
                    item.DealId = entity.DealId;
                    _dataSource.Entry(item).State = EntityState.Added;
                }
                await _dataSource.SaveChangesAsync();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Deal> GetDealAsync(long id)
        {
            var model =await  _dataSource.Deal.Where(x => x.DealId == id).Select(x => new Deal {
            DealId=x.DealId,
            PropertyMergeId=x.PropertyMergeId,
            CompanyId=x.CompanyId,
            SaleValue1=x.SaleValue1,
            SaleValue2=x.SaleValue2
            }).FirstOrDefaultAsync();

            var parties =await (from dp in _dataSource.DealParties
                           join p in _dataSource.Parties on dp.PartyId equals p.PartyId
                           where dp.DealId == id
                           select new DealParties
                           {
                               DealId = dp.DealId,
                               DealPartyId = dp.DealPartyId,
                               PartyId = dp.PartyId,
                               PartyName = p.PartyFirstName
                           }).ToListAsync();

           // var dealParties =await _dataSource.DealParties.Where(x => x.DealId == id).ToListAsync();
            if (parties.Count > 0)
                model.DealParties = parties;

            var pay = await _dataSource.DealPaySchedule.Where(x => x.DealId == id).ToListAsync();
            if(pay.Count>0)
            model.DealPaySchedules = pay;

            return model;
        }

        public async Task<List<DealParties>> GetDealParties(int dealId) {
            var models = await (from dp in _dataSource.DealParties join
                                p in _dataSource.Parties on dp.PartyId equals p.PartyId
                                where (dp.DealId == dealId)
                                select (new DealParties
                                {
                                    DealPartyId = dp.DealPartyId,
                                    DealId = dp.DealId,
                                    PartyId = dp.PartyId,
                                    PartyName = p.PartyFirstName
                                })).ToListAsync();
            return models;
        }

        public async Task<IList<Deal>> GetDealsAsync(DataRequest<Deal> request)
        {
            IQueryable<Deal> items = GetDeals(request);
            return await items.ToListAsync();
        }

        private IQueryable<Deal> GetDeals(DataRequest<Deal> request)
        {
             IQueryable<Deal> items = from d in _dataSource.Deal join
                                      pm in _dataSource.PropertyMerge on d.PropertyMergeId equals pm.PropertyMergeId join
                                      c in _dataSource.Companies on d.CompanyId equals c.CompanyID
                        select (new Deal
                        {
                            DealId = d.DealId,
                            DealName=pm.PropertyMergeDealName,
                            CompanyId = d.CompanyId,
                            SaleValue1 = d.SaleValue1,
                            SaleValue2 = d.SaleValue2,
                            Amount1 = _dataSource.DealPaySchedule.Where(x => x.DealId == d.DealId).Sum(x => x.Amount1).ToString(),
                            Amount2 = _dataSource.DealPaySchedule.Where(x => x.DealId == d.DealId).Sum(x => x.Amount2).ToString()
                        });

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

        public async Task<IList<Deal>> GetDealsAsync(int skip, int take, DataRequest<Deal> request)
        {
            IQueryable<Deal> items = GetDeals(request);
            var records = await items.Skip(skip).Take(take)
                .Select(x=>x)
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetDealsCountAsync(DataRequest<Deal> request)
        {
            IQueryable<Deal> items = _dataSource.Deal;

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

            return await items.CountAsync();
        }

        public async Task<int> UpdateDealAsync(Deal model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();

            foreach (var item in model.DealParties)
            {
                item.DealId = model.DealId;
                _dataSource.Entry(item).State = EntityState.Added;
            }
            await _dataSource.SaveChangesAsync();

            foreach (var item in model.DealPaySchedules)
            {
                item.DealId = model.DealId;
                _dataSource.Entry(item).State = EntityState.Added;
            }
            await _dataSource.SaveChangesAsync();

            return res;
        }

        public async Task<int> DeleteDealAsync(Deal model)
        {
            var parties = _dataSource.DealParties.Where(x => x.DealId == model.DealId).ToList();
            _dataSource.DealParties.RemoveRange(parties);
            await _dataSource.SaveChangesAsync();

            var pays = _dataSource.DealPaySchedule.Where(x => x.DealId == model.DealId).ToList();
            _dataSource.DealPaySchedule.RemoveRange(pays);
            await _dataSource.SaveChangesAsync();

            _dataSource.Deal.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteDealPartiesAsync(int id)
        {
            var items = _dataSource.DealParties.Where(x => x.DealPartyId == id).FirstOrDefault();
            _dataSource.DealParties.Remove(items);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteDealPayScheduleAsync(int id)
        {
            var items = _dataSource.DealPaySchedule.Where(x => x.DealPayScheduleId == id).FirstOrDefault();
            _dataSource.DealPaySchedule.Remove(items);
            return await _dataSource.SaveChangesAsync();
        }

    }
}
