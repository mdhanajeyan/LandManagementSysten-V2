using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class PartyCollection : VirtualCollection<PartyModel>
    {
        private DataRequest<Party> _dataRequest = null;
        public IPartyService PartyService { get; }
        public PartyCollection(IPartyService partyService, ILogService logService) : base(logService)
        {
            PartyService = partyService;
        }

        private PartyModel _defaultItem = PartyModel.CreateEmpty();
        protected override PartyModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Party> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await PartyService.GetPartiesCountAsync(_dataRequest);
                Ranges[0] = await PartyService.GetPartiesAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<PartyModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await PartyService.GetPartiesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("PartyCollection", "Fetch", ex);
            }
            return null;

        }

    }
}
