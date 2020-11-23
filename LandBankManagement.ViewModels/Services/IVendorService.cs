using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;

using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IVendorService
    {
        Task<int> AddVendorAsync(VendorModel model);
        Task<VendorModel> GetVendorAsync(long id);
        Task<IList<VendorModel>> GetVendorsAsync(DataRequest<Vendor> request);
        Task<IList<VendorModel>> GetVendorsAsync(int skip, int take, DataRequest<Vendor> request);
        Task<int> GetVendorsCountAsync(DataRequest<Vendor> request);

        Task<int> UpdateVendorAsync(VendorModel model);

        Task<int> DeleteVendorAsync(VendorModel model);
      //  Task<int> DeleteCompanyRangeAsync(int index, int length, DataRequest<Vendor> request);
    }
}
