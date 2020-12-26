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
        IDealService DealService { get; }
        public IFilePickerService FilePickerService { get; }
        public ReceiptsListViewModel ReceiptsListViewModel { get; }
        private ObservableCollection<ComboBoxOptions> _companyOptions = null;
        public ObservableCollection<ComboBoxOptions> CompanyOptions
        {
            get => _companyOptions;
            set => Set(ref _companyOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _bankOptions = null;
        public ObservableCollection<ComboBoxOptions> BankOptions
        {
            get => _bankOptions;
            set => Set(ref _bankOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _dealOptions = null;
        public ObservableCollection<ComboBoxOptions> DealOptions
        {
            get => _dealOptions;
            set => Set(ref _dealOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _partyOptions = null;
        public ObservableCollection<ComboBoxOptions> PartyOptions
        {
            get => _partyOptions;
            set => Set(ref _partyOptions, value);
        }

        private ObservableCollection<DealPartiesModel> _dealParties;
        public ObservableCollection<DealPartiesModel> DealParties
        {
            get => _dealParties;
            set => Set(ref _dealParties, value);
        }

        private bool _isCashChecked;
        public bool IsCashChecked
        {
            get => _isCashChecked;
            set => Set(ref _isCashChecked, value);
        }
        private bool _isBankChecked;
        public bool IsBankChecked
        {
            get => _isBankChecked;
            set => Set(ref _isBankChecked, value);
        }

        private ReceiptsViewModel ReceiptsViewModel { get; set; }
        public ReceiptsDetailsViewModel(IDropDownService dropDownService, IReceiptService receiptService, IFilePickerService filePickerService, ICommonServices commonServices, ReceiptsListViewModel villageListViewModel, ReceiptsViewModel receiptsViewModel, IDealService dealService) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            ReceiptsService = receiptService;
            ReceiptsListViewModel = villageListViewModel;
            ReceiptsViewModel = receiptsViewModel;
            DealService= dealService;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Receipts" : TitleEdit;
        public string TitleEdit => Item == null ? "Receipts" : $"{Item.PayeeId}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new ReceiptModel { DealId=0,PaymentTypeId=0,PayeeId=0,DepositBankId=0};
            IsEditMode = true;
           await GetDropdowns();
        }
        private async Task GetDropdowns()
        {
            ReceiptsViewModel.ShowProgressRing();
           // PartyOptions = await DropDownService.GetPartyOptions();
            BankOptions = await DropDownService.GetBankOptions();
            CompanyOptions = await DropDownService.GetCompanyOptions();
            DealOptions = await DropDownService.GetDealOptions();
            ReceiptsViewModel.HideProgressRing();           
        }

        public async void LoadDealParties() {
            DealParties =await DealService.GetDealParties(Item.DealId);
        }

        public async void LoadSelectedReceipt(int id) {
            ReceiptsViewModel.ShowProgressRing();
            var model = await ReceiptsService.GetReceiptAsync(id);
            Item = model;
            if (model.PaymentTypeId == 1)
                IsCashChecked = true;
            else
                IsBankChecked = true;
            LoadDealParties();
            ReceiptsViewModel.HideProgressRing();
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

        protected override async Task<bool> SaveItemAsync(ReceiptModel model)
        {
            try
            {
                StartStatusMessage("Saving Receipts...");
                ReceiptsViewModel.ShowProgressRing();

                if (IsCashChecked)
                    model.PaymentTypeId = 1;
                else
                    model.PaymentTypeId = 2;

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
            finally {
                ReceiptsViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = new ReceiptModel { DealId = 0, PaymentTypeId = 0, PayeeId = 0, DepositBankId = 0 };
            DealParties = new ObservableCollection<DealPartiesModel>();
        }
        protected override async Task<bool> DeleteItemAsync(ReceiptModel model)
        {
            try
            {
                StartStatusMessage("Deleting Receipts...");
                ReceiptsViewModel.ShowProgressRing();
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
            finally {
                ReceiptsViewModel.HideProgressRing();
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
