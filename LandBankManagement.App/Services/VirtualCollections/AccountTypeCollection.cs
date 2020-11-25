using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;


namespace LandBankManagement.Services
{
    public class AccountTypeCollection : VirtualCollection<AccountTypeModel>
    {
        private DataRequest<AccountType> _dataRequest = null;
        public IAccountTypeService AccountTypeService { get; }
        public AccountTypeCollection(IAccountTypeService accountTypeService, ILogService logService) : base(logService)
        {
            AccountTypeService = accountTypeService;
        }



        private AccountTypeModel _defaultItem = AccountTypeModel.CreateEmpty();
        protected override AccountTypeModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<AccountType> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await AccountTypeService.GetAccountTypesCountAsync(_dataRequest);
                Ranges[0] = await AccountTypeService.GetAccountTypesAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw;
            }
        }

        protected override async Task<IList<AccountTypeModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await AccountTypeService.GetAccountTypesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("AccountTypeCollection", "Fetch", ex);
            }
            return null;
        }
    }
}
