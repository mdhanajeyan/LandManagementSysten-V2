using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
   partial class DataServiceBase
    {
        public async Task<int> AddFundTransferAsync(FundTransfer model)
        {
            if (model == null)
                return 0;

            var entity = new FundTransfer()
            {
                FundTransferId = model.FundTransferId,
                FundTransferGuid = model.FundTransferGuid,
                PayeeId = model.PayeeId,
                PayeePaymentType = model.PayeePaymentType,
                PayeeBankId = model.PayeeBankId,
                DateOfPayment = model.DateOfPayment,
                Amount = model.Amount,
                Narration = model.Narration,
                ReceiverId = model.ReceiverId,
                ReceiverPaymentType = model.ReceiverPaymentType,
                ReceiverBankId = model.ReceiverBankId,
        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        private IQueryable<FundTransfer> GetFundTransfers(DataRequest<FundTransfer> request)
        {
            IQueryable<FundTransfer> items = _dataSource.FundTransfers;

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


        public async Task<FundTransfer> GetFundTransferAsync(long id)
        {
            return await _dataSource.FundTransfers
                .Where(x => x.FundTransferId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<FundTransfer>> GetFundTransfersAsync(DataRequest<FundTransfer> request)
        {
            IQueryable<FundTransfer> items = GetFundTransfers(request);
            return await items.ToListAsync();
        }

        public async Task<IList<FundTransfer>> GetFundTransfersAsync(int skip, int take, DataRequest<FundTransfer> request)
        {
            IQueryable<FundTransfer> items = GetFundTransfers(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new FundTransfer
                {
                    FundTransferId = source.FundTransferId,
                    FundTransferGuid = source.FundTransferGuid,
                    PayeeId = source.PayeeId,
                    PayeePaymentType = source.PayeePaymentType,
                    PayeeBankId = source.PayeeBankId,
                    DateOfPayment = source.DateOfPayment,
                    Amount = source.Amount,
                    Narration = source.Narration,
                    ReceiverId = source.ReceiverId,
                    ReceiverPaymentType = source.ReceiverPaymentType,
                    ReceiverBankId = source.ReceiverBankId,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetFundTransfersCountAsync(DataRequest<FundTransfer> request)
        {
            IQueryable<FundTransfer> items = _dataSource.FundTransfers;

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

        public async Task<int> UpdateFundTransferAsync(FundTransfer model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteFundTransferAsync(FundTransfer model)
        {
            _dataSource.FundTransfers.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
