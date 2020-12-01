using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
  public  class ExpenseHeadDetailsViewModel : GenericDetailsViewModel<ExpenseHeadModel>
    {
        public IExpenseHeadService ExpenseHeadService { get; }
        public IFilePickerService FilePickerService { get; }
        public ExpenseHeadListViewModel ExpenseHeadListViewModel { get; }
        public ExpenseHeadDetailsViewModel(IExpenseHeadService documentTypeService, IFilePickerService filePickerService, ICommonServices commonServices, ExpenseHeadListViewModel documentTypeListViewModel) : base(commonServices)
        {
            ExpenseHeadService = documentTypeService;
            FilePickerService = filePickerService;
            ExpenseHeadListViewModel = documentTypeListViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New ExpenseHead" : TitleEdit;
        public string TitleEdit => Item == null ? "ExpenseHead" : $"{Item.ExpenseHeadName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;


        public async Task LoadAsync()
        {
            Item = new ExpenseHeadModel();
        }
        public void Unload()
        {

        }

        public void Subscribe()
        {
            MessageService.Subscribe<ExpenseHeadDetailsViewModel, ExpenseHeadModel>(this, OnDetailsMessage);
            MessageService.Subscribe<ExpenseHeadListViewModel>(this, OnListMessage);
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

        protected override async Task<bool> SaveItemAsync(ExpenseHeadModel model)
        {
            try
            {
                StartStatusMessage("Saving ExpenseHead...");
                await Task.Delay(100);
                if (model.ExpenseHeadId <= 0)
                    await ExpenseHeadService.AddExpenseHeadAsync(model);
                else
                    await ExpenseHeadService.UpdateExpenseHeadAsync(model);

                await ExpenseHeadListViewModel.RefreshAsync();
                ClearItem();
                EndStatusMessage("ExpenseHead saved");
                LogInformation("ExpenseHead", "Save", "ExpenseHead saved successfully", $"ExpenseHead {model.ExpenseHeadId} '{model.ExpenseHeadName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Party: {ex.Message}");
                LogException("Party", "Save", ex);
                return false;
            }
        }
        protected override void ClearItem()
        {
            Item = new ExpenseHeadModel();
        }
        protected override async Task<bool> DeleteItemAsync(ExpenseHeadModel model)
        {
            try
            {
                StartStatusMessage("Deleting ExpenseHead...");
                await Task.Delay(100);
                await ExpenseHeadService.DeleteExpenseHeadAsync(model);
                ClearItem();
                await ExpenseHeadListViewModel.RefreshAsync();
                EndStatusMessage("ExpenseHead deleted");
                LogWarning("ExpenseHead", "Delete", "ExpenseHead deleted", $"ExpenseHead {model.ExpenseHeadId} '{model.ExpenseHeadName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting ExpenseHead: {ex.Message}");
                LogException("ExpenseHead", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current ExpenseHead?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<ExpenseHeadModel>> GetValidationConstraints(ExpenseHeadModel model)
        {
            yield return new RequiredConstraint<ExpenseHeadModel>("Name", m => m.ExpenseHeadName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(ExpenseHeadDetailsViewModel sender, string message, ExpenseHeadModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.ExpenseHeadId == current?.ExpenseHeadId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await ExpenseHeadService.GetExpenseHeadAsync(current.ExpenseHeadId);
                                    item = item ?? new ExpenseHeadModel { ExpenseHeadId = current.ExpenseHeadId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This ExpenseHead has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("ExpenseHead", "Handle Changes", ex);
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

        private async void OnListMessage(ExpenseHeadListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<ExpenseHeadModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.ExpenseHeadId == current.ExpenseHeadId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await ExpenseHeadService.GetExpenseHeadAsync(current.ExpenseHeadId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("ExpenseHead", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This ExpenseHead has been deleted externally");
            });
        }
    }
}
