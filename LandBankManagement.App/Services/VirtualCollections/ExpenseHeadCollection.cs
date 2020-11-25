using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public class ExpenseHeadCollection : VirtualCollection<ExpenseHeadModel>
    {
        private DataRequest<ExpenseHead> _dataRequest = null;
        public IExpenseHeadService ExpenseHeadService { get; }
        public ExpenseHeadCollection(IExpenseHeadService expenseHeadService, ILogService logService) : base(logService)
        {
            ExpenseHeadService = expenseHeadService;
        }

        private ExpenseHeadModel _defaultItem = ExpenseHeadModel.CreateEmpty();
        protected override ExpenseHeadModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<ExpenseHead> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await ExpenseHeadService.GetExpenseHeadsCountAsync(_dataRequest);
                Ranges[0] = await ExpenseHeadService.GetExpenseHeadsAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<ExpenseHeadModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await ExpenseHeadService.GetExpenseHeadsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("ExpenseHeadCollection", "Fetch", ex);
            }
            return null;

        }
    }
}
