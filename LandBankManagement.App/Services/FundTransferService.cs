using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public class FundTransferService : IFundTransferService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public FundTransferService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<FundTransferModel> AddFundTransferAsync(FundTransferModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var fundTransfer = new FundTransfer();
                if (fundTransfer != null)
                {
                    UpdateFundTransferFromModel(fundTransfer, model);
                    fundTransfer.FundTransferGuid = Guid.NewGuid();
                   var id= await dataService.AddFundTransferAsync(fundTransfer);
                    model.Merge(await GetFundTransferAsync(dataService, id));
                }
                return model;
            }
        }

        static private async Task<FundTransferModel> GetFundTransferAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetFundTransferAsync(id);
            if (item != null)
            {
                return await CreateFundTransferModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<FundTransferModel> GetFundTransferAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetFundTransferAsync(dataService, id);
            }
        }

        public async Task<IList<FundTransferModel>> GetFundTransfersAsync(DataRequest<FundTransfer> request)
        {
            var collection = new FundTransferCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<FundTransferModel>> GetFundTransfersAsync(int skip, int take, DataRequest<FundTransfer> request)
        {
            var models = new List<FundTransferModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetFundTransfersAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateFundTransferModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }       

        public async Task<int> GetFundTransfersCountAsync(DataRequest<FundTransfer> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetFundTransfersCountAsync(request);
            }
        }

        public async Task<FundTransferModel> UpdateFundTransferAsync(FundTransferModel model)
        {
            long id = model.FundTransferId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var fundTransfer = id > 0 ? await dataService.GetFundTransferAsync(model.FundTransferId) : new FundTransfer();
                if (fundTransfer != null)
                {
                    UpdateFundTransferFromModel(fundTransfer, model);
                    await dataService.UpdateFundTransferAsync(fundTransfer);
                    model.Merge(await GetFundTransferAsync(dataService, fundTransfer.FundTransferId));
                }
                return model;
            }
        }

        public async Task<int> DeleteFundTransferAsync(FundTransferModel model)
        {
            var fundTransfer = new FundTransfer { FundTransferId = model.FundTransferId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteFundTransferAsync(fundTransfer);
            }
        }


        static public async Task<FundTransferModel> CreateFundTransferModelAsync(FundTransfer source, bool includeAllFields)
        {
            var model = new FundTransferModel()
            {
                FundTransferId = source.FundTransferId,
                FundTransferGuid = source.FundTransferGuid,
                PayeeId = source.PayeeId.ToString(),
                PayeePaymentType = source.PayeePaymentType,
                PayeeBankId = source.PayeeBankId,
                PayeeCashId=source.PayeeCashId,
                DateOfPayment = source.DateOfPayment,
                Amount = source.Amount.ToString(),
                Narration = source.Narration,
                ReceiverId = source.ReceiverId.ToString(),
                ReceiverPaymentType = source.ReceiverPaymentType,
                ReceiverBankId = source.ReceiverBankId,
                ReceiverCashId=source.ReceiverCashId,
                FromAccountName=source.FromAccountName,
                FromCompanyName=source.FromCompanyName,
                ToAccountName=source.ToAccountName,
                ToCompanyName=source.ToCompanyName
        };

            return model;
        }

        private void UpdateFundTransferFromModel(FundTransfer target, FundTransferModel source)
        {
            target.FundTransferId = source.FundTransferId;
            target.FundTransferGuid = source.FundTransferGuid;
            target.PayeeId = Convert.ToInt32(source.PayeeId);
            target.PayeePaymentType = source.PayeePaymentType;
            target.PayeeBankId = source.PayeeBankId;
            target.PayeeCashId = source.PayeeCashId;
            target.DateOfPayment = source.DateOfPayment.UtcDateTime;
            target.Amount = Convert.ToDecimal(string.IsNullOrEmpty(source.Amount) ? "0" : source.Amount);
            target.Narration = source.Narration;
            target.ReceiverId = Convert.ToInt32(source.ReceiverId);
            target.ReceiverPaymentType = source.ReceiverPaymentType;
            target.ReceiverBankId = source.ReceiverBankId;
            target.ReceiverCashId = source.ReceiverCashId;
        }

    }
}
