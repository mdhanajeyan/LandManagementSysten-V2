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

        Task<int> AddTalukAsync(Taluk model);
        Task<Taluk> GetTalukAsync(long id);
        Task<IList<Taluk>> GetTaluksAsync(DataRequest<Taluk> request);
        Task<IList<Taluk>> GetTaluksAsync(int skip, int take, DataRequest<Taluk> request);
        Task<int> GetTaluksCountAsync(DataRequest<Taluk> request);
        Task<int> UpdateTalukAsync(Taluk model);
        Task<int> DeleteTalukAsync(Taluk model);


        Task<int> AddHobliAsync(Hobli model);
        Task<Hobli> GetHobliAsync(long id);
        Task<IList<Hobli>> GetHoblisAsync(DataRequest<Hobli> request);
        Task<IList<Hobli>> GetHoblisAsync(int skip, int take, DataRequest<Hobli> request);
        Task<int> GetHoblisCountAsync(DataRequest<Hobli> request);
        Task<int> UpdateHobliAsync(Hobli model);
        Task<int> DeleteHobliAsync(Hobli model);


        Task<int> AddVillageAsync(Village model);
        Task<Village> GetVillageAsync(long id);
        Task<IList<Village>> GetVillagesAsync(DataRequest<Village> request);
        Task<IList<Village>> GetVillagesAsync(int skip, int take, DataRequest<Village> request);
        Task<int> GetVillagesCountAsync(DataRequest<Village> request);
        Task<int> UpdateVillageAsync(Village model);
        Task<int> DeleteVillageAsync(Village model);
    }
}
