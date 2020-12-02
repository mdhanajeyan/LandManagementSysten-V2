using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPaymentAsync(Payment model)
        {
            if (model == null)
                return 0;

            var entity = new Payment()
            {
                PaymentId = model.PaymentId,
                PaymentGuid = model.PaymentGuid,
                PayeeId = model.PayeeId,
                PayeeTypeId = model.PayeeTypeId,
                ExpenseHeadId = model.ExpenseHeadId,
                PropertyId = model.PropertyId,
                PartyId = model.PartyId,
                PaymentTypeId = model.PaymentTypeId,
                DocumentTypeId = model.DocumentTypeId,
                DateOfPayment = model.DateOfPayment,
                Amount = model.Amount,
                ChequeNo = model.ChequeNo,
                Narration = model.Narration,
        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<Payment> GetPaymentAsync(long id)
        {
            return await _dataSource.Payments.Where(r => r.PaymentId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<Payment>> GetPaymentsAsync(DataRequest<Payment> request)
        {
            IQueryable<Payment> items = GetPayments(request);
            return await items.ToListAsync();
        }

        public async Task<IList<Payment>> GetPaymentsAsync(int skip, int take, DataRequest<Payment> request)
        {
            IQueryable<Payment> items = GetPayments(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Payment
                {
                    PaymentId = source.PaymentId,
                    PaymentGuid = source.PaymentGuid,
                    PayeeId = source.PayeeId,
                    PayeeTypeId = source.PayeeTypeId,
                    ExpenseHeadId = source.ExpenseHeadId,
                    PropertyId = source.PropertyId,
                    PartyId = source.PartyId,
                    PaymentTypeId = source.PaymentTypeId,
                    DocumentTypeId = source.DocumentTypeId,
                    DateOfPayment = source.DateOfPayment,
                    Amount = source.Amount,
                    ChequeNo = source.ChequeNo,
                    Narration = source.Narration,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Payment> GetPayments(DataRequest<Payment> request)
        {
            IQueryable<Payment> items = _dataSource.Payments;

            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.BuildSearchTerms().Contains(request.Query.ToLower()));
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

        public async Task<int> GetPaymentsCountAsync(DataRequest<Payment> request)
        {
            IQueryable<Payment> items = _dataSource.Payments;

            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.BuildSearchTerms().Contains(request.Query.ToLower()));
            }

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            return await items.CountAsync();
        }

        public async Task<int> UpdatePaymentAsync(Payment model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeletePaymentAsync(Payment model)
        {
            _dataSource.Payments.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

    }
}
