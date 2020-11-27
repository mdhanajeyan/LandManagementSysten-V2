using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class CashAccountCollection : VirtualCollection<CashAccountModel>
    {
        private DataRequest<CashAccount> _dataRequest = null;
        public ICashAccountService CashAccountService { get; }
        public CashAccountCollection(ICashAccountService cashAccountService, ILogService logService) : base(logService)
        {
            CashAccountService = cashAccountService;
        }

        private CashAccountModel _defaultItem = CashAccountModel.CreateEmpty();
        protected override CashAccountModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<CashAccount> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await CashAccountService.GetCashAccountsCountAsync(_dataRequest);
                Ranges[0] = await CashAccountService.GetCashAccountsAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<CashAccountModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await CashAccountService.GetCashAccountsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("CashAccountCollection", "Fetch", ex);
            }
            return null;

        }
    }
}
