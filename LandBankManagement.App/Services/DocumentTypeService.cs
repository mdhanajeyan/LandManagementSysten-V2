using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public DocumentTypeService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddDocumentTypeAsync(DocumentTypeModel model)
        {
            long id = model.DocumentTypeId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var documentType = new DocumentType();
                if (documentType != null)
                {
                    UpdateDocumentTypeFromModel(documentType, model);
                    documentType.DocumentTypeGuid = Guid.NewGuid();
                    await dataService.AddDocumentTypeAsync(documentType);
                    model.Merge(await GetDocumentTypeAsync(dataService, documentType.DocumentTypeId));
                }
                return 0;
            }
        }

        static private async Task<DocumentTypeModel> GetDocumentTypeAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetDocumentTypeAsync(id);
            if (item != null)
            {
                return CreateDocumentTypeModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public static DocumentTypeModel CreateDocumentTypeModelAsync(DocumentType source, bool includeAllFields)
        {
            var model = new DocumentTypeModel()
            {
                DocumentTypeId = source.DocumentTypeId,
                DocumentTypeGuid = source.DocumentTypeGuid,
                DocumentTypeName = source.DocumentTypeName,
                IsDocumentTypeActive = source.IsDocumentTypeActive,
            };

            return model;
        }

        private void UpdateDocumentTypeFromModel(DocumentType target, DocumentTypeModel source)
        {
            target.DocumentTypeId = source.DocumentTypeId;
            target.DocumentTypeGuid = source.DocumentTypeGuid;
            target.DocumentTypeName = source.DocumentTypeName;
            target.IsDocumentTypeActive = source.IsDocumentTypeActive;
        }

        public async Task<DocumentTypeModel> GetDocumentTypeAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetDocumentTypeAsync(dataService, id);
            }
        }

        public async Task<IList<DocumentTypeModel>> GetDocumentTypesAsync(DataRequest<DocumentType> request)
        {
            var collection = new DocumentTypeCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<DocumentTypeModel>> GetDocumentTypesAsync(int skip, int take, DataRequest<DocumentType> request)
        {
            var models = new List<DocumentTypeModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetDocumentTypesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(CreateDocumentTypeModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetDocumentTypesCountAsync(DataRequest<DocumentType> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetDocumentTypesCountAsync(request);
            }
        }

        public async Task<int> UpdateDocumentTypeAsync(DocumentTypeModel model)
        {
            long id = model.DocumentTypeId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var documentType = id > 0 ? await dataService.GetDocumentTypeAsync(model.DocumentTypeId) : new DocumentType();
                if (documentType != null)
                {
                    UpdateDocumentTypeFromModel(documentType, model);
                    await dataService.UpdateDocumentTypeAsync(documentType);
                    model.Merge(await GetDocumentTypeAsync(dataService, documentType.DocumentTypeId));
                }
                return 0;
            }
        }

        public async Task<int> DeleteDocumentTypeAsync(DocumentTypeModel model)
        {
            var documentType = new DocumentType { DocumentTypeId = model.DocumentTypeId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteDocumentTypeAsync(documentType);
            }
        }
    }
}
