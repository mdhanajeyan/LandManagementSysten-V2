using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class PropertyMergeCollection : VirtualCollection<PropertyMergeModel>
    {
        private DataRequest<PropertyMerge> _dataRequest = null;
        public IPropertyMergeService PropertyMergeService { get; }
        public PropertyMergeCollection(IPropertyMergeService propertyMergeService, ILogService logService) : base(logService)
        {
            PropertyMergeService = propertyMergeService;
        }

        private PropertyMergeModel _defaultItem = PropertyMergeModel.CreateEmpty();
        protected override PropertyMergeModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<PropertyMerge> dataRequest)
        {
            _dataRequest = dataRequest;
            Count = await PropertyMergeService.GetPropertyMergeCountAsync(_dataRequest);
            Ranges[0] = await PropertyMergeService.GetPropertyMergeAsync(0, RangeSize, _dataRequest);
        }

        protected override async Task<IList<PropertyMergeModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await PropertyMergeService.GetPropertyMergeAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("PaymentCollection", "Fetch", ex);
            }
            return null;

        }
    }
}
