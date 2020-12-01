using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
   public class DocumentTypeViewModel : ViewModelBase
    {
        IDocumentTypeService DocumentTypeService { get; }
        public DocumentTypeListViewModel DocumentTypeList { get; set; }

        public DocumentTypeDetailsViewModel DocumentTypeDetials { get; set; }

        public DocumentTypeViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IDocumentTypeService documentTypeService) : base(commonServices)
        {
            DocumentTypeService = documentTypeService;
            DocumentTypeList = new DocumentTypeListViewModel(documentTypeService, commonServices);
            DocumentTypeDetials = new DocumentTypeDetailsViewModel(documentTypeService, filePickerService, commonServices, DocumentTypeList);
        }

        public async Task LoadAsync(DocumentTypeListArgs args)
        {
            await DocumentTypeList.LoadAsync(args);
        }
        public void Unload()
        {
            DocumentTypeList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<DocumentTypeListViewModel>(this, OnMessage);
            DocumentTypeList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            DocumentTypeList.Unsubscribe();

        }

        private async void OnMessage(DocumentTypeListViewModel viewModel, string message, object args)
        {
            if (viewModel == DocumentTypeList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = DocumentTypeList.SelectedItem;
            if (!DocumentTypeList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(DocumentTypeModel selected)
        {
            try
            {
                var model = await DocumentTypeService.GetDocumentTypeAsync(selected.DocumentTypeId);
                selected.Merge(model);
                DocumentTypeDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("DocumentType", "Load Details", ex);
            }
        }

    }
}
