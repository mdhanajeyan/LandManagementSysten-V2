using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddUserRoleAsync(UserRole model)
        {
            if (model == null)
                return 0;

            var entity = new UserRole()
            {
                UserInfoId = model.UserInfoId,
                RoleId = model.RoleId,
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Updated = model.Updated,
                UpdatedBy = model.UpdatedBy,
        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<UserRole> GetUserRoleAsync(long id)
        {
            return await _dataSource.UserRoles.Where(r => r.UserRoleId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<UserRole>> GetUserRolesAsync(DataRequest<UserRole> request)
        {
            IQueryable<UserRole> items = GetUserRoles(request);
            return await items.ToListAsync();
        }

        public async Task<IList<UserRole>> GetUserRolesAsync(int skip, int take, DataRequest<UserRole> request)
        {
            IQueryable<UserRole> items = GetUserRoles(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new UserRole
                {
                    UserInfoId = source.UserInfoId,
                    UserRoleId = source.UserRoleId,
                    RoleId = source.RoleId,
                    Created = source.Created,
                    CreatedBy = source.CreatedBy,
                    Updated = source.Updated,
                    UpdatedBy = source.UpdatedBy,
        })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<UserRole> GetUserRoles(DataRequest<UserRole> request)
        {
            IQueryable<UserRole> items = _dataSource.UserRoles;

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

        public async Task<int> GetUserRolesCountAsync(DataRequest<UserRole> request)
        {
            IQueryable<UserRole> items = _dataSource.UserRoles;

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

        public async Task<int> UpdateUserRoleAsync(UserRole userRole)
        {
            _dataSource.Entry(userRole).State = EntityState.Modified;
            userRole.SearchTerms = userRole.BuildSearchTerms();
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteUserRoleAsync(UserRole userRole)
        {
            _dataSource.UserRoles.Remove(userRole);
            return await _dataSource.SaveChangesAsync();
        }

      
    }
}
