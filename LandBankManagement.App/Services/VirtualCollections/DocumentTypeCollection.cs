using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class DocumentTypeCollection : VirtualCollection<DocumentTypeModel>
    {
        private DataRequest<DocumentType> _dataRequest = null;
        public IDocumentTypeService DocumentTypeService { get; }
        public DocumentTypeCollection(IDocumentTypeService documentTypeService, ILogService logService) : base(logService)
        {
            DocumentTypeService = documentTypeService;
        }

        private DocumentTypeModel _defaultItem = DocumentTypeModel.CreateEmpty();
        protected override DocumentTypeModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<DocumentType> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await DocumentTypeService.GetDocumentTypesCountAsync(_dataRequest);
                Ranges[0] = await DocumentTypeService.GetDocumentTypesAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<DocumentTypeModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await DocumentTypeService.GetDocumentTypesAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("DocumentTypeCollection", "Fetch", ex);
            }
            return null;

        }

    }
}
