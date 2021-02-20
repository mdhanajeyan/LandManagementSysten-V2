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
        private ObservableCollection<ComboBoxOptionsStringId> _bankOptions = null;
        public ObservableCollection<ComboBoxOptionsStringId> BankOptions
        {
            get => _bankOptions;
            set => Set(ref _bankOptions, value);
        } 
        private ObservableCollection<ComboBoxOptionsStringId> _cashOptions = null;
        public ObservableCollection<ComboBoxOptionsStringId> CashOptions
        {
            get => _cashOptions;
            set => Set(ref _cashOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _dealOptions = null;
        public ObservableCollection<ComboBoxOptions> DealOptions
        {
            get => _dealOptions;
            set => Set(ref _dealOptions, value);
        }

        private ObservableCollection<ComboBoxOptionsStringId> _partyOptions = null;
        public ObservableCollection<ComboBoxOptionsStringId> PartyOptions
        {
            get => _partyOptions;
            set => Set(ref _partyOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _dealParties;
        public ObservableCollection<ComboBoxOptions> DealParties
        {
            get => _dealParties;
            set => Set(ref _dealParties, value);
        }

        private bool _cashVisibility;
        public bool CashVisibility
        {
            get => _cashVisibility;
            set => Set(ref _cashVisibility, value);
        }

        private bool _bankVisibility;
        public bool BankVisibility
        {
            get => _bankVisibility;
            set => Set(ref _bankVisibility, value);
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
        private string _selectedBankId ="0";
        public string SelectedBankId
        {
            get => _selectedBankId;
            set => Set(ref _selectedBankId, value);
        }

        private string _selectedCashId="0";
        public string SelectedCashId
        {
            get => _selectedCashId;
            set => Set(ref _selectedCashId, value);
        }

        private string _selectedPartyId = "0";
        public string SelectedPartyId
        {
            get => _selectedPartyId;
            set => Set(ref _selectedPartyId, value);
        }

        private ReceiptsViewModel ReceiptsViewModel { get; set; }
        private int currentCompanyId { get; set; } = 0;
        private int currentDealId { get; set; } = 0;
        private bool IsProcessing = false;
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
            Item = new ReceiptModel { DealId="0",PaymentTypeId=0,PayeeId="0",DepositBankId="0",DateOfPayment=DateTime.Now};
            IsEditMode = true;
           await GetDropdowns();
        }
        private async Task GetDropdowns()
        {
            ReceiptsViewModel.ShowProgressRing();
           // PartyOptions = await DropDownService.GetPartyOptions();
           // BankOptions = await DropDownService.GetBankOptions();
            CompanyOptions = await DropDownService.GetCompanyOptions();
            DealOptions = await DropDownService.GetDealOptions();
            ReceiptsViewModel.HideProgressRing();           
        }

        private async Task<ObservableCollection<ComboBoxOptionsStringId>> GetBankList(int id, string type)
        {
            if (type == "bank")
            {
                var items = await DropDownService.GetBankOptionsByCompany(id);
                var bankList = new ObservableCollection<ComboBoxOptionsStringId>();
                foreach (var obj in items)
                {
                    bankList.Add(new ComboBoxOptionsStringId { Id = obj.Id.ToString(), Description = obj.Description });
                }
                return bankList;
            }
            if(type == "cash")
            {
                var items = await DropDownService.GetCashOptionsByCompany(id);
                var bankList = new ObservableCollection<ComboBoxOptionsStringId>();
                foreach (var obj in items)
                {
                    bankList.Add(new ComboBoxOptionsStringId { Id = obj.Id.ToString(), Description = obj.Description });
                }
                return bankList;
            }
            else  {
                var items = await DropDownService.GetDealPartiesOptions(id);
                var partyList = new ObservableCollection<ComboBoxOptionsStringId>();
                foreach (var obj in items)
                {
                    partyList.Add(new ComboBoxOptionsStringId { Id = obj.Id.ToString(), Description = obj.Description });
                }
                return partyList;
            }
        }
        public async Task LoadBankAndCompany()
        {
            var payeeid = Convert.ToInt32(Item.PayeeId);
            if (payeeid == 0|| payeeid == currentCompanyId)
                return;
            
            CashOptions = await GetBankList(payeeid, "cash");
            BankOptions = await GetBankList(payeeid, "bank");
            SelectedCashId = "0";
            SelectedBankId = "0";
            currentCompanyId = payeeid;
        }

        public async Task LoadDealParties(int dealId) {
            if (dealId == 0 || dealId== currentDealId)
                return;
            PartyOptions = await GetBankList(dealId,"party");
            currentDealId = dealId;
           // DealParties =await DealService.GetDealParties(Item.DealId);
        }

        public async void LoadSelectedReceipt(int id) {
            ReceiptsViewModel.ShowProgressRing();
            var model = await ReceiptsService.GetReceiptAsync(id);
            Item.PayeeId = model.PayeeId;
           await LoadBankAndCompany();
           await LoadDealParties(Convert.ToInt32(model.DealId));
            if (model.PaymentTypeId == 1)
            {
                IsCashChecked = true;
                OnCashRadioChecked();
                //if (CashOptions.Count>1)
                //SelectedCashId = Item.DepositCashId;
            }
            else
            {
                IsBankChecked = true;
                OnCashRadioChecked();
                //if (BankOptions.Count > 1)
                //    SelectedBankId = Item.DepositBankId;
            }
             Item = model;
            if(Convert.ToInt32( Item.DepositCashId)>0)
            SelectedCashId = Item.DepositCashId;
            else
            SelectedBankId = Item.DepositBankId;

            SelectedPartyId = Item.PartyId;
            ReceiptsViewModel.HideProgressRing();
        }

        public ICommand CashCheckedCommand => new RelayCommand(OnCashRadioChecked);
        virtual protected void OnCashRadioChecked()
        {
            if (IsCashChecked)
            {
                CashVisibility = true;
                BankVisibility = false;
            }
            else
            {
                CashVisibility = false;
                BankVisibility = true;
            }
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
                if (IsProcessing)
                    return false;
                IsProcessing = true;
                StartStatusMessage("Saving Receipts...");
                ReceiptsViewModel.ShowProgressRing();

                if (IsCashChecked)
                    model.PaymentTypeId = 1;
                else
                    model.PaymentTypeId = 2;

                model.DepositBankId =SelectedBankId;
                model.DepositCashId = SelectedCashId;
                model.PartyId =SelectedPartyId;
                if (model.ReceiptId <= 0)
                    await ReceiptsService.AddReceiptAsync(model);
                else
                    await ReceiptsService.UpdateReceiptAsync(model);
                await ReceiptsListViewModel.RefreshAsync();
                IsProcessing = false;
                ShowPopup("success", "Receipt is Saved");
                EndStatusMessage("Receipts saved");
                LoadSelectedReceipt(model.ReceiptId);
                LogInformation("Receipts", "Save", "Receipts saved successfully", $"Receipts {model.ReceiptId} '{model.PayeeId}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                IsProcessing = false;
                ShowPopup("error", "Receipt is  not Saved");
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
            Item = new ReceiptModel { DealId = "0", PaymentTypeId = 0, PayeeId = "0", DepositBankId = "0", DateOfPayment = DateTime.Now };
            PartyOptions = new ObservableCollection<ComboBoxOptionsStringId>();
           // DealParties = new ObservableCollection<DealPartiesModel>();
            SelectedBankId = "0";
            SelectedCashId = "0";
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
                ShowPopup("success", "Receipt is deleted");
                EndStatusMessage("Receipts deleted");
                LogWarning("Receipts", "Delete", "Receipts deleted", $"Taluk {model.ReceiptId} '{model.PayeeId}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "Receipt is not deleted");
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
            if (Item.ReceiptId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Receipts?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<ReceiptModel>> GetValidationConstraints(ReceiptModel model)
        {
            yield return new ValidationConstraint<ReceiptModel>("Company must be selected", m =>Convert.ToInt32( m.PayeeId) > 0);
            yield return new ValidationConstraint<ReceiptModel>("Deal Name must be selected", m => Convert.ToInt32(m.DealId) > 0);
            yield return new ValidationConstraint<ReceiptModel>("Deposit Bank / Cash must be selected", m => Convert.ToInt32(SelectedBankId) > 0 || Convert.ToInt32(SelectedCashId) > 0);
            yield return new ValidationConstraint<ReceiptModel>("Amount", m => ValidateAmount( m));
        }
        private bool ValidateAmount(ReceiptModel model)
        {
            return string.IsNullOrEmpty(model.Amount) ? false : Convert.ToDecimal(model.Amount) > 0;
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
