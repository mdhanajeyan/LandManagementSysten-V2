﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPartyAsync(Party party)
        {
            try
            {
                if (party == null)
                    return 0;

                var entity = new Party()
                {
                    PartyFirstName = party.PartyFirstName,
                    PartyGuid = party.PartyGuid,
                    PartyAlias = party.PartyAlias,
                    PartySalutation = party.PartySalutation,
                    AadharNo = party.AadharNo,
                    ContactPerson = party.ContactPerson,
                    PAN = party.PAN,
                    GSTIN = party.GSTIN,
                    email = party.email,
                    IsPartyActive = party.IsPartyActive,
                    PhoneNo = party.PhoneNo,
                    AddressLine1 = party.AddressLine1,
                    AddressLine2 = party.AddressLine2,
                    City = party.City,
                    PinCode = party.PinCode
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdatePartyAsync(Party party)
        {
            try
            {
                _dataSource.Entry(party).State = EntityState.Modified;
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Party> GetPartyAsync(long id)
        {
            return await _dataSource.Parties.Where(r => r.PartyId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<Party>> GetPartiesAsync(int skip, int take, DataRequest<Party> request)
        {
            IQueryable<Party> items = GetParties(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Party
                {
                    PartyId = source.PartyId,
                    PartyFirstName = source.PartyFirstName,
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
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<IList<Party>> GetPartiesAsync(DataRequest<Party> request)
        {
            IQueryable<Party> items = GetParties(request);
            return await items.ToListAsync();
        }

        public async Task<int> DeletePartyAsync(Party model)
        {
            _dataSource.Parties.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> GetPartiesCountAsync(DataRequest<Party> request)
        {
            IQueryable<Party> items = _dataSource.Parties;

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

        private IQueryable<Party> GetParties(DataRequest<Party> request)
        {
            IQueryable<Party> items = _dataSource.Parties;

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
    }
}