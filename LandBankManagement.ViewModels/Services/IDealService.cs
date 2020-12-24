
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface IDealService
    {
        Task<int> AddDealAsync(DealModel model);
        Task<DealModel> GetDealAsync(long id);
        Task<IList<DealModel>> GetDealsAsync(DataRequest<Deal> request);
        Task<IList<DealModel>> GetDealsAsync(int skip, int take, DataRequest<Deal> request);
        Task<int> GetDealCountAsync(DataRequest<Deal> request);
        Task<int> UpdateDealAsync(DealModel model);
        Task<int> DeleteDealAsync(DealModel model);
        Task<ObservableCollection<DealPartiesModel>> GetDealParties(int dealId);
        Task<int> DeleteDealPartiesAsync(int id);
        Task<int> DeleteDealPayScheduleAsync(int id);
    }
}
