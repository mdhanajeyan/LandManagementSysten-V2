using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class CompanyService : ICompanyService
    {
        public CompanyService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }


        public async Task<int> AddCompanyAsync(CompanyModel model)
        {
            long id = model.CompanyID;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var company = new Company();
                if (company != null)
                {
                    UpdateCompanyFromModel(company, model);
                    company.CompanyGuid = Guid.NewGuid();
                    await dataService.UpdateCompanyAsync(company);
                    model.Merge(await GetCompanyAsync(dataService, company.CompanyID));
                }
                return 0;
            }
        }

        public async Task<CompanyModel> GetCompanyAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetCompanyAsync(dataService, id);
            }
        }
        static private async Task<CompanyModel> GetCompanyAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetCompanyAsync(id);
            if (item != null)
            {
                return await CreateCompanyModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<CompanyModel>> GetCompaniesAsync(DataRequest<Company> request)
        {
            var collection = new CompanyCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<CompanyModel>> GetCompaniesAsync(int skip, int take, DataRequest<Company> request)
        {
            var models = new List<CompanyModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCompaniesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateCompanyModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetCompaniesCountAsync(DataRequest<Company> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetCompaniesCountAsync(request);
            }
        }

        public async Task<int> UpdateCompanyAsync(CompanyModel model)
        {
            long id = model.CompanyID;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var Company = id > 0 ? await dataService.GetCompanyAsync(model.CompanyID) : new Company();
                if (Company != null)
                {
                    UpdateCompanyFromModel(Company, model);
                    await dataService.UpdateCompanyAsync(Company);
                    model.Merge(await GetCompanyAsync(dataService, Company.CompanyID));
                }
                return 0;
            }
        }

        public async Task<int> DeleteCompanyAsync(CompanyModel model)
        {
            var Company = new Company { CompanyID = model.CompanyID };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteCompanyAsync(Company);
            }
        }

        public async Task<int> DeleteCompanyRangeAsync(int index, int length, DataRequest<Company> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCompanyKeysAsync(index, length, request);
                return await dataService.DeleteCompanyAsync(items.ToArray());
            }
        }

        static public async Task<CompanyModel> CreateCompanyModelAsync(Company source, bool includeAllFields)
        {
            var model = new CompanyModel()
            {
                CompanyID = source.CompanyID,
                CompanyGuid = source.CompanyGuid,
                Name = source.Name,
                PhoneNoIsdCode = source.PhoneNoIsdCode,
                PhoneNo = source.PhoneNo,
                Email = source.Email,
                PAN = source.PAN,
                GSTIN = source.GSTIN,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                City = source.City,
                IsActive = source.IsActive,
                Pincode = source.Pincode
            };

            return model;
        }

        private void UpdateCompanyFromModel(Company target, CompanyModel source)
        {
            target.CompanyGuid = source.CompanyGuid;
            target.Name = source.Name;
            target.PhoneNoIsdCode = source.PhoneNoIsdCode;
            target.PhoneNo = source.PhoneNo;
            target.Email = source.Email;
            target.PAN = source.PAN;
            target.GSTIN = source.GSTIN;
            target.AddressLine1 = source.AddressLine1;
            target.AddressLine2 = source.AddressLine2;
            target.City = source.City;
            target.IsActive = source.IsActive;
            target.Pincode = source.Pincode;
        }

    }
}
