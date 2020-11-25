using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface ICashAccountService
    {
        Task<int> AddCashAccountAsync(CashAccountModel model);
        Task<CashAccountModel> GetCashAccountAsync(long id);
        Task<IList<CashAccountModel>> GetCashAccountsAsync(DataRequest<CashAccount> request);
        Task<IList<CashAccountModel>> GetCashAccountsAsync(int skip, int take, DataRequest<CashAccount> request);
        Task<int> GetCashAccountsCountAsync(DataRequest<CashAccount> request);
        Task<int> UpdateCashAccountAsync(CashAccountModel model);
        Task<int> DeleteCashAccountAsync(CashAccountModel model);
    }
}
