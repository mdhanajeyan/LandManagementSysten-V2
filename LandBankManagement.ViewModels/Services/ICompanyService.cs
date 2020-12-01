using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface ICompanyService
    {
        Task<int> AddCompanyAsync(CompanyModel model, ICollection<ImagePickerResult> docs);
        Task<CompanyModel> GetCompanyAsync(long id);
        Task<IList<CompanyModel>> GetCompaniesAsync(DataRequest<Company> request);
        Task<IList<CompanyModel>> GetCompaniesAsync();
        Task<IList<CompanyModel>> GetCompaniesAsync(int skip, int take, DataRequest<Company> request);
        Task<int> GetCompaniesCountAsync(DataRequest<Company> request);
        Task<int> UpdateCompanyAsync(CompanyModel model, ICollection<ImagePickerResult> docs);
        Task<int> DeleteCompanyAsync(CompanyModel model);
        Task<int> DeleteCompanyRangeAsync(int index, int length, DataRequest<Company> request);
        Task<int> UploadCompanyDocumentsAsync(List<ImagePickerResult> model);
        Task<int> DeleteCompanyDocumentAsync(ImagePickerResult documents);
    }
}
