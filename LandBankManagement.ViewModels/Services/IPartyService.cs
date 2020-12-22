using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public interface IPartyService
    {
        Task<int> AddPartyAsync(PartyModel model, ICollection<ImagePickerResult> docs);
        Task<PartyModel> GetPartyAsync(long id);
        Task<IList<PartyModel>> GetPartiesAsync(DataRequest<Party> request);
        Task<IList<PartyModel>> GetPartiesAsync(int skip, int take, DataRequest<Party> request);
        Task<int> GetPartiesCountAsync(DataRequest<Party> request);
        Task<int> UpdatePartyAsync(PartyModel model, ICollection<ImagePickerResult> doc);
        Task<int> DeletePartyAsync(PartyModel model);
        Task<int> DeletePartyDocumentAsync(ImagePickerResult documents);
        Task<int> UploadPartyDocumentsAsync(List<ImagePickerResult> model, Guid guid);
        Task<ObservableCollection<ImagePickerResult>> GetDocuments(Guid guid);

    }
}
