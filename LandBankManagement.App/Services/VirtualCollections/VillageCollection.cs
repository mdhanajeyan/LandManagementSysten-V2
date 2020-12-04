using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class VillageCollection : VirtualCollection<VillageModel>
    {
        private DataRequest<Village> _dataRequest = null;
        public IVillageService VillageService { get; }
        public VillageCollection(IVillageService villageService, ILogService logService) : base(logService)
        {
            VillageService = villageService;
        }

        private VillageModel _defaultItem = VillageModel.CreateEmpty();
        protected override VillageModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Village> dataRequest)
        {

            _dataRequest = dataRequest;
            Count = await VillageService.GetVillagesCountAsync(_dataRequest);
            Ranges[0] = await VillageService.GetVillagesAsync(0, RangeSize, _dataRequest);

        }

        protected override async Task<IList<VillageModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await VillageService.GetVillagesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("VillageCollection", "Fetch", ex);
            }
            return null;

        }

    }
}
