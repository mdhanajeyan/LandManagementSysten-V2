using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
   partial class DataServiceBase
    {
        public async Task<int> AddPropertyCheckListAsync(PropertyCheckList model)
        {
            if (model == null)
                return 0;
            try
            {
                ICollection<PropertyCheckListDocuments> docs = model.PropertyCheckListDocuments;
                ICollection<PropertyCheckListVendor> vendors = model.PropertyCheckListVendors;
                ICollection<CheckListOfProperty> checklist = model.CheckListOfProperties;

                var entity = new PropertyCheckList()
                {
                    PropertyGuid = model.PropertyGuid,
                    PropertyName = model.PropertyName,
                    TalukId = model.TalukId,
                    HobliId = model.HobliId,
                    VillageId = model.VillageId,
                    DocumentTypeId = model.DocumentTypeId,
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
                    CompanyID = model.CompanyID,
                    CheckListMaster=model.CheckListMaster,
                    PropertyDescription=model.PropertyDescription
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                await _dataSource.SaveChangesAsync();
                int res = entity.PropertyCheckListId;


                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (doc.PropertyCheckListBlobId == 0)
                        {
                            doc.PropertyGuid = model.PropertyGuid;
                            _dataSource.PropertyCheckListDocuments.Add(doc);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();
                if (vendors != null) {
                    foreach (var vendor in vendors)
                    {
                        if (vendor.CehckListVendorId == 0)
                        {
                            _dataSource.PropertyCheckListVendor.Add(vendor);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();

                if (checklist != null)
                {
                    foreach (var check in checklist)
                    {
                        if (check.CheckListPropertyId == 0)
                        {
                            check.PropertyCheckListId = res;
                            _dataSource.CheckListOfProperty.Add(check);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PropertyCheckList> GetPropertyCheckListAsync(long id)
        {
            var property = await _dataSource.PropertyCheckList.Where(r => r.PropertyCheckListId == id).FirstOrDefaultAsync();

            if (property.PropertyGuid != null)
            {
                var docs = GetPropertyCheckListDocumentsAsync(property.PropertyGuid);
                if (docs.Any())
                {
                    property.PropertyCheckListDocuments = docs;
                }
                var vendors = GetPropertyCheckListVendor(property.PropertyCheckListId);
                if (vendors.Any()) {
                    property.PropertyCheckListVendors = vendors;
                }
                var checklist= GetCheckListOfProperty(property.PropertyCheckListId);
                if (checklist.Any()) {
                    property.CheckListOfProperties = checklist;
                }
            }
            return property;
        }
                

        private IList<PropertyCheckListDocuments> GetPropertyCheckListDocumentsAsync(Guid id)
        {
            try
            {
                return _dataSource.PropertyCheckListDocuments
                    .Where(r => r.PropertyGuid == id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<PropertyCheckListVendor> GetPropertyCheckListVendor(int id)
        {
            try
            {
                return _dataSource.PropertyCheckListVendor
                    .Where(r => r.PropertyCheckListId == id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CheckListOfProperty> GetCheckListOfProperty(int id)
        {
            try
            {
                return _dataSource.CheckListOfProperty
                    .Where(r => r.PropertyCheckListId == id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<PropertyCheckList>> GetPropertyCheckListAsync(DataRequest<PropertyCheckList> request)
        {
            IQueryable<PropertyCheckList> items = GetPropertyCheckList(request);
            return await items.ToListAsync();
        }

        private IQueryable<PropertyCheckList> GetPropertyCheckList(DataRequest<PropertyCheckList> request)
        {
            IQueryable<PropertyCheckList> items = _dataSource.PropertyCheckList;

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

        public async Task<IList<PropertyCheckList>> GetPropertyCheckListAsync(int skip, int take, DataRequest<PropertyCheckList> request)
        {
            IQueryable<PropertyCheckList> items = GetPropertyCheckList(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new PropertyCheckList
                {                  
                    PropertyGuid = source.PropertyGuid,
                    PropertyName = source.PropertyName,                   
                    TalukId = source.TalukId,
                    HobliId = source.HobliId,
                    VillageId = source.VillageId,
                    DocumentTypeId = source.DocumentTypeId,                  
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
                    CheckListMaster=source.CheckListMaster,
                    PropertyDescription=source.PropertyDescription                    
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetPropertyCheckListCountAsync(DataRequest<PropertyCheckList> request)
        {
            IQueryable<PropertyCheckList> items = _dataSource.PropertyCheckList;

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

        public async Task<int> UpdatePropertyCheckListAsync(PropertyCheckList model)
        {
            try
            {
                ICollection<PropertyCheckListDocuments> docs = model.PropertyCheckListDocuments;
                ICollection<PropertyCheckListVendor> vendors = model.PropertyCheckListVendors;
                ICollection<CheckListOfProperty> checklist = model.CheckListOfProperties;
                _dataSource.Entry(model).State = EntityState.Modified;
                int res = await _dataSource.SaveChangesAsync();
                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (doc.PropertyCheckListBlobId == 0)
                        {
                            doc.PropertyGuid = model.PropertyGuid;
                            _dataSource.PropertyCheckListDocuments.Add(doc);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();
                if (vendors != null)
                {
                    foreach (var vendor in vendors)
                    {
                        if (vendor.CehckListVendorId == 0)
                        {
                            _dataSource.PropertyCheckListVendor.Add(vendor);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();

                if (checklist != null)
                {
                    foreach (var check in checklist)
                    {
                        if (check.CheckListPropertyId == 0)
                        {
                            check.PropertyCheckListId = res;
                            _dataSource.CheckListOfProperty.Add(check);
                        }
                    }
                }
               
                await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeletePropertyCheckListAsync(PropertyCheckList model)
        {
            _dataSource.PropertyCheckList.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

      
        public async Task<int> DeletePropertyCheckListDocumentAsync(PropertyCheckListDocuments documents)
        {
            _dataSource.PropertyCheckListDocuments.Remove(documents);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
