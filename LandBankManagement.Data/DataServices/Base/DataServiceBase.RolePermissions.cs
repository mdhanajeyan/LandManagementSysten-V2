using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddRolePermissionAsync(RolePermission model)
        {
           
                if (model == null)
                    return 0;

                var entity = new RolePermission()
                {
                    RolePermissionId = model.RolePermissionId,
                    RoleInfoId = model.RoleInfoId,
                    ScreenId = model.ScreenId,
                    OptionId = model.OptionId,
        };
                _dataSource.Entry(entity).State = EntityState.Added;
                int res = await _dataSource.SaveChangesAsync();
                return res;
           
        }

        public async Task<RolePermission> GetRolePermissionAsync(long id)
        {
            return await _dataSource.RolePermissions
                .Where(x => x.RolePermissionId == id).FirstOrDefaultAsync();

        }

        public async Task<IList<RolePermission>> GetRolePermissionsAsync(DataRequest<RolePermission> request)
        {
            IQueryable<RolePermission> items = GetRolePermissions(request);
            return await items.ToListAsync();
        }

        private IQueryable<RolePermission> GetRolePermissions(DataRequest<RolePermission> request)
        {
            IQueryable<RolePermission> items = _dataSource.RolePermissions;
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


        public async Task<IList<RolePermission>> GetRolePermissionsAsync(int skip, int take, DataRequest<RolePermission> request)
        {
            IQueryable<RolePermission> items = GetRolePermissions(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new RolePermission
                {
                    RolePermissionId = source.RolePermissionId,
                    RoleInfoId = source.RoleInfoId,
                    ScreenId = source.ScreenId,
                    OptionId = source.OptionId,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetRolePermissionsCountAsync(DataRequest<RolePermission> request)
        {
            IQueryable<RolePermission> items = _dataSource.RolePermissions;

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

        public async Task<int> UpdateRolePermissionAsync(RolePermission model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteRolePermissionAsync(RolePermission model)
        {
            _dataSource.RolePermissions.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
