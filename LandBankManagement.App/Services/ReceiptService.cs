using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;
namespace LandBankManagement.Services
{
    public class ReceiptService:IReceiptService
    {
        public ReceiptService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public async Task<ReceiptModel> GetReceiptAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetReceiptAsync(dataService, id);
            }
        }
        static private async Task<ReceiptModel> GetReceiptAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetReceiptAsync(id);
            if (item != null)
            {
                return await CreateReceiptModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<ReceiptModel>> GetReceiptsAsync(DataRequest<Receipt> request)
        {
            var collection = new ReceiptCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<ReceiptModel>> GetReceiptsAsync(int skip, int take, DataRequest<Receipt> request)
        {
            var models = new List<ReceiptModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetReceiptsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateReceiptModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetReceiptsCountAsync(DataRequest<Receipt> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetReceiptsCountAsync(request);
            }
        }

        public async Task<ReceiptModel> AddReceiptAsync(ReceiptModel model)
        {
            long id = model.ReceiptId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var receipt =  new Receipt();
                if (receipt != null)
                {
                    UpdateReceiptFromModel(receipt, model);
                    receipt.ReceiptGuid = Guid.NewGuid();
                    await dataService.AddReceiptAsync(receipt);
                    model.Merge(await GetReceiptAsync(dataService, receipt.ReceiptId));
                }
                return model;
            }
        }

        public async Task<ReceiptModel> UpdateReceiptAsync(ReceiptModel model)
        {
            long id = model.ReceiptId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var receipt = id > 0 ? await dataService.GetReceiptAsync(model.ReceiptId) : new Receipt();
                if (receipt != null)
                {
                    UpdateReceiptFromModel(receipt, model);
                    await dataService.UpdateReceiptAsync(receipt);
                    model.Merge(await GetReceiptAsync(dataService, receipt.ReceiptId));
                }
                return model;
            }
        }

        public async Task<int> DeleteReceiptAsync(ReceiptModel model)
        {
            var receipt = new Receipt { ReceiptId = model.ReceiptId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteReceiptAsync(receipt);
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

        static public async Task<ReceiptModel> CreateReceiptModelAsync(Receipt source, bool includeAllFields)
        {
            var model = new ReceiptModel()
            {
                ReceiptId = source.ReceiptId,
                ReceiptGuid = source.ReceiptGuid,
                PayeeId = source.PayeeId,
                DealId = source.DealId,
                PartyId = source.PartyId,
                PaymentTypeId = source.PaymentTypeId,
                DepositBankId = source.DepositBankId,
                DepositCashId = source.DepositCashId,
                DateOfPayment = source.DateOfPayment,
                Amount = source.Amount.ToString(),
                Narration = source.Narration,
                BankName=source.BankName,
                CashName=source.CashName
            };

            return model;
        }

        private void UpdateReceiptFromModel(Receipt target, ReceiptModel source)
        {
            target.ReceiptId = source.ReceiptId;
            target.ReceiptGuid = source.ReceiptGuid;
            target.PayeeId = source.PayeeId;
            target.DealId = source.DealId;
            target.PartyId = source.PartyId;
            target.PaymentTypeId = source.PaymentTypeId;
            target.DepositBankId = source.DepositBankId;
            target.DepositCashId = source.DepositCashId;
            target.DateOfPayment = source.DateOfPayment.UtcDateTime;
            target.Amount = Convert.ToDecimal(string.IsNullOrEmpty(source.Amount) ? "0" : source.Amount);
            target.Narration = source.Narration;

        }
    }
}
