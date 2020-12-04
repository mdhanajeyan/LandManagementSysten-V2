using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddUserAsync(User model)
        {
            if (model == null)
                return 0;

            var entity = new User()
            {
                UserName = model.UserName,
                loginName = model.loginName,
                UserPassword = model.UserPassword,
                Code = model.Code,
                Email = model.Email,
                MobileNo = model.MobileNo,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                IsActive = model.IsActive,
                IsAgent = model.IsAgent,
                IsAdmin = model.IsAdmin,
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Updated = model.Updated,
                UpdatedBy = model.UpdatedBy,

            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<User> GetUserAsync(long id)
        {

            return await _dataSource.Users
                .Where(x => x.UserId == id)
                .FirstOrDefaultAsync();

        }

        public async Task<IList<User>> GetUsersAsync(DataRequest<User> request)
        {
            IQueryable<User> items = GetUsers(request);
            return await items.ToListAsync();
        }

        public async Task<IList<User>> GetUsersAsync(int skip, int take, DataRequest<User> request)
        {
            IQueryable<User> items = GetUsers(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new User
                {
                    UserId = source.UserId,
                    UserName = source.UserName,
                    loginName = source.loginName,
                    UserPassword = source.UserPassword,
                    Code = source.Code,
                    Email = source.Email,
                    MobileNo = source.MobileNo,
                    FromDate = source.FromDate,
                    ToDate = source.ToDate,
                    IsActive = source.IsActive,
                    IsAgent = source.IsAgent,
                    IsAdmin = source.IsAdmin,
                    Created = source.Created,
                    CreatedBy = source.CreatedBy,
                    Updated = source.Updated,
                    UpdatedBy = source.UpdatedBy,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<User> GetUsers(DataRequest<User> request)
        {
            IQueryable<User> items = _dataSource.Users;

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


        public async Task<int> GetUsersCountAsync(DataRequest<User> request)
        {
            IQueryable<User> items = _dataSource.Users;

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

        public async Task<int> UpdateUserAsync(User model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteUserAsync(User model)
        {
            _dataSource.Users.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
