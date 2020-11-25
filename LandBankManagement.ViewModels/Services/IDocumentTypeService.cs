using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
   public interface IDocumentTypeService
    {
        Task<int> AddDocumentTypeAsync(DocumentTypeModel model);
        Task<DocumentTypeModel> GetDocumentTypeAsync(long id);
        Task<IList<DocumentTypeModel>> GetDocumentTypesAsync(DataRequest<DocumentType> request);
        Task<IList<DocumentTypeModel>> GetDocumentTypesAsync(int skip, int take, DataRequest<DocumentType> request);
        Task<int> GetDocumentTypesCountAsync(DataRequest<DocumentType> request);
        Task<int> UpdateDocumentTypeAsync(DocumentTypeModel model);
        Task<int> DeleteDocumentTypeAsync(DocumentTypeModel model);
    }
}
