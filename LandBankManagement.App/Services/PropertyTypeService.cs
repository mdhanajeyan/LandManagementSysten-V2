
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;
using LandBankManagement.Services.VirtualCollections;

namespace LandBankManagement.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public PropertyTypeService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<PropertyTypeModel> AddPropertyTypeAsync(PropertyTypeModel model)
        {
            long id = model.PropertyTypeId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var propertyType = new PropertyType();
                if (propertyType != null)
                {
                    UpdatePropertyTypeFromModel(propertyType, model);
                    propertyType.PropertyTypeGuid = Guid.NewGuid();
                    await dataService.AddPropertyTypeAsync(propertyType);
                    model.Merge(await GetPropertyTypeAsync(dataService, propertyType.PropertyTypeId));
                }
                return model;
            }
        }

        static private async Task<PropertyTypeModel> GetPropertyTypeAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetPropertyTypeAsync(id);
            if (item != null)
            {
                return await CreatePropertyTypeModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<PropertyTypeModel> GetPropertyTypeAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetPropertyTypeAsync(dataService, id);
            }
        }

        public async Task<IList<PropertyTypeModel>> GetPropertyTypesAsync(DataRequest<PropertyType> request)
        {
            var collection = new PropertyTypeCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<PropertyTypeModel>> GetPropertyTypesAsync(int skip, int take, DataRequest<PropertyType> request)
        {
            var models = new List<PropertyTypeModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPropertyTypesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreatePropertyTypeModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetPropertyTypesCountAsync(DataRequest<PropertyType> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetPropertyTypesCountAsync(request);
            }
        }

        public async Task<PropertyTypeModel> UpdatePropertyTypeAsync(PropertyTypeModel model)
        {
            long id = model.PropertyTypeId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var propertyType = id > 0 ? await dataService.GetPropertyTypeAsync(model.PropertyTypeId) : new PropertyType();
                if (propertyType != null)
                {
                    UpdatePropertyTypeFromModel(propertyType, model);
                    await dataService.UpdatePropertyTypeAsync(propertyType);
                    model.Merge(await GetPropertyTypeAsync(dataService, propertyType.PropertyTypeId));
                }
                return model;
            }
        }

        public async Task<int> DeletePropertyTypeAsync(PropertyTypeModel model)
        {
            var propertyType = new PropertyType { PropertyTypeId = model.PropertyTypeId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePropertyTypeAsync(propertyType);
            }
        }

        static public async Task<PropertyTypeModel> CreatePropertyTypeModelAsync(PropertyType source, bool includeAllFields)
        {
            var model = new PropertyTypeModel()
            {
               
                PropertyTypeId = source.PropertyTypeId,
                PropertyTypeGuid = source.PropertyTypeGuid,
                PropertyTypeText = source.PropertyTypeText,
                PropertyTypeIsActive = source.PropertyTypeIsActive,
        };
            return model;
        }

        private void UpdatePropertyTypeFromModel(PropertyType target, PropertyTypeModel source)
        {
            target.PropertyTypeId = source.PropertyTypeId;
            target.PropertyTypeGuid = source.PropertyTypeGuid;
            target.PropertyTypeText = source.PropertyTypeText;
            target.PropertyTypeIsActive = source.PropertyTypeIsActive;
        }
    }
}
