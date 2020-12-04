using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface IPropertyTypeService
    {
        Task<PropertyTypeModel> AddPropertyTypeAsync(PropertyTypeModel model);
        Task<PropertyTypeModel> GetPropertyTypeAsync(long id);
        Task<IList<PropertyTypeModel>> GetPropertyTypesAsync(DataRequest<PropertyType> request);
        Task<IList<PropertyTypeModel>> GetPropertyTypesAsync(int skip, int take, DataRequest<PropertyType> request);
        Task<int> GetPropertyTypesCountAsync(DataRequest<PropertyType> request);
        Task<PropertyTypeModel> UpdatePropertyTypeAsync(PropertyTypeModel model);
        Task<int> DeletePropertyTypeAsync(PropertyTypeModel model);
    }
}
