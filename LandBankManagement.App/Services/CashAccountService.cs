using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public class CashAccountService:ICashAccountService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public CashAccountService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddCashAccountAsync(CashAccountModel model)
        {
            long id = model.CashAccountId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var bankAccount = new CashAccount();
                if (bankAccount != null)
                {
                    UpdateCashAccountFromModel(bankAccount, model);
                    bankAccount.CashAccountGuid = Guid.NewGuid();
                    await dataService.AddCashAccountAsync(bankAccount);
                    model.Merge(await GetCashAccountAsync(dataService, bankAccount.CashAccountId));
                }
                return 0;
            }
        }

        static private async Task<CashAccountModel> GetCashAccountAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetCashAccountAsync(id);
            if (item != null)
            {
                return CreateCashAccountModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<CashAccountModel> GetCashAccountAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetCashAccountAsync(dataService, id);
            }
        }

        public async Task<IList<CashAccountModel>> GetCashAccountsAsync(DataRequest<CashAccount> request)
        {
            var collection = new CashAccountCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<CashAccountModel>> GetCashAccountsAsync(int skip, int take, DataRequest<CashAccount> request)
        {
            var models = new List<CashAccountModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCashAccountsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateCashAccountModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetCashAccountsCountAsync(DataRequest<CashAccount> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetCashAccountsCountAsync(request);
            }
        }

        public async Task<int> UpdateCashAccountAsync(CashAccountModel model)
        {
            long id = model.CashAccountId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var bankAccount =  new CashAccount();
                if (bankAccount != null)
                {
                    UpdateCashAccountFromModel(bankAccount, model);
                    await dataService.UpdateCashAccountAsync(bankAccount);
                    model.Merge(await GetCashAccountAsync(dataService, bankAccount.CashAccountId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteCashAccountAsync(CashAccountModel model)
        {
            var bankAcc = new CashAccount { CashAccountId = model.CashAccountId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteCashAccountAsync(bankAcc);
            }
        }

        static public CashAccountModel CreateCashAccountModelAsync(CashAccount source, bool includeAllFields)
        {
            var model = new CashAccountModel()
            {
                CashAccountId = source.CashAccountId,
                CashAccountGuid = source.CashAccountGuid,
                CashAccountName = source.CashAccountName,
                IsCashAccountActive = source.IsCashAccountActive,
                AccountTypeId = source.AccountTypeId,
                CompanyID = source.CompanyID,
                CompanyName=source.CompanyName
            };
            return model;
        }

        private void UpdateCashAccountFromModel(CashAccount target, CashAccountModel source)
        {
            target.CashAccountId = source.CashAccountId;
            target.CashAccountGuid = source.CashAccountGuid;
            target.CashAccountName = source.CashAccountName;
            target.IsCashAccountActive = source.IsCashAccountActive;
            target.AccountTypeId = source.AccountTypeId;
            target.CompanyID = source.CompanyID;
        }
    }
}
