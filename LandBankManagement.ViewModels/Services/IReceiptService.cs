using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IReceiptService
    {
        Task<ReceiptModel> AddReceiptAsync(ReceiptModel model);
        Task<ReceiptModel> GetReceiptAsync(long id);
        Task<IList<ReceiptModel>> GetReceiptsAsync(DataRequest<Receipt> request);
        Task<IList<ReceiptModel>> GetReceiptsAsync(int skip, int take, DataRequest<Receipt> request);
        Task<int> GetReceiptsCountAsync(DataRequest<Receipt> request);
        Task<ReceiptModel> UpdateReceiptAsync(ReceiptModel model);
        Task<int> DeleteReceiptAsync(ReceiptModel model);
    }
}
