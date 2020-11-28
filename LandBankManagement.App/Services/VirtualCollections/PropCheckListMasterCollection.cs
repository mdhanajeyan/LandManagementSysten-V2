using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;


namespace LandBankManagement.Services
{
    public class PropCheckListMasterCollection : VirtualCollection<PropCheckListMasterModel>
    {
        private DataRequest<PropCheckListMaster> _dataRequest = null;
        public IPropCheckListMasterService PropCheckListMasterService { get; }

       

        public PropCheckListMasterCollection(IPropCheckListMasterService propCheckListMasterService, ILogService logService) : base(logService)
        {
            PropCheckListMasterService = propCheckListMasterService;
        }

        private PropCheckListMasterModel _defaultItem = PropCheckListMasterModel.CreateEmpty();
        protected override PropCheckListMasterModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<PropCheckListMaster> dataRequest)
        {
            
                _dataRequest = dataRequest;
                Count = await PropCheckListMasterService.GetPropCheckListMastersCountAsync(_dataRequest);
                Ranges[0] = await PropCheckListMasterService.GetPropCheckListMastersAsync(0, RangeSize, _dataRequest);
           
        }

        protected override async Task<IList<PropCheckListMasterModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await PropCheckListMasterService.GetPropCheckListMastersAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("PropCheckListMasterCollection", "Fetch", ex);
            }
            return null;
        }
    }
}
