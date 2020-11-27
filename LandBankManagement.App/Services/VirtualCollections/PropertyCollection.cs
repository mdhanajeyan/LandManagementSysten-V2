using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services.VirtualCollections
{
    public class PropertyCollection : VirtualCollection<PropertyModel>
    {
        private DataRequest<Property> _dataRequest = null;
        public IPropertyService PropertyService { get; }
        public PropertyCollection(IPropertyService propertyService, ILogService logService) : base(logService)
        {
            PropertyService = propertyService;
        }

        private PropertyModel _defaultItem = PropertyModel.CreateEmpty();
        protected override PropertyModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Property> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await PropertyService.GetPropertiesCountAsync(_dataRequest);
                Ranges[0] = await PropertyService.GetPropertiesAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<PropertyModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await PropertyService.GetPropertiesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("PropertyCollection", "Fetch", ex);
            }
            return null;

        }


    }
}
