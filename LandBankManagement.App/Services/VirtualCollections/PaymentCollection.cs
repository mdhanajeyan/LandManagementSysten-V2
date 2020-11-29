using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class PaymentCollection : VirtualCollection<PaymentModel>
    {
        private DataRequest<Payment> _dataRequest = null;
        public IPaymentService PaymentService { get; }
        public PaymentCollection(IPaymentService paymentService, ILogService logService) : base(logService)
        {
            PaymentService = paymentService;
        }

        private PaymentModel _defaultItem = PaymentModel.CreateEmpty();
        protected override PaymentModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Payment> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await PaymentService.GetPaymentsCountAsync(_dataRequest);
            Ranges[0] = await PaymentService.GetPaymentsAsync(0, RangeSize, _dataRequest);
        }

        protected override async Task<IList<PaymentModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await PaymentService.GetPaymentsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("PaymentCollection", "Fetch", ex);
            }
            return null;

        }

    }
}
