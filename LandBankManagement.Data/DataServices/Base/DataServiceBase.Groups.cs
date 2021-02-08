using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddGroupsAsync(Groups model)
        {
            if (model == null)
                return 0;

            var entity = new Groups()
            {
                GroupId = model.GroupId,
                GroupName = model.GroupName,
                IsActive = model.IsActive,
                GroupType=model.GroupType
            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<Groups> GetGroupsAsync(long id)
        {
            return await _dataSource.Groups
                .Where(r => r.GroupId == id).FirstOrDefaultAsync();
        }

        private IQueryable<Groups> GetGroups(DataRequest<Groups> request)
        {
            IQueryable<Groups> items = _dataSource.Groups;

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


        public async Task<IList<Groups>> GetGroupsAsync(DataRequest<Groups> request)
        {
            IQueryable<Groups> items = GetGroups(request);
            return await items.ToListAsync();
        }

        public async Task<IList<Groups>> GetGroupsAsync(int skip, int take, DataRequest<Groups> request)
        {
            IQueryable<Groups> items = GetGroups(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Groups
                {
                    GroupId = source.GroupId,
                    GroupName = source.GroupName,
                    GroupType=source.GroupType,
                    IsActive = source.IsActive,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetGroupsCountAsync(DataRequest<Groups> request)
        {
            IQueryable<Groups> items = _dataSource.Groups;

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

        public async Task<int> UpdateGroupsAsync(Groups model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteGroupsAsync(Groups model)
        {
            _dataSource.Groups.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
