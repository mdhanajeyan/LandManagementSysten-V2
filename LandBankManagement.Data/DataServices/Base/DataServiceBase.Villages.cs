using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
   partial class DataServiceBase
    {
        public async Task<int> AddVillageAsync(Village model)
        {
            if (model == null)
                return 0;

            var entity = new Village()
            {
                VillageId = model.VillageId,
                VillageGuid = model.VillageGuid,
                TalukId = model.TalukId,
                HobliId = model.HobliId,
                VillageName = model.VillageName,
                VillageGMapLink = model.VillageGMapLink,
                VillageIsActive = model.VillageIsActive,

        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<Village> GetVillageAsync(long id)
        {
            try
            {
                return await _dataSource.Villages
                    .Where(x => x.VillageId == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private IQueryable<Village> GetVillages(DataRequest<Village> request)
        {
            IQueryable<Village> items = _dataSource.Villages;

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

        public async Task<IList<Village>> GetVillagesAsync(DataRequest<Village> request)
        {
            IQueryable<Village> items = GetVillages(request);
            return await items.ToListAsync();
        }

        public async Task<IList<Village>> GetVillagesAsync(int skip, int take, DataRequest<Village> request)
        {
            IQueryable<Village> items = GetVillages(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Village
                {
                    VillageId = source.VillageId,
                    VillageGuid = source.VillageGuid,
                    TalukId = source.TalukId,
                    HobliId = source.HobliId,
                    VillageName = source.VillageName,
                    VillageGMapLink = source.VillageGMapLink,
                    VillageIsActive = source.VillageIsActive,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetVillagesCountAsync(DataRequest<Village> request)
        {
            IQueryable<Village> items = _dataSource.Villages;

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

        public async Task<int> UpdateVillageAsync(Village model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteVillageAsync(Village model)
        {
            _dataSource.Villages.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
        public Dictionary<int, string> GetVillageOptions()
        {
            return _dataSource.Villages.Select(x => new { x.VillageId, x.VillageName }).ToDictionary(t => t.VillageId, t => t.VillageName);
        }
    }
}
