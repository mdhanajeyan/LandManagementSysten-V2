using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{

    public class DocumentTypeDetailsViewModel : GenericDetailsViewModel<DocumentTypeModel>
    {
        public IDocumentTypeService DocumentTypeService { get; }
        public IFilePickerService FilePickerService { get; }
        public DocumentTypeListViewModel DocumentTypeListViewModel {get;}
        public DocumentTypeViewModel DocumentTypeViewModel { get; set; }
        public DocumentTypeDetailsViewModel(IDocumentTypeService documentTypeService, IFilePickerService filePickerService, ICommonServices commonServices, DocumentTypeListViewModel documentTypeListViewModel, DocumentTypeViewModel documentTypeViewModel) : base(commonServices)
        {
            DocumentTypeService = documentTypeService;
            FilePickerService = filePickerService;
            DocumentTypeListViewModel = documentTypeListViewModel;
            DocumentTypeViewModel = documentTypeViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New DocumentType" : TitleEdit;
        public string TitleEdit => Item == null ? "DocumentType" : $"{Item.DocumentTypeName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;


        public async Task LoadAsync()
        {
            Item = new DocumentTypeModel { IsDocumentTypeActive=true};
        }
        public void Unload()
        {
            
        }

        public void Subscribe()
        {
            MessageService.Subscribe<DocumentTypeDetailsViewModel, DocumentTypeModel>(this, OnDetailsMessage);
            MessageService.Subscribe<DocumentTypeListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        private object _newPictureSource = null;
        public object NewPictureSource
        {
            get => _newPictureSource;
            set => Set(ref _newPictureSource, value);
        }

        public override void BeginEdit()
        {
            NewPictureSource = null;
            base.BeginEdit();
        }

        public ICommand EditPictureCommand => new RelayCommand(OnEditFile);
        private async void OnEditFile()
        {
            NewPictureSource = null;
            var result = await FilePickerService.OpenImagePickerAsync();
            if (result != null)
            {

               // NewPictureSource = result.ImageSource;
            }
            else
            {
                NewPictureSource = null;
            }
        }

        protected override async Task<bool> SaveItemAsync(DocumentTypeModel model)
        {
            try
            {
                StartStatusMessage("Saving DocumentType...");
                DocumentTypeViewModel.ShowProgressRing();
                if (model.DocumentTypeId <= 0)
                    await DocumentTypeService.AddDocumentTypeAsync(model);
                else
                    await DocumentTypeService.UpdateDocumentTypeAsync(model);

                await DocumentTypeListViewModel.RefreshAsync();
                ClearItem();
                EndStatusMessage("DocumentType saved");
                LogInformation("DocumentType", "Save", "DocumentType saved successfully", $"DocumentType {model.DocumentTypeId} '{model.DocumentTypeName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Party: {ex.Message}");
                LogException("Party", "Save", ex);
                return false;
            }
            finally {
                DocumentTypeViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = new DocumentTypeModel { IsDocumentTypeActive = true };
        }
        protected override async Task<bool> DeleteItemAsync(DocumentTypeModel model)
        {
            try
            {
                StartStatusMessage("Deleting DocumentType...");
                DocumentTypeViewModel.ShowProgressRing();
                await DocumentTypeService.DeleteDocumentTypeAsync(model);
                ClearItem();
                await DocumentTypeListViewModel.RefreshAsync();
                EndStatusMessage("DocumentType deleted");
                LogWarning("DocumentType", "Delete", "DocumentType deleted", $"DocumentType {model.DocumentTypeId} '{model.DocumentTypeName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting DocumentType: {ex.Message}");
                LogException("DocumentType", "Delete", ex);
                return false;
            }
            finally {
                DocumentTypeViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.DocumentTypeId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current DocumentType?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<DocumentTypeModel>> GetValidationConstraints(DocumentTypeModel model)
        {
            yield return new RequiredConstraint<DocumentTypeModel>("Document Type", m => m.DocumentTypeName);
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(DocumentTypeDetailsViewModel sender, string message, DocumentTypeModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.DocumentTypeId == current?.DocumentTypeId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await DocumentTypeService.GetDocumentTypeAsync(current.DocumentTypeId);
                                    item = item ?? new DocumentTypeModel { DocumentTypeId = current.DocumentTypeId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This DocumentType has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("DocumentType", "Handle Changes", ex);
                                }
                            });
                            break;
                        case "ItemDeleted":
                            await OnItemDeletedExternally();
                            break;
                    }
                }
            }
        }

        private async void OnListMessage(DocumentTypeListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<DocumentTypeModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.DocumentTypeId == current.DocumentTypeId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await DocumentTypeService.GetDocumentTypeAsync(current.DocumentTypeId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("DocumentType", "Handle Ranges Deleted", ex);
                        }
                        break;
                }
            }
        }

        private async Task OnItemDeletedExternally()
        {
            await ContextService.RunAsync(() =>
            {
                CancelEdit();
                IsEnabled = false;
                StatusMessage("WARNING: This DocumentType has been deleted externally");
            });
        }

    }
}
