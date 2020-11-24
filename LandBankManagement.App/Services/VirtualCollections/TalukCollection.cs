using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class TalukCollection : VirtualCollection<TalukModel>
    {
        private DataRequest<Taluk> _dataRequest = null;
        public ITalukService TalukService { get; }
        public TalukCollection(ITalukService talukService, ILogService logService) : base(logService)
        {
            TalukService = talukService;
        }

        private TalukModel _defaultItem = TalukModel.CreateEmpty();
        protected override TalukModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Taluk> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await TalukService.GetTaluksCountAsync(_dataRequest);
                Ranges[0] = await TalukService.GetTaluksAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw;
            }
        }

        protected override async Task<IList<TalukModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await TalukService.GetTaluksAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("TalukCollection", "Fetch", ex);
            }
            return null;

        }
    }
}
