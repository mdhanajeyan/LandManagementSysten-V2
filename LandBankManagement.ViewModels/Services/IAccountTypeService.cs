using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IAccountTypeService
    {
        Task<int> AddAccountTypeAsync(AccountTypeModel model);
        Task<AccountTypeModel> GetAccountTypeAsync(long id);
        Task<IList<AccountTypeModel>> GetAccountTypesAsync(DataRequest<AccountType> request);
        Task<IList<AccountTypeModel>> GetAccountTypesAsync(int skip, int take, DataRequest<AccountType> request);
        Task<int> GetAccountTypesCountAsync(DataRequest<AccountType> request);
        Task<int> UpdateAccountTypeAsync(AccountTypeModel model);
        Task<int> DeleteAccountTypeAsync(AccountTypeModel model);
    }
}
