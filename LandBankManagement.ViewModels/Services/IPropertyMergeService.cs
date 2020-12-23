using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IPropertyMergeService
    {

        Task<int> AddPropertyMergeAsync(PropertyMergeModel model);
        Task<PropertyMergeModel> GetPropertyMergeAsync(long id);
        Task<IList<PropertyMergeModel>> GetPropertyMergeAsync(DataRequest<PropertyMerge> request);
        Task<IList<PropertyMergeModel>> GetPropertyMergeAsync(int skip, int take, DataRequest<PropertyMerge> request);
        Task<int> GetPropertyMergeCountAsync(DataRequest<PropertyMerge> request);
        Task<int> UpdatePropertyMergeAsync(PropertyMergeModel model);
        Task<int> DeletePropertyMergeAsync(PropertyMergeModel model);
        Task<int> DeletePropertyMergeItemAsync(int id);
        Task<PropertyMergeListModel> GetPropertyListItemForProeprty(int id);
    }
}
