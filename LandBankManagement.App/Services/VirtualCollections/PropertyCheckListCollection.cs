using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services.VirtualCollections
{
    public class PropertyCheckListCollection : VirtualCollection<PropertyCheckListModel>
    {
        private DataRequest<PropertyCheckList> _dataRequest = null;
        public IPropertyCheckListService PropertyService { get; }
        public PropertyCheckListCollection(IPropertyCheckListService propertyService, ILogService logService) : base(logService)
        {
            PropertyService = propertyService;
        }

        private PropertyCheckListModel _defaultItem = PropertyCheckListModel.CreateEmpty();
        protected override PropertyCheckListModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<PropertyCheckList> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await PropertyService.GetPropertyCheckListCountAsync(_dataRequest);
                Ranges[0] = await PropertyService.GetPropertyCheckListAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<PropertyCheckListModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await PropertyService.GetPropertyCheckListAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("PropertyCollection", "Fetch", ex);
            }
            return null;

        }


    }
}
