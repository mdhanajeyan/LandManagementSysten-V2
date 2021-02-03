using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;
namespace LandBankManagement.Services
{
    public class BankAccountService : IBankAccountService
    {

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public BankAccountService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddBankAccountAsync(BankAccountModel model)
        {
            long id = model.BankAccountId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var bankAccount = new BankAccount();
                if (bankAccount != null)
                {
                    UpdateBankAccountFromModel(bankAccount, model);
                    bankAccount.BankGuid = Guid.NewGuid();
                    await dataService.AddBankAccountAsync(bankAccount);
                    model.Merge(await GetBankAccountAsync(dataService, bankAccount.BankAccountId));
                }
                return 0;
            }
        }

        static private async Task<BankAccountModel> GetBankAccountAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetBankAccountAsync(id);
            if (item != null)
            {
                return CreateBankAccountModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<BankAccountModel> GetBankAccountAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetBankAccountAsync(dataService, id);
            }
        }

       

        public async Task<IList<BankAccountModel>> GetBankAccountsAsync(DataRequest<BankAccount> request)
        {
            var collection = new BankAccountCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<BankAccountModel>> GetBankAccountsAsync(int skip, int take, DataRequest<BankAccount> request)
        {
            var models = new List<BankAccountModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetBankAccountsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateBankAccountModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetBankAccountsCountAsync(DataRequest<BankAccount> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetBankAccountsCountAsync(request);
            }
        }

        public async Task<int> UpdateBankAccountAsync(BankAccountModel model)
        {
            long id = model.BankAccountId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var bankAccount =  new BankAccount();
                if (bankAccount != null)
                {
                    UpdateBankAccountFromModel(bankAccount, model);
                    await dataService.UpdateBankAccountAsync(bankAccount);
                    model.Merge(await GetBankAccountAsync(dataService, bankAccount.BankAccountId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteBankAccountAsync(BankAccountModel model)
        {
            var bankAcc = new BankAccount { BankAccountId = model.BankAccountId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteBankAccountAsync(bankAcc);
            }
        }

        static public BankAccountModel CreateBankAccountModelAsync(BankAccount source, bool includeAllFields)
        {
            var model = new BankAccountModel()
            {
                BankAccountId = source.BankAccountId,
                BankGuid = source.BankGuid,
                BankName = source.BankName,
                BranchName = source.BranchName,
                AccountNumber = source.AccountNumber,
                AccountTypeId = source.AccountTypeId.ToString(),
                IFSCCode = source.IFSCCode,
                OpeningBalance = source.OpeningBalance.ToString(),
                IsBankAccountActive = source.IsBankAccountActive,
                CompanyName= source.CompanyName,
                CompanyID=source.CompanyID.ToString(),
                AccountTypeName=source.AccountTypeName
        };
            return model;
        }

        private void UpdateBankAccountFromModel(BankAccount target, BankAccountModel source)
        {
            target.BankAccountId = source.BankAccountId;
            target.BankGuid = source.BankGuid;
            target.BankName = source.BankName;
            target.BranchName = source.BranchName;
            target.AccountNumber = source.AccountNumber;
            target.AccountTypeId =Convert.ToInt32( source.AccountTypeId??"0");
            target.IFSCCode = source.IFSCCode;
            target.OpeningBalance =Convert.ToDecimal(  string.IsNullOrEmpty(source.OpeningBalance)?"0": source.OpeningBalance);
            target.IsBankAccountActive = source.IsBankAccountActive;
            target.CompanyID =Convert.ToInt32( source.CompanyID);

        }
    }
}
