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

        Task<int> AddVendorAsync(Vendor model);
        Task<Vendor> GetVendorAsync(long id);
        Task<IList<Vendor>> GetVendorsAsync(DataRequest<Vendor> request);
        Task<IList<Vendor>> GetVendorsAsync(int skip, int take, DataRequest<Vendor> request);
        Task<int> GetVendorsCountAsync(DataRequest<Vendor> request);
        Task<int> UpdateVendorAsync(Vendor model);
        Task<int> DeleteVendorAsync(Vendor model);

        Task<int> AddPartyAsync(Party model);
        Task<Party> GetPartyAsync(long id);
        Task<IList<Party>> GetPartiesAsync(DataRequest<Party> request);
        Task<IList<Party>> GetPartiesAsync(int skip, int take, DataRequest<Party> request);
        Task<int> GetPartiesCountAsync(DataRequest<Party> request);
        Task<int> UpdatePartyAsync(Party model);
        Task<int> DeletePartyAsync(Party model);
    }
}
