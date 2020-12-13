using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IVillageService
    {
        Task<VillageModel> AddVillageAsync(VillageModel model);
        Task<VillageModel> GetVillageAsync(long id);
        Task<IList<VillageModel>> GetVillagesAsync(DataRequest<Village> request);
        Task<IList<VillageModel>> GetVillagesAsync(int skip, int take, DataRequest<Village> request);
        Task<int> GetVillagesCountAsync(DataRequest<Village> request);
        Task<VillageModel> UpdateVillageAsync(VillageModel model);
        Task<Result> DeleteVillageAsync(VillageModel model);
    }
}
