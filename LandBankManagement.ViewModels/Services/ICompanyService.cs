using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface ICompanyService
    {
        Task<CompanyModel> AddCompanyAsync(CompanyModel model, ICollection<ImagePickerResult> docs);
        Task<CompanyModel> GetCompanyAsync(long id);
        Task<IList<CompanyModel>> GetCompaniesAsync(DataRequest<Company> request);
        Task<IList<CompanyModel>> GetCompaniesAsync();
        Task<IList<CompanyModel>> GetCompaniesAsync(int skip, int take, DataRequest<Company> request);
        Task<int> GetCompaniesCountAsync(DataRequest<Company> request);
        Task<CompanyModel> UpdateCompanyAsync(CompanyModel model, ICollection<ImagePickerResult> docs);
        Task<Result> DeleteCompanyAsync(CompanyModel model);
        Task<int> DeleteCompanyRangeAsync(int index, int length, DataRequest<Company> request);
        Task<int> UploadCompanyDocumentsAsync(List<ImagePickerResult> model, Guid guid);
        Task<int> DeleteCompanyDocumentAsync(ImagePickerResult documents);
        Task<ObservableCollection<ImagePickerResult>> GetDocuments(Guid guid);
    }
}
