using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface IExpenseHeadService
    {
        Task<int> AddExpenseHeadAsync(ExpenseHeadModel model);
        Task<ExpenseHeadModel> GetExpenseHeadAsync(long id);
        Task<IList<ExpenseHeadModel>> GetExpenseHeadsAsync(DataRequest<ExpenseHead> request);
        Task<IList<ExpenseHeadModel>> GetExpenseHeadsAsync(int skip, int take, DataRequest<ExpenseHead> request);
        Task<int> GetExpenseHeadsCountAsync(DataRequest<ExpenseHead> request);

        Task<int> UpdateExpenseHeadAsync(ExpenseHeadModel model);

        Task<int> DeleteExpenseHeadAsync(ExpenseHeadModel model);
    }
}
