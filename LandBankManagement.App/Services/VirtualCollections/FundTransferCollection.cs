using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;


namespace LandBankManagement.Services
{
    public class FundTransferCollection : VirtualCollection<FundTransferModel>
    {
        private DataRequest<FundTransfer> _dataRequest = null;
        public IFundTransferService FundTransferService { get; }

       

        public FundTransferCollection(IFundTransferService fundTransfer, ILogService logService) : base(logService)
        {
            FundTransferService = fundTransfer;
        }

        private FundTransferModel _defaultItem = FundTransferModel.CreateEmpty();
        protected override FundTransferModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<FundTransfer> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await FundTransferService.GetFundTransfersCountAsync(_dataRequest);
                Ranges[0] = await FundTransferService.GetFundTransfersAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<FundTransferModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await FundTransferService.GetFundTransfersAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("FundTransferCollection", "Fetch", ex);
            }
            return null;
        }
    }
}
