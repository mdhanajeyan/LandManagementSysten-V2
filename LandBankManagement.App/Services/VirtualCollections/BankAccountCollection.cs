using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class BankAccountCollection : VirtualCollection<BankAccountModel>
    {
        private DataRequest<BankAccount> _dataRequest = null;
        public IBankAccountService BankAccountService { get; }
        public BankAccountCollection(IBankAccountService bankAccountService, ILogService logService) : base(logService)
        {
            BankAccountService = bankAccountService;
        }

        private BankAccountModel _defaultItem = BankAccountModel.CreateEmpty();
        protected override BankAccountModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<BankAccount> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await BankAccountService.GetBankAccountsCountAsync(_dataRequest);
                Ranges[0] = await BankAccountService.GetBankAccountsAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<BankAccountModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await BankAccountService.GetBankAccountsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("BankAccountCollection", "Fetch", ex);
            }
            return null;

        }

    }
}
