using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class CheckListCollection : VirtualCollection<CheckListModel>
    {
        private DataRequest<CheckList> _dataRequest = null;
        public ICheckListService CheckListService { get; }
        public CheckListCollection(ICheckListService checkListService, ILogService logService) : base(logService)
        {
            CheckListService = checkListService;
        }

        private CheckListModel _defaultItem = CheckListModel.CreateEmpty();
        protected override CheckListModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<CheckList> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await CheckListService.GeCheckListsCountAsync(_dataRequest);
                Ranges[0] = await CheckListService.GetCheckListsAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<CheckListModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await CheckListService.GetCheckListsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("CheckListCollection", "Fetch", ex);
            }
            return null;

        }

    }
}
