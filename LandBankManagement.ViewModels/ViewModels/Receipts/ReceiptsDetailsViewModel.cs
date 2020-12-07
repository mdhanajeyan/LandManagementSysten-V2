using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class ReceiptsDetailsViewModel : GenericDetailsViewModel<ReceiptModel>
    {
        public IDropDownService DropDownService { get; }
        public IReceiptService ReceiptsService { get; }
        public IFilePickerService FilePickerService { get; }
        public ReceiptsListViewModel ReceiptsListViewModel { get; }
        private ObservableCollection<ComboBoxOptions> _talukOptions = null;
        public ObservableCollection<ComboBoxOptions> TalukOptions
        {
            get => _talukOptions;
            set => Set(ref _talukOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _hobliOptions = null;
        public ObservableCollection<ComboBoxOptions> HobliOptions
        {
            get => _hobliOptions;
            set => Set(ref _hobliOptions, value);
        }
        public ReceiptsDetailsViewModel(IDropDownService dropDownService, IReceiptService receiptService, IFilePickerService filePickerService, ICommonServices commonServices, ReceiptsListViewModel villageListViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            ReceiptsService = receiptService;
            ReceiptsListViewModel = villageListViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Receipts" : TitleEdit;
        public string TitleEdit => Item == null ? "Receipts" : $"{Item.PayeeId}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new ReceiptModel();
            IsEditMode = true;
            GetTaluks();
            GetHobli();
        }
        private void GetTaluks()
        {
            var models = DropDownService.GetTalukOptions();
            TalukOptions = models;

        }
        private void GetHobli()
        {
            var models = DropDownService.GetHobliOptions();
            HobliOptions = models;

        }

        public void Subscribe()
        {
            MessageService.Subscribe<ReceiptsDetailsViewModel, ReceiptModel>(this, OnDetailsMessage);
            MessageService.Subscribe<ReceiptsListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        //public ExpenseHeadDetailsArgs CreateArgs()
        //{
        //    return new ExpenseHeadDetailsArgs
        //    {
        //        ExpenseHeadId = Item?.ExpenseHeadId ?? 0
        //    };
        //}


        protected override async Task<bool> SaveItemAsync(ReceiptModel model)
        {
            try
            {
                StartStatusMessage("Saving Receipts...");

                if (model.ReceiptId <= 0)
                    await ReceiptsService.AddReceiptAsync(model);
                else
                    await ReceiptsService.UpdateReceiptAsync(model);
                await ReceiptsListViewModel.RefreshAsync();
                EndStatusMessage("Receipts saved");
                LogInformation("Receipts", "Save", "Receipts saved successfully", $"Receipts {model.ReceiptId} '{model.PayeeId}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Receipts: {ex.Message}");
                LogException("Receipts", "Save", ex);
                return false;
            }
        }
        protected override void ClearItem()
        {
            Item = new ReceiptModel() { PayeeId = 1 };
        }
        protected override async Task<bool> DeleteItemAsync(ReceiptModel model)
        {
            try
            {
                StartStatusMessage("Deleting Receipts...");

                await ReceiptsService.DeleteReceiptAsync(model);
                ClearItem();
                await ReceiptsListViewModel.RefreshAsync();
                EndStatusMessage("Receipts deleted");
                LogWarning("Receipts", "Delete", "Receipts deleted", $"Taluk {model.ReceiptId} '{model.PayeeId}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Receipts: {ex.Message}");
                LogException("Receipts", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Receipts?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<ReceiptModel>> GetValidationConstraints(ReceiptModel model)
        {
            yield return new RequiredConstraint<ReceiptModel>("Name", m => m.PayeeId);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(ReceiptsDetailsViewModel sender, string message, ReceiptModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.ReceiptId == current?.ReceiptId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await ReceiptsService.GetReceiptAsync(current.ReceiptId);
                                    item = item ?? new ReceiptModel { ReceiptId = current.ReceiptId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Receipts has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Receipts", "Handle Changes", ex);
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

        private async void OnListMessage(ReceiptsListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<ReceiptModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.ReceiptId == current.ReceiptId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await ReceiptsService.GetReceiptAsync(current.ReceiptId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Receipts", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This Taluk has been deleted externally");
            });
        }
    }
}
