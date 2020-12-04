using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IFundTransferService
    {
        Task<FundTransferModel> AddFundTransferAsync(FundTransferModel model);
        Task<FundTransferModel> GetFundTransferAsync(long id);
        Task<IList<FundTransferModel>> GetFundTransfersAsync(DataRequest<FundTransfer> request);
        Task<IList<FundTransferModel>> GetFundTransfersAsync(int skip, int take, DataRequest<FundTransfer> request);
        Task<int> GetFundTransfersCountAsync(DataRequest<FundTransfer> request);
        Task<FundTransferModel> UpdateFundTransferAsync(FundTransferModel model);
        Task<int> DeleteFundTransferAsync(FundTransferModel model);
    }
}
