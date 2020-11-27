
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface IPropertyService
    {
        Task<int> AddPropertyAsync(PropertyModel model);
        Task<PropertyModel> GetPropertyAsync(long id);
        Task<IList<PropertyModel>> GetPropertiesAsync(DataRequest<Property> request);
        Task<IList<PropertyModel>> GetPropertiesAsync(int skip, int take, DataRequest<Property> request);
        Task<int> GetPropertiesCountAsync(DataRequest<Property> request);
        Task<int> UpdatePropertyAsync(PropertyModel model);
        Task<int> DeletePropertyAsync(PropertyModel model);
    }
}
