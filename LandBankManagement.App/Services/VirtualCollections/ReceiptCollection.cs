using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class ReceiptCollection : VirtualCollection<ReceiptModel>
    {
        private DataRequest<Receipt> _dataRequest = null;
        public IReceiptService ReceiptService { get; }
        public ReceiptCollection(IReceiptService vendorService, ILogService logService) : base(logService)
        {
            ReceiptService = vendorService;
        }

        private ReceiptModel _defaultItem = ReceiptModel.CreateEmpty();
        protected override ReceiptModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Receipt> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await ReceiptService.GetReceiptsCountAsync(_dataRequest);
            Ranges[0] = await ReceiptService.GetReceiptsAsync(0, RangeSize, _dataRequest);
        }

        protected override async Task<IList<ReceiptModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await ReceiptService.GetReceiptsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("ReceiptCollection", "Fetch", ex);
            }
            return null;

        }

    }
}
