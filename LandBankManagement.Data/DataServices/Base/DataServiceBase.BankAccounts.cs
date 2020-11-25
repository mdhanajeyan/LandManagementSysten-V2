using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddBankAccountAsync(BankAccount model)
        {

            if (model == null)
                return 0;

            var entity = new BankAccount()
            {
                BankAccountId = model.BankAccountId,
                BankGuid = model.BankGuid,
                BankName = model.BankName,
                BranchName = model.BranchName,
                AccountNumber = model.AccountNumber,
                AccountType = model.AccountType,
                IFSCCode = model.IFSCCode,
                OpeningBalance = model.OpeningBalance,
                IsBankAccountActive = model.IsBankAccountActive,
            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<BankAccount> GetBankAccountAsync(long id)
        {
            return await _dataSource.BankAccounts
                .Where(x => x.BankAccountId == id).FirstOrDefaultAsync();

        }

        public async Task<IList<BankAccount>> GetBankAccountsAsync(DataRequest<BankAccount> request)
        {
            IQueryable<BankAccount> items = GetBankAccounts(request);
            return await items.ToListAsync();
        }

        public async Task<IList<BankAccount>> GetBankAccountsAsync(int skip, int take, DataRequest<BankAccount> request)
        {
            IQueryable<BankAccount> items = GetBankAccounts(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new BankAccount
                {
                    BankAccountId = source.BankAccountId,
                    BankGuid = source.BankGuid,
                    BankName = source.BankName,
                    BranchName = source.BranchName,
                    AccountNumber = source.AccountNumber,
                    AccountType = source.AccountType,
                    IFSCCode = source.IFSCCode,
                    OpeningBalance = source.OpeningBalance,
                    IsBankAccountActive = source.IsBankAccountActive,
        })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<BankAccount> GetBankAccounts(DataRequest<BankAccount> request)
        {
            IQueryable<BankAccount> items = _dataSource.BankAccounts;

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


        public async Task<int> GetBankAccountsCountAsync(DataRequest<BankAccount> request)
        {
            IQueryable<BankAccount> items = _dataSource.BankAccounts;

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

        public async Task<int> UpdateBankAccountAsync(BankAccount model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteBankAccountAsync(BankAccount model)
        {
            _dataSource.BankAccounts.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
