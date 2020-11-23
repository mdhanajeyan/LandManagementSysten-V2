using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LandBankManagement.Data.Services
{
    public interface IDataService : IDisposable
    {

        Task<Company> GetCompanyAsync(long id);
        Task<IList<Company>> GetCompaniesAsync(int skip, int take, DataRequest<Company> request);
        Task<IList<Company>> GetCompanyKeysAsync(int skip, int take, DataRequest<Company> request);
        Task<int> GetCompaniesCountAsync(DataRequest<Company> request);
        Task<int> UpdateCompanyAsync(Company company);
        Task<int> DeleteCompanyAsync(params Company[] company);
    }
}
