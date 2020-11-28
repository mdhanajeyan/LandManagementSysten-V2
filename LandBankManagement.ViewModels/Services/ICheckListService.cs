using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface ICheckListService
    {
        Task<int> AddCheckListAsync(CheckListModel model);
        Task<CheckListModel> GetCheckListAsync(long id);
        Task<IList<CheckListModel>> GetCheckListsAsync(DataRequest<CheckList> request);
        Task<IList<CheckListModel>> GetCheckListsAsync(int skip, int take, DataRequest<CheckList> request);
        Task<int> GetCheckListsCountAsync(DataRequest<CheckList> request);
        Task<int> UpdateCheckListAsync(CheckListModel model);
        Task<int> DeleteCheckListAsync(CheckListModel model);
    }
}
