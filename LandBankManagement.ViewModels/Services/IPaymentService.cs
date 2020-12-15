using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface IPaymentService
    {
        Task<int> AddPaymentAsync(PaymentModel model);
        Task<PaymentModel> GetPaymentAsync(long id);
        Task<IList<PaymentModel>> GetPaymentsAsync(DataRequest<Payment> request);
        Task<IList<PaymentModel>> GetPaymentsAsync(int skip, int take, DataRequest<Payment> request);
        Task<int> GetPaymentsCountAsync(DataRequest<Payment> request);
        Task<int> UpdatePaymentAsync(PaymentModel model);
        Task<int> DeletePaymentAsync(PaymentModel model);
        Task<int> DeletePaymentListAsync(int id);
    }
}
