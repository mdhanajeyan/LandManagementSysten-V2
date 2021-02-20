using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class CheckListDetailsViewModel : GenericDetailsViewModel<CheckListModel>
    {
        public ICheckListService CheckListService { get; }
        public IFilePickerService FilePickerService { get; }
        public CheckListListViewModel CheckListListViewModel { get; }
        private CheckListViewModel CheckListViewModel { get; set; }
        private bool IsProcessing = false;
        public CheckListDetailsViewModel(ICheckListService checkListService, IFilePickerService filePickerService, ICommonServices commonServices, CheckListListViewModel checkListListViewModel, CheckListViewModel checkListViewModel) : base(commonServices)
        {
            CheckListService = checkListService;
            FilePickerService = filePickerService;
            CheckListListViewModel = checkListListViewModel;
            CheckListViewModel = checkListViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New CheckList" : TitleEdit;
        public string TitleEdit => Item == null ? "CheckList" : $"{Item.CheckListName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;


        public async Task LoadAsync()
        {
            Item = new CheckListModel();
            IsEditMode = true;
        }
        public void Unload()
        {

        }

        public void Subscribe()
        {
            MessageService.Subscribe<CheckListDetailsViewModel, CheckListModel>(this, OnDetailsMessage);
            MessageService.Subscribe<CheckListListViewModel>(this, OnListMessage);
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

        protected override async Task<bool> SaveItemAsync(CheckListModel model)
        {
            try
            {
                if (IsProcessing)
                    return false;
                IsProcessing = true;
                StartStatusMessage("Saving CheckList...");
                CheckListViewModel.ShowProgressRing();
                if (model.CheckListId <= 0)
                    await CheckListService.AddCheckListAsync(model);
                else
                    await CheckListService.UpdateCheckListAsync(model);
                IsProcessing = false;
                EndStatusMessage("CheckList saved");
                ShowPopup("success", "CheckList details is Saved");
                await CheckListListViewModel.RefreshAsync();
                ClearItem();
                LogInformation("CheckList", "Save", "CheckList saved successfully", $"CheckList {model.CheckListId} '{model.CheckListName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                IsProcessing = false;
                ShowPopup("error", "CheckList details is not Saved");
                StatusError($"Error saving CheckList: {ex.Message}");
                LogException("CheckList", "Save", ex);
                return false;
            }
            finally {
                CheckListViewModel.HideProgressRing();
            }
        }

        protected override void ClearItem()
        {
            Item = new CheckListModel();
        }
        protected override async Task<bool> DeleteItemAsync(CheckListModel model)
        {
            try
            {
                StartStatusMessage("Deleting CheckList...");
                CheckListViewModel.ShowProgressRing();
                await CheckListService.DeleteCheckListAsync(model);
                await CheckListListViewModel.RefreshAsync();
                ShowPopup("success", "CheckList details is deleted");
                ClearItem();
                EndStatusMessage("CheckList deleted");
                LogWarning("CheckList", "Delete", "CheckList deleted", $"CheckList {model.CheckListId} '{model.CheckListName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "CheckList details is not deleted");
                StatusError($"Error deleting CheckList: {ex.Message}");
                LogException("CheckList", "Delete", ex);
                return false;
            }
            finally {
                CheckListViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.CheckListId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current CheckList?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<CheckListModel>> GetValidationConstraints(CheckListModel model)
        {
            yield return new RequiredConstraint<CheckListModel>("Check List Name", m => m.CheckListName);
           
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(CheckListDetailsViewModel sender, string message, CheckListModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.CheckListId == current?.CheckListId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await CheckListService.GetCheckListAsync(current.CheckListId);
                                    item = item ?? new CheckListModel { CheckListId = current.CheckListId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This CheckList has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("CheckList", "Handle Changes", ex);
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

        private async void OnListMessage(CheckListListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<CheckListModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.CheckListId == current.CheckListId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await CheckListService.GetCheckListAsync(current.CheckListId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("CheckList", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This CheckList has been deleted externally");
            });
        }
    }
}
