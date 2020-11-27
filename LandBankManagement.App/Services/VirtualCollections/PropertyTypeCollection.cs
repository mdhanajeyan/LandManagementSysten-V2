using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services.VirtualCollections
{
    public class PropertyTypeCollection : VirtualCollection<PropertyTypeModel>
    {
        private DataRequest<PropertyType> _dataRequest = null;
        public IPropertyTypeService PropertyTypeService { get; }
        public PropertyTypeCollection(IPropertyTypeService propertyTypeService, ILogService logService) : base(logService)
        {
            PropertyTypeService = propertyTypeService;
        }

        private PropertyTypeModel _defaultItem = PropertyTypeModel.CreateEmpty();
        protected override PropertyTypeModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<PropertyType> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await PropertyTypeService.GetPropertyTypesCountAsync(_dataRequest);
                Ranges[0] = await PropertyTypeService.GetPropertyTypesAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<PropertyTypeModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await PropertyTypeService.GetPropertyTypesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("PropertyTypeCollection", "Fetch", ex);
            }
            return null;

        }


    }
}
