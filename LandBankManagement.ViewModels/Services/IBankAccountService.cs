using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IBankAccountService
    {
        Task<int> AddBankAccountAsync(BankAccountModel model);
        Task<BankAccountModel> GetBankAccountAsync(long id);
        Task<IList<BankAccountModel>> GetBankAccountsAsync(DataRequest<BankAccount> request);
        Task<IList<BankAccountModel>> GetBankAccountsAsync(int skip, int take, DataRequest<BankAccount> request);
        Task<int> GetBankAccountsCountAsync(DataRequest<BankAccount> request);
        Task<int> UpdateBankAccountAsync(BankAccountModel model);
        Task<int> DeleteBankAccountAsync(BankAccountModel model);
    }
}
