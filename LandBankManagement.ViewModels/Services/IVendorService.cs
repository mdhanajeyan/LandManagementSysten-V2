using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IVendorService
    {
        Task<VendorModel> AddVendorAsync(VendorModel model, ICollection<ImagePickerResult> docs);
        Task<VendorModel> GetVendorAsync(long id);
        Task<IList<VendorModel>> GetVendorsAsync(DataRequest<Vendor> request);
        Task<IList<VendorModel>> GetVendorsAsync(int skip, int take, DataRequest<Vendor> request);
        Task<int> GetVendorsCountAsync(DataRequest<Vendor> request);
        Task<VendorModel> UpdateVendorAsync(VendorModel model, ICollection<ImagePickerResult> docs);
        Task<int> DeleteVendorAsync(VendorModel model);
        Task<int> DeleteVendorDocumentAsync(ImagePickerResult documents);
        Task<int> UploadVendorDocumentsAsync(List<ImagePickerResult> model, Guid guid);
        Task<ObservableCollection<ImagePickerResult>> GetDocuments(Guid guid);
    }
}
