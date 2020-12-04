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
            _dataRequest = dataRequest;
            Count = await TalukService.GetTaluksCountAsync(_dataRequest);
        }

        protected override async Task<IList<TalukModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            return await TalukService.GetTaluksAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
        }
    }
}
