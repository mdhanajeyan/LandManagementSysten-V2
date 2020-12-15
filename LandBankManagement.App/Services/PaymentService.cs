using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                var payment =  new Payment();
                if (payment != null)
                {
                    UpdatePaymentFromModel(payment, model);
                    payment.PaymentGuid = Guid.NewGuid();
                    if (model.PaymentListModel != null && model.PaymentListModel.Count > 0) {
                        var list = new List<PaymentList>();
                        foreach (var obj in model.PaymentListModel) {
                            if (obj.PaymentListId > 0)
                                continue;
                            var paymentlist = new PaymentList();
                            UpdatePaymentListFromModel(paymentlist, obj);
                            list.Add(paymentlist);
                        }
                        payment.PaymentLists = list;

                    }
                   var paymentId= await dataService.AddPaymentAsync(payment);
                    model.Merge(await GetPaymentAsync(dataService, payment.PaymentId));
                    return paymentId;

                }
                return 0;
            }
        }

        public async Task<int> UpdatePaymentAsync(PaymentModel model)
        {
            long id = model.PaymentId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var payment = new Payment();
                UpdatePaymentFromModel(payment, model);
                if (model.PaymentListModel != null && model.PaymentListModel.Count > 0)
                {
                    var list = new List<PaymentList>();
                    foreach (var obj in model.PaymentListModel)
                    {
                        if (obj.PaymentListId > 0)
                            continue;
                        var paymentlist = new PaymentList();
                        UpdatePaymentListFromModel(paymentlist, obj);
                        list.Add(paymentlist);
                    }
                    payment.PaymentLists = list;

                }

                await dataService.UpdatePaymentAsync(payment);
                model.Merge(await GetPaymentAsync(dataService, payment.PaymentId));

                return 0;
            }
        }

        public async Task<int> DeletePaymentAsync(PaymentModel model)
        {
            var payment = new Payment { PaymentId = model.PaymentId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePaymentAsync(payment);
            }
        }

        public async Task<int> DeletePaymentListAsync(int id)
        {
            var payment = new PaymentList { PaymentId = id };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePaymentListAsync(payment);
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
                Amount = source.Amount.ToString(),
                ChequeNo = source.ChequeNo,
                Narration = source.Narration,
                BankAccountId=source.BankAccountId??0,
                CashAccountId=source.CashAccountId??0,
                PDC=source.PDC,
                AccountName=source.AccountName
        };
            if (source.PaymentLists!=null && source.PaymentLists.Count > 0)
            {
                model.PaymentListModel = new ObservableCollection<PaymentListModel>();
                foreach (var obj in source.PaymentLists)
                {
                    model.PaymentListModel.Add(new PaymentListModel
                    {
                        PaymentId = obj.PaymentId,
                        DateOfPayment=obj.DateOfPayment,
                        PaymentTypeId=obj.PaymentTypeId,
                        PaymentListId=obj.PaymentListId,
                        BankAccountId=obj.BankAccountId,
                        CashAccountId=obj.CashAccountId,
                        ChequeNo=obj.ChequeNo,
                        Narration=obj.Narration,
                        Amount=obj.Amount,
                        PDC=obj.PDC,
                        AccountName=obj.AccountName
                    }) ;

                }
            }

            return model;
        }

        private void UpdatePaymentListFromModel(PaymentList target, PaymentListModel source) {
            target.DateOfPayment = source.DateOfPayment.UtcDateTime;
            target.BankAccountId = source.BankAccountId;
            target.CashAccountId = source.CashAccountId;
            target.ChequeNo = source.ChequeNo;
            target.Narration = source.Narration;
            target.PaymentId = source.PaymentId;
            target.PaymentTypeId = source.PaymentTypeId;
            target.Amount = source.Amount;
            target.PDC = source.PDC;
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
            target.DateOfPayment = source.DateOfPayment.UtcDateTime;
            target.Amount = Convert.ToDecimal(string.IsNullOrEmpty(source.Amount) ? "0" : source.Amount);
            target.ChequeNo = source.ChequeNo;
            target.Narration = source.Narration;
            target.BankAccountId = source.BankAccountId;
            target.CashAccountId = source.CashAccountId;
            target.PDC = source.PDC;
            target.AccountName = source.AccountName;
        }
    }
}
