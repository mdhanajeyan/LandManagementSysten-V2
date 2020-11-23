using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class VendorCollection : VirtualCollection<VendorModel>
    {
        private DataRequest<Vendor> _dataRequest = null;
        public IVendorService VendorService { get; }
        public VendorCollection(IVendorService vendorService, ILogService logService) : base(logService)
        {
            VendorService = vendorService;
        }

        private VendorModel _defaultItem = VendorModel.CreateEmpty();
        protected override VendorModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Vendor> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await VendorService.GetVendorsCountAsync(_dataRequest);
                Ranges[0] = await VendorService.GetVendorsAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<VendorModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await VendorService.GetVendorsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("VendorCollection", "Fetch", ex);
            }
            return null;

        }
    }
}
