using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface IPartyService
    {
        Task<int> AddPartyAsync(PartyModel model);
        Task<PartyModel> GetPartyAsync(long id);
        Task<IList<PartyModel>> GetPartiesAsync(DataRequest<Party> request);
        Task<IList<PartyModel>> GetPartiesAsync(int skip, int take, DataRequest<Party> request);
        Task<int> GetPartiesCountAsync(DataRequest<Party> request);
        Task<int> UpdatePartyAsync(PartyModel model);
        Task<int> DeletePartyAsync(PartyModel model);
    }
}
