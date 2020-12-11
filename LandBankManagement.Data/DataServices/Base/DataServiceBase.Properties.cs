using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPropertyAsync(Property model)
        {
            if (model == null)
                return 0;
            try
            {
                ICollection<PropertyDocuments> docs = model.PropertyDocuments;
                var entity = new Property()
                {
                    PropertyGuid = model.PropertyGuid,
                    PropertyName = model.PropertyName,
                    PartyId = model.PartyId,
                    TalukId = model.TalukId,
                    HobliId = model.HobliId,
                    VillageId = model.VillageId,
                    DocumentTypeId = model.DocumentTypeId,
                    DateOfExecution = model.DateOfExecution,
                    DocumentNo = model.DocumentNo,
                    PropertyTypeId = model.PropertyTypeId,
                    SurveyNo = model.SurveyNo,
                    PropertyGMapLink = model.PropertyGMapLink,
                    LandAreaInputAcres = model.LandAreaInputAcres,
                    LandAreaInputGuntas = model.LandAreaInputGuntas,
                    LandAreaInAcres = model.LandAreaInAcres,
                    LandAreaInGuntas = model.LandAreaInGuntas,
                    LandAreaInSqMts = model.LandAreaInSqMts,
                    LandAreaInSqft = model.LandAreaInSqft,
                    AKarabAreaInputAcres = model.AKarabAreaInputAcres,
                    AKarabAreaInputGuntas = model.AKarabAreaInputGuntas,
                    AKarabAreaInAcres = model.AKarabAreaInAcres,
                    AKarabAreaInGuntas = model.AKarabAreaInGuntas,
                    AKarabAreaInSqMts = model.AKarabAreaInSqMts,
                    AKarabAreaInSqft = model.AKarabAreaInSqft,
                    BKarabAreaInputAcres = model.BKarabAreaInputAcres,
                    BKarabAreaInputGuntas = model.BKarabAreaInputGuntas,
                    BKarabAreaInAcres = model.BKarabAreaInAcres,
                    BKarabAreaInGuntas = model.BKarabAreaInGuntas,
                    BKarabAreaInSqMts = model.BKarabAreaInSqMts,
                    BKarabAreaInSqft = model.BKarabAreaInSqft,
                    SaleValue1 = model.SaleValue1,
                    SaleValue2 = model.SaleValue2,
                    CompanyID = model.CompanyID
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                await _dataSource.SaveChangesAsync();
                int res = entity.PropertyId;


                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (doc.PropertyBlobId == 0)
                        {
                            doc.PropertyGuid = model.PropertyGuid;
                            _dataSource.PropertyDocuments.Add(doc);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();

                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<Property> GetPropertyAsync(long id)
        {
            var property= await _dataSource.Properties.Where(r => r.PropertyId == id).FirstOrDefaultAsync();

            if (property.PropertyGuid != null)
            {
                var docs = GetPropertyDocumentsAsync(property.PropertyGuid);
                if (docs.Any())
                {
                    property.PropertyDocuments = docs;
                }

            }
            return property;
        }

        private List<PropertyDocuments> GetPropertyDocumentsAsync(Guid id)
        {
            try
            {
                return _dataSource.PropertyDocuments
                    .Where(r => r.PropertyGuid == id).ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<IList<Property>> GetPropertiesAsync(DataRequest<Property> request)
        {
            IQueryable<Property> items = GetProperties(request);
            return await items.ToListAsync();
        }

        private IQueryable<Property> GetProperties(DataRequest<Property> request)
        {
            IQueryable<Property> items = _dataSource.Properties;

            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            }

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            // Order By
            if (request.OrderBy != null)
            {
                items = items.OrderBy(request.OrderBy);
            }
            if (request.OrderByDesc != null)
            {
                items = items.OrderByDescending(request.OrderByDesc);
            }

            return items;
        }

        public async Task<IList<Property>> GetPropertiesAsync(int skip, int take, DataRequest<Property> request)
        {
            IQueryable<Property> items = GetProperties(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Property
                {
                    PropertyId = source.PropertyId,
                    PropertyGuid = source.PropertyGuid,
                    PropertyName = source.PropertyName,
                    PartyId = source.PartyId,
                    TalukId = source.TalukId,
                    HobliId = source.HobliId,
                    VillageId = source.VillageId,
                    DocumentTypeId = source.DocumentTypeId,
                    DateOfExecution = source.DateOfExecution,
                    DocumentNo = source.DocumentNo,
                    PropertyTypeId = source.PropertyTypeId,
                    SurveyNo = source.SurveyNo,
                    PropertyGMapLink = source.PropertyGMapLink,
                    LandAreaInputAcres = source.LandAreaInputAcres,
                    LandAreaInputGuntas = source.LandAreaInputGuntas,
                    LandAreaInAcres = source.LandAreaInAcres,
                    LandAreaInGuntas = source.LandAreaInGuntas,
                    LandAreaInSqMts = source.LandAreaInSqMts,
                    LandAreaInSqft = source.LandAreaInSqft,
                    AKarabAreaInputAcres = source.AKarabAreaInputAcres,
                    AKarabAreaInputGuntas = source.AKarabAreaInputGuntas,
                    AKarabAreaInAcres = source.AKarabAreaInAcres,
                    AKarabAreaInGuntas = source.AKarabAreaInGuntas,
                    AKarabAreaInSqMts = source.AKarabAreaInSqMts,
                    AKarabAreaInSqft = source.AKarabAreaInSqft,
                    BKarabAreaInputAcres = source.BKarabAreaInputAcres,
                    BKarabAreaInputGuntas = source.BKarabAreaInputGuntas,
                    BKarabAreaInAcres = source.BKarabAreaInAcres,
                    BKarabAreaInGuntas = source.BKarabAreaInGuntas,
                    BKarabAreaInSqMts = source.BKarabAreaInSqMts,
                    BKarabAreaInSqft = source.BKarabAreaInSqft,
                    SaleValue1 = source.SaleValue1,
                    SaleValue2 = source.SaleValue2
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetPropertiesCountAsync(DataRequest<Property> request)
        {
            IQueryable<Property> items = _dataSource.Properties;

            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            }

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            return await items.CountAsync();
        }

        public async Task<int> UpdatePropertyAsync(Property model)
        {
            try
            {
                ICollection<PropertyDocuments> docs = model.PropertyDocuments;
                _dataSource.Entry(model).State = EntityState.Modified;
                int res = await _dataSource.SaveChangesAsync();
                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (doc.PropertyBlobId == 0)
                        {
                            doc.PropertyGuid = model.PropertyGuid;
                            _dataSource.PropertyDocuments.Add(doc);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> DeletePropertyAsync(Property model)
        {
            _dataSource.Properties.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> AddPropertyParty(List<PropertyParty> propertyParties) {
            if (propertyParties == null)
                return 0;
            foreach (var model in propertyParties) {
                _dataSource.Entry(model).State = EntityState.Added;
            }
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<List<PropertyParty>> GetPartiesOfProperty(int propertyId) {
            try
            {
                var model = await (from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == propertyId)
                                   from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId)
                                   select new PropertyParty
                                   {
                                       PropertyPartyId = pp.PropertyPartyId,
                                       PartyId = pp.PartyId,
                                       PropertyGuid = pp.PropertyGuid,
                                       PropertyId = pp.PropertyId,
                                       PartyName = party.PartyFirstName
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> DeletePropertyPartyAsync(PropertyParty model)
        {
            _dataSource.PropertyParty.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> AddPropPaySchedule(List<PropPaySchedule> schedules,decimal Sale1,decimal Sale2)
        {
            if (schedules == null)
                return 0;
            try
            {
                var property = _dataSource.Properties.Where(x => x.PropertyId == schedules[0].PropertyId).FirstOrDefault();
                if (property != null) {
                    property.SaleValue1 = Sale1;
                    property.SaleValue2 = Sale2;
                    _dataSource.Entry(property).State = EntityState.Modified;
                    await _dataSource.SaveChangesAsync();
                }

                foreach (var model in schedules)
                {
                    _dataSource.Entry(model).State = EntityState.Added;
                }
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<PropertyCostDetails> GetPropertyCostDetails(int propertyId)
        {
            try
            {
                var model = (from prop in _dataSource.Properties.Where(x => x.PropertyId == propertyId)
                             from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == propertyId).DefaultIfEmpty()
                             from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId).DefaultIfEmpty()
                             from com in _dataSource.Companies.Where(x => x.CompanyID == prop.CompanyID).DefaultIfEmpty()
                             from pt in _dataSource.PropertyTypes.Where(x => x.PropertyTypeId == prop.PropertyTypeId).DefaultIfEmpty()
                             from dt in _dataSource.DocumentTypes.Where(x => x.DocumentTypeId == prop.DocumentTypeId).DefaultIfEmpty()
                             from t in _dataSource.Taluks.Where(x => x.TalukId == prop.TalukId).DefaultIfEmpty()
                             from h in _dataSource.Hoblis.Where(x => x.HobliId == prop.HobliId).DefaultIfEmpty()
                             from v in _dataSource.Villages.Where(x => x.VillageId == prop.VillageId).DefaultIfEmpty()
                             select new PropertyCostDetails
                             {
                                 PropertyId = prop.PropertyId,
                                 PropertyName = prop.PropertyName,
                                 ComapnyName = com.Name,
                                 PropertyType = pt.PropertyTypeText,
                                 DocumentType = dt.DocumentTypeName,
                                 Taluk = t.TalukName,
                                 Hobli = h.HobliName,
                                 Village = v.VillageName,
                                 SurveyNo = prop.SurveyNo,
                                 SaleValue1 = prop.SaleValue1,
                                 SaleValue2 = prop.SaleValue2,

                             }).FirstOrDefault();

                model.PropertyParties = await (from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == propertyId)
                                               from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId)
                                               select new PropertyParty
                                               {
                                                   PropertyPartyId = pp.PropertyPartyId,
                                                   PartyId = pp.PartyId,
                                                   PropertyGuid = pp.PropertyGuid,
                                                   PropertyId = pp.PropertyId,
                                                   PartyName = party.PartyFirstName
                                               }).ToListAsync();
                model.PropPaySchedules = await _dataSource.PropPaySchedules.Where(p => p.PropertyId == propertyId).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UploadPropertyDocumentsAsync(List<PropertyDocuments> documents)
        {
            try
            {
                foreach (var doc in documents)
                {
                    _dataSource.Entry(doc).State = EntityState.Added;
                }
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeletePropertyDocumentAsync(PropertyDocuments documents)
        {
            _dataSource.PropertyDocuments.Remove(documents);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
