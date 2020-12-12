
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface IPropertyService
    {
        Task<PropertyModel> AddPropertyAsync(PropertyModel model,ICollection<ImagePickerResult> docs);
        Task<PropertyModel> GetPropertyAsync(long id);
        Task<IList<PropertyModel>> GetPropertiesAsync(DataRequest<Property> request);
        Task<IList<PropertyModel>> GetPropertiesAsync(int skip, int take, DataRequest<Property> request);
        Task<int> GetPropertiesCountAsync(DataRequest<Property> request);
        Task<PropertyModel> UpdatePropertyAsync(PropertyModel model, ICollection<ImagePickerResult> docs);
        Task<int> DeletePropertyAsync(PropertyModel model);
        Task<int> AddPropertyPartyAsync(List<PropertyPartyModel> propertyParties);
        Task<ObservableCollection<PropertyPartyModel>> GetPartiesOfProperty(int propertyId);
        Task<int> DeletePropertyPartyAsync(PropertyPartyModel model);
        Task<PropertyCostDetailsModel> GetPropertyCostDetails(int propertyId);
        Task<int> AddPropPaySchedule(List<PaymentScheduleModel> schedules, decimal Sale1, decimal Sale2);
        Task<int> DeletePropertyDocumentAsync(ImagePickerResult documents);
        Task<ObservableCollection<PropertyModel>> GetPropertyByGroupGuidAsync(Guid guid);
    }
}
