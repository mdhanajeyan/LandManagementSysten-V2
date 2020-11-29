using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IPropCheckListMasterService
    {
        Task<int> AddPropCheckListMasterAsync(PropCheckListMasterModel model);
        Task<PropCheckListMasterModel> GetPropCheckListMasterAsync(long id);
        Task<IList<PropCheckListMasterModel>> GetPropCheckListMastersAsync(DataRequest<PropCheckListMaster> request);
        Task<IList<PropCheckListMasterModel>> GetPropCheckListMastersAsync(int skip, int take, DataRequest<PropCheckListMaster> request);
        Task<int> GetPropCheckListMastersCountAsync(DataRequest<PropCheckListMaster> request);
        Task<int> UpdatePropCheckListMasterAsync(PropCheckListMasterModel model);
        Task<int> DeletePropCheckListMasterAsync(PropCheckListMasterModel model);
    }
}
