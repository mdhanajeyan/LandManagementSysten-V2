using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface IGroupsService
    {
        Task<int> AddGroupsAsync(GroupsModel model);
        Task<GroupsModel> GetGroupsAsync(long id);
        Task<IList<GroupsModel>> GetGroupsAsync(DataRequest<Groups> request);
        Task<IList<GroupsModel>> GetGroupsAsync(int skip, int take, DataRequest<Groups> request);
        Task<int> GetGroupsCountAsync(DataRequest<Groups> request);
        Task<int> UpdateGroupsAsync(GroupsModel model);
        Task<int> DeleteGroupsAsync(GroupsModel model);
    }
}
