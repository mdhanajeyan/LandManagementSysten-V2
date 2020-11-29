using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;
namespace LandBankManagement.Services
{
    public class PaymentService:IPaymentService
    {
        public PaymentService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public async Task<PaymentModel> GetPaymentAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetPaymentAsync(dataService, id);
            }
        }
        static private async Task<PaymentModel> GetPaymentAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetPaymentAsync(id);
            if (item != null)
            {
                return await CreatePaymentModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<PaymentModel>> GetPaymentsAsync(DataRequest<Payment> request)
        {
            var collection = new PaymentCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<PaymentModel>> GetPaymentsAsync(int skip, int take, DataRequest<Payment> request)
        {
            var models = new List<PaymentModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPaymentsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreatePaymentModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetPaymentsCountAsync(DataRequest<Payment> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetPaymentsCountAsync(request);
            }
        }

        public async Task<int> AddPaymentAsync(PaymentModel model)
        {
            long id = model.PaymentId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var vendor =  new Payment();
                if (vendor != null)
                {
                    UpdatePaymentFromModel(vendor, model);
                    vendor.PaymentGuid = Guid.NewGuid();
                    await dataService.AddPaymentAsync(vendor);
                    model.Merge(await GetPaymentAsync(dataService, vendor.PaymentId));
                }
                return 0;
            }
        }

        public async Task<int> UpdatePaymentAsync(PaymentModel model)
        {
            long id = model.PaymentId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var vendor = id > 0 ? await dataService.GetPaymentAsync(model.PaymentId) : new Payment();
                if (vendor != null)
                {
                    UpdatePaymentFromModel(vendor, model);
                    await dataService.UpdatePaymentAsync(vendor);
                    model.Merge(await GetPaymentAsync(dataService, vendor.PaymentId));
                }
                return 0;
            }
        }

        public async Task<int> DeletePaymentAsync(PaymentModel model)
        {
            var vendor = new Payment { PaymentId = model.PaymentId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePaymentAsync(vendor);
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

        static public async Task<PaymentModel> CreatePaymentModelAsync(Payment source, bool includeAllFields)
        {
            var model = new PaymentModel()
            {
                PaymentId = source.PaymentId,
                PaymentGuid = source.PaymentGuid,
                PayeeId = source.PayeeId,
                PayeeTypeId = source.PayeeTypeId,
                ExpenseHeadId = source.ExpenseHeadId,
                PropertyId = source.PropertyId,
                PartyId = source.PartyId,
                DocumentTypeId = source.DocumentTypeId,
                PaymentTypeId = source.PaymentTypeId,
                DateOfPayment = source.DateOfPayment,
                Amount = source.Amount,
                ChequeNo = source.ChequeNo,
                Narration = source.Narration,
        };

            return model;
        }

        private void UpdatePaymentFromModel(Payment target, PaymentModel source)
        {
            target.PaymentId = source.PaymentId;
            target.PaymentGuid = source.PaymentGuid;
            target.PayeeId = source.PayeeId;
            target.PayeeTypeId = source.PayeeTypeId;
            target.ExpenseHeadId = source.ExpenseHeadId;
            target.PropertyId = source.PropertyId;
            target.PartyId = source.PartyId;
            target.DocumentTypeId = source.DocumentTypeId;
            target.PaymentTypeId = source.PaymentTypeId;
            target.DateOfPayment = source.DateOfPayment;
            target.Amount = source.Amount;
            target.ChequeNo = source.ChequeNo;
            target.Narration = source.Narration;

        }
    }
}
