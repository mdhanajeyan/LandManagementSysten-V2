using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class DealCollection : VirtualCollection<DealModel>
    {
        private DataRequest<Deal> _dataRequest = null;
        public IDealService DealService { get; }
        public DealCollection(IDealService dealService, ILogService logService) : base(logService)
        {
            DealService = dealService;
        }

        private DealModel _defaultItem = DealModel.CreateEmpty();
        protected override DealModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Deal> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await DealService.GetDealCountAsync(_dataRequest);
            Ranges[0] = await DealService.GetDealsAsync(0, RangeSize, _dataRequest);
        }

        protected override async Task<IList<DealModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await DealService.GetDealsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("DealCollection", "Fetch", ex);
            }
            return null;

        }
    }
}
