using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class PartyService:IPartyService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public PartyService(IDataServiceFactory dataServiceFactory, ILogService logService) {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddPartyAsync(PartyModel model)
        {
            long id = model.PartyId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var party = new Party();
                if (party != null)
                {
                    UpdatePartyFromModel(party, model);
                    party.PartyGuid = Guid.NewGuid();
                    await dataService.AddPartyAsync(party);
                    model.Merge(await GetPartyAsync(dataService, party.PartyId));
                }
                return 0;
            }
        }

        public async Task<PartyModel> GetPartyAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetPartyAsync(dataService, id);
            }
        }
        static private async Task<PartyModel> GetPartyAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetPartyAsync(id);
            if (item != null)
            {
                return await CreatePartyModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<PartyModel>> GetPartiesAsync(DataRequest<Party> request)
        {
            var collection = new PartyCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<PartyModel>> GetPartiesAsync(int skip, int take, DataRequest<Party> request)
        {
            var models = new List<PartyModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPartiesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreatePartyModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetPartiesCountAsync(DataRequest<Party> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetPartiesCountAsync(request);
            }
        }

        public async Task<int> UpdatePartyAsync(PartyModel model)
        {
            long id = model.PartyId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var party = id > 0 ? await dataService.GetPartyAsync(model.PartyId) : new Party();
                if (party != null)
                {
                    UpdatePartyFromModel(party, model);
                    await dataService.UpdatePartyAsync(party);
                    model.Merge(await GetPartyAsync(dataService, party.PartyId));
                }
                return 0;
            }
        }

        public async Task<int> DeletePartyAsync(PartyModel model)
        {
            var party = new Party { PartyId = model.PartyId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePartyAsync(party);
            }
        }

        //public async Task<int> DeletepartyRangeAsync(int index, int length, DataRequest<Party> request)
        //{
        //    using (var dataService = DataServiceFactory.CreateDataService())
        //    {
        //        var items = await dataService.GetCompanyKeysAsync(index, length, request);
        //        return await dataService.DeleteCompanyAsync(items.ToArray());
        //    }
        //}

        static public async Task<PartyModel> CreatePartyModelAsync(Party source, bool includeAllFields)
        {
            var model = new PartyModel()
            {
                PartyId = source.PartyId,
                PartyName = source.PartyFirstName,
                PartyGuid = source.PartyGuid,
                PartyAlias = source.PartyAlias,
                PartySalutation = source.PartySalutation,
                AadharNo = source.AadharNo,
                ContactPerson = source.ContactPerson,
                PAN = source.PAN,
                GSTIN = source.GSTIN,
                email = source.email,
                IsPartyActive = source.IsPartyActive,
                PhoneNo = source.PhoneNo,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                City = source.City,
                PinCode = source.PinCode
            };

            return model;
        }

        private void UpdatePartyFromModel(Party target, PartyModel source)
        {
            target.PartyId = source.PartyId;
            target.PartyFirstName = source.PartyName;
            target.PartyGuid = source.PartyGuid;
            target.PartyAlias = source.PartyAlias;
            target.PartySalutation = source.PartySalutation;
            target.AadharNo = source.AadharNo;
            target.ContactPerson = source.ContactPerson;
            target.PAN = source.PAN.Trim();
            target.GSTIN = source.GSTIN;
            target.email = source.email;
            target.IsPartyActive = source.IsPartyActive;
            target.PhoneNo = source.PhoneNo.Trim();
            target.AddressLine1 = source.AddressLine1;
            target.AddressLine2 = source.AddressLine2;
            target.City = source.City;
            target.PinCode = source.PinCode.Trim();
        }
    }
}
