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
        private bool _progressRingVisibility;
        public bool ProgressRingVisibility
        {
            get => _progressRingVisibility;
            set => Set(ref _progressRingVisibility, value);
        }

        private bool _progressRingActive;
        public bool ProgressRingActive
        {
            get => _progressRingActive;
            set => Set(ref _progressRingActive, value);
        }
        public DocumentTypeViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IDocumentTypeService documentTypeService) : base(commonServices)
        {
            DocumentTypeService = documentTypeService;
            DocumentTypeList = new DocumentTypeListViewModel(documentTypeService, commonServices,this);
            DocumentTypeDetials = new DocumentTypeDetailsViewModel(documentTypeService, filePickerService, commonServices, DocumentTypeList,this);
        }

        public async Task LoadAsync(DocumentTypeListArgs args)
        {
            await DocumentTypeList.LoadAsync(args);
        }
        public void Unload()
        {
            DocumentTypeList.Unload();
        }
        int noOfApiCalls = 0;
        public void ShowProgressRing()
        {
            noOfApiCalls++;
               ProgressRingActive = true;
            ProgressRingVisibility = true;
        }
        public void HideProgressRing()
        {
            if (noOfApiCalls > 1)
            {
                noOfApiCalls--;
                return;
            }
            else
                noOfApiCalls--;
            ProgressRingActive = false;
            ProgressRingVisibility = false;
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
                ShowProgressRing();
                var model = await DocumentTypeService.GetDocumentTypeAsync(selected.DocumentTypeId);
                selected.Merge(model);
                DocumentTypeDetials.Item = model;
                HideProgressRing();
            }
            catch (Exception ex)
            {
                LogException("DocumentType", "Load Details", ex);
            }
        }

    }
}
