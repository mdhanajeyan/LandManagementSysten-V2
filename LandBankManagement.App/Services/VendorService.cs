using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;
namespace LandBankManagement.Services
{
    public class VendorService:IVendorService
    {
        public VendorService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public async Task<VendorModel> GetVendorAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetVendorAsync(dataService, id);
            }
        }
        static private async Task<VendorModel> GetVendorAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetVendorAsync(id);
            if (item != null)
            {
                return await CreateVendorModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<VendorModel>> GetVendorsAsync(DataRequest<Vendor> request)
        {
            var collection = new VendorCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<VendorModel>> GetVendorsAsync(int skip, int take, DataRequest<Vendor> request)
        {
            var models = new List<VendorModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetVendorsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateVendorModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetVendorsCountAsync(DataRequest<Vendor> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetVendorsCountAsync(request);
            }
        }

        public async Task<int> AddVendorAsync(VendorModel model)
        {
            long id = model.VendorId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var vendor =  new Vendor();
                if (vendor != null)
                {
                    UpdateVendorFromModel(vendor, model);
                    vendor.VendorGuid = Guid.NewGuid();
                    await dataService.AddVendorAsync(vendor);
                    model.Merge(await GetVendorAsync(dataService, vendor.VendorId));
                }
                return 0;
            }
        }

        public async Task<int> UpdateVendorAsync(VendorModel model)
        {
            long id = model.VendorId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var vendor = id > 0 ? await dataService.GetVendorAsync(model.VendorId) : new Vendor();
                if (vendor != null)
                {
                    UpdateVendorFromModel(vendor, model);
                    await dataService.UpdateVendorAsync(vendor);
                    model.Merge(await GetVendorAsync(dataService, vendor.VendorId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteVendorAsync(VendorModel model)
        {
            var vendor = new Vendor { VendorId = model.VendorId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteVendorAsync(vendor);
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

        static public async Task<VendorModel> CreateVendorModelAsync(Vendor source, bool includeAllFields)
        {
            var model = new VendorModel()
            {
                VendorId = source.VendorId,
                VendorGuid = source.VendorGuid,
                VendorSalutation = source.VendorSalutation,
                VendorLastName = source.VendorLastName,
                VendorName = source.VendorName,
                VendorAlias = source.VendorAlias,
                RelativeName = source.RelativeName,
                RelativeLastName = source.RelativeLastName,
                RelativeSalutation = source.RelativeSalutation,
                ContactPerson = source.ContactPerson,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                City = source.City,
                PinCode = source.PinCode,
                PhoneNoIsdCode = source.PhoneNoIsdCode,
                PhoneNo = source.PhoneNo,
                email = source.email,
                PAN = source.PAN,
                AadharNo = source.AadharNo,
                GSTIN = source.GSTIN,
                IsVendorActive = source.IsVendorActive
            };

            return model;
        }

        private void UpdateVendorFromModel(Vendor target, VendorModel source)
        {
            target.VendorId = source.VendorId;
            target.VendorGuid = source.VendorGuid;
            target.VendorSalutation = source.VendorSalutation;
            target.VendorLastName = source.VendorLastName;
            target.VendorName = source.VendorName;
            target.VendorAlias = source.VendorAlias;
            target.RelativeName = source.RelativeName;
            target.RelativeLastName = source.RelativeLastName;
            target.RelativeSalutation = source.RelativeSalutation;
            target.ContactPerson = source.ContactPerson;
            target.AddressLine1 = source.AddressLine1;
            target.AddressLine2 = source.AddressLine2;
            target.City = source.City;
            target.PinCode = source.PinCode;
            target.PhoneNoIsdCode = source.PhoneNoIsdCode;
            target.PhoneNo = source.PhoneNo;
            target.email = source.email;
            target.PAN = source.PAN;
            target.AadharNo = source.AadharNo;
            target.GSTIN = source.GSTIN;
            target.IsVendorActive = source.IsVendorActive;

        }
    }
}
