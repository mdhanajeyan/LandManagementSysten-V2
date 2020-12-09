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

            var entity = new Property()
            {
                PropertyId = model.PropertyId,
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
                SaleValue2 = model.SaleValue2
            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<Property> GetPropertyAsync(long id)
        {
            return await _dataSource.Properties.Where(r => r.PropertyId == id).FirstOrDefaultAsync();

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
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeletePropertyAsync(Property model)
        {
            _dataSource.Properties.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
