using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;


namespace LandBankManagement.Services
{
    public class HobliCollection : VirtualCollection<HobliModel>
    {
        private DataRequest<Hobli> _dataRequest = null;
        public IHobliService HobliService { get; }

       

        public HobliCollection(IHobliService hobliService, ILogService logService) : base(logService)
        {
            HobliService = hobliService;
        }

        private HobliModel _defaultItem = HobliModel.CreateEmpty();
        protected override HobliModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Hobli> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await HobliService.GetHoblisCountAsync(_dataRequest);
                Ranges[0] = await HobliService.GetHoblisAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<HobliModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await HobliService.GetHoblisAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("HobliCollection", "Fetch", ex);
            }
            return null;
        }
    }
}
