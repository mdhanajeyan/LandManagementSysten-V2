using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class GroupsCollection : VirtualCollection<GroupsModel>
    {
        private DataRequest<Groups> _dataRequest = null;
        public IGroupsService GroupsService { get; }
        public GroupsCollection(IGroupsService groupsService, ILogService logService) : base(logService)
        {
            GroupsService = groupsService;
        }

        private GroupsModel _defaultItem = GroupsModel.CreateEmpty();
        protected override GroupsModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Groups> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await GroupsService.GetGroupsCountAsync(_dataRequest);
                Ranges[0] = await GroupsService.GetGroupsAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<GroupsModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await GroupsService.GetGroupsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("GroupsCollection", "Fetch", ex);
            }
            return null;

        }

    }
}
