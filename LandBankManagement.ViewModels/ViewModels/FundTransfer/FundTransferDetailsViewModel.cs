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
    public class FundTransferDetailsViewModel : GenericDetailsViewModel<FundTransferModel>
    {
        public IDropDownService DropDownService { get; }
        public IFundTransferService FundTransferService { get; }
        public IFilePickerService FilePickerService { get; }
        private ObservableCollection<ComboBoxOptions> _companyOptions = null;
        public ObservableCollection<ComboBoxOptions> CompanyOptions
        {
            get => _companyOptions;
            set => Set(ref _companyOptions, value);
        }

        private ObservableCollection<ComboBoxOptionsStringId> _fromCashOptions = null;
        public ObservableCollection<ComboBoxOptionsStringId> FromCashOptions
        {
            get => _fromCashOptions;
            set => Set(ref _fromCashOptions, value);
        }
        private ObservableCollection<ComboBoxOptionsStringId> _fromBankOptions = null;
        public ObservableCollection<ComboBoxOptionsStringId> FromBankOptions
        {
            get => _fromBankOptions;
            set => Set(ref _fromBankOptions, value);
        } 
        private ObservableCollection<ComboBoxOptionsStringId> _toCashOptions = null;
        public ObservableCollection<ComboBoxOptionsStringId> ToCashOptions
        {
            get => _toCashOptions;
            set => Set(ref _toCashOptions, value);
        }
        private ObservableCollection<ComboBoxOptionsStringId> _toBankOptions = null;
        public ObservableCollection<ComboBoxOptionsStringId> ToBankOptions
        {
            get => _toBankOptions;
            set => Set(ref _toBankOptions, value);
        }
             

        private bool _fromCashVisibility;
        public bool FromCashVisibility
        {
            get => _fromCashVisibility;
            set => Set(ref _fromCashVisibility, value);
        }

        private bool _fromBankVisibility;
        public bool FromBankVisibility
        {
            get => _fromBankVisibility;
            set => Set(ref _fromBankVisibility, value);
        }
        private bool _toCashVisibility;
        public bool ToCashVisibility
        {
            get => _toCashVisibility;
            set => Set(ref _toCashVisibility, value);
        }

        private bool _toBankVisibility;
        public bool ToBankVisibility
        {
            get => _toBankVisibility;
            set => Set(ref _toBankVisibility, value);
        }
       
        private bool _isToCashChecked;
        public bool IsToCashChecked
        {
            get => _isToCashChecked;
            set => Set(ref _isToCashChecked, value);
        }
        private bool _isFromCashChecked;
        public bool IsFromCashChecked
        {
            get => _isFromCashChecked;
            set => Set(ref _isFromCashChecked, value);
        }
        private bool _isToBankChecked;
        public bool IsToBankChecked
        {
            get => _isToBankChecked;
            set => Set(ref _isToBankChecked, value);
        }
        private bool _isFromBankChecked;
        public bool IsFromBankChecked
        {
            get => _isFromBankChecked;
            set => Set(ref _isFromBankChecked, value);
        }

        private string _selectedFromBankId = "0";
        public string SelectedFromBankId
        {
            get => _selectedFromBankId;
            set=> Set(ref _selectedFromBankId, value);
        }

        private string _selectedFromCashId = "0";
        public string SelectedFromCashId
        {
            get => _selectedFromCashId;
            set => Set(ref _selectedFromCashId, value);
        }
        private string _selectedToBankId = "0";
        public string SelectedToBankId
        {
            get => _selectedToBankId;
            set => Set(ref _selectedToBankId, value);
        }

        private string _selectedToCashId = "0";
        public string SelectedToCashId
        {
            get => _selectedToCashId;
            set => Set(ref _selectedToCashId, value);
        }
        private int currentFromCompanyId { get; set; } = 0; 
        private int currentToCompanyId { get; set; } = 0;
        private FundTransferViewModel FundTransferViewModel { get; set; }
        private bool IsProcessing = false;
        public FundTransferDetailsViewModel(IDropDownService dropDownService, IFundTransferService fundTransferService, IFilePickerService filePickerService, ICommonServices commonServices, FundTransferViewModel fundTransferViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            FundTransferService = fundTransferService;
            FundTransferViewModel = fundTransferViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New FundTransfer" : TitleEdit;
        public string TitleEdit = "FundTransfer";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            IsEditMode = true;
            Item = new FundTransferModel() {  PayeePaymentType = 1,ReceiverPaymentType=1,DateOfPayment=DateTimeOffset.Now };
            GetDropdowns();
            defaultSettings();
            OnFromCashRadioChecked();
            OnToCashRadioChecked();
        }
        public void defaultSettings()
        {
            
            if (Item.PayeePaymentType == 1)
            {
                IsFromCashChecked = true;
                IsFromBankChecked = false;
            }
            else
            {
                IsFromCashChecked = false;
                IsFromBankChecked = true;
            }
            if (Item.ReceiverPaymentType == 1)
            {
                IsToCashChecked = true;
                IsToBankChecked = false;
            }
            else
            {
                IsToCashChecked = false;
                IsToBankChecked = true;
            }

        }
        private async void GetDropdowns()
        {
            FundTransferViewModel.ShowProgressRing();
               CompanyOptions = await DropDownService.GetCompanyOptions();           
           // BankOptions =await DropDownService.GetBankOptions();
            FundTransferViewModel.HideProgressRing();
        }
        private async Task<ObservableCollection<ComboBoxOptionsStringId>> GetBankList(int id,string type) {
            if (type == "bank") {
                var items = await DropDownService.GetBankOptionsByCompany(id);
                var bankList = new ObservableCollection<ComboBoxOptionsStringId>();
                foreach (var obj in items)
                {
                    bankList.Add(new ComboBoxOptionsStringId { Id = obj.Id.ToString(), Description = obj.Description });
                }
                return bankList;
            }
            else
            {
                var items = await DropDownService.GetCashOptionsByCompany(id);
                var bankList = new ObservableCollection<ComboBoxOptionsStringId>();
                foreach (var obj in items)
                {
                    bankList.Add(new ComboBoxOptionsStringId { Id = obj.Id.ToString(), Description = obj.Description });
                }
                return bankList;
            }
        }

        public async Task LoadFromBankAndCompany()
        {
            if (Convert.ToInt32( Item.PayeeId) == 0 || Convert.ToInt32(Item.PayeeId) == currentFromCompanyId)
                return;
            var payeeid = Convert.ToInt32(Item.PayeeId);
            SelectedFromCashId = "0";
            SelectedFromBankId = "0";
           
            FromBankOptions =await GetBankList(payeeid, "bank");
            FromCashOptions = await GetBankList(payeeid, "cash"); 
            
            currentFromCompanyId = payeeid;
        }
        public async Task LoadToBankAndCompany()
        {
            if (Convert.ToInt32(Item.ReceiverId) == 0 || Convert.ToInt32(Item.ReceiverId) == currentToCompanyId)
                return;
            var receiverId = Convert.ToInt32(Item.ReceiverId);
            SelectedToCashId = "0";
            SelectedToBankId = "0";
            ToCashOptions = await GetBankList(receiverId, "cash");
            ToBankOptions = await GetBankList(receiverId, "bank");
          
            currentToCompanyId = receiverId;
        }

        public ICommand FromCashCheckedCommand => new RelayCommand(OnFromCashRadioChecked);
        virtual protected void OnFromCashRadioChecked()
        {
            if (IsFromCashChecked)
            {
                FromCashVisibility = true;
                FromBankVisibility = false;
            }
            else
            {
                FromCashVisibility = false;
                FromBankVisibility = true;
            }
        } 
        public ICommand ToCashCheckedCommand => new RelayCommand(OnToCashRadioChecked);
        virtual protected void OnToCashRadioChecked()
        {
            if (IsToCashChecked)
            {
                ToCashVisibility = true;
                ToBankVisibility = false;
            }
            else
            {
                ToCashVisibility = false;
                ToBankVisibility = true;
            }
        }

        public async void LoadSelectedFundTransfer(int id)
        {
            var model = await FundTransferService.GetFundTransferAsync(id);
            Item.PayeeId = model.PayeeId;
            Item.ReceiverId = model.ReceiverId;
            Item.PayeePaymentType = model.PayeePaymentType;
            Item.ReceiverPaymentType = model.ReceiverPaymentType;
            await LoadFromBankAndCompany();
            await LoadToBankAndCompany();
            defaultSettings();
            OnFromCashRadioChecked();
            OnToCashRadioChecked();
            Item = model;
            if (Item.PayeeBankId > 0)
                SelectedFromBankId = Item.PayeeBankId.ToString();
            else
                SelectedFromCashId = Item.PayeeCashId.ToString();
            if (Item.ReceiverBankId > 0)
                SelectedToBankId = Item.ReceiverBankId.ToString();
            else
                SelectedToCashId = Item.ReceiverCashId.ToString();
        }

            public void Subscribe()
        {
            MessageService.Subscribe<FundTransferDetailsViewModel, FundTransferModel>(this, OnDetailsMessage);
            MessageService.Subscribe<FundTransferListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        
        protected override async Task<bool> SaveItemAsync(FundTransferModel model)
        {
            try
            {
                if (IsProcessing)
                    return false;
                IsProcessing = true;
                FundTransferViewModel.ShowProgressRing();
                if (IsFromCashChecked)
                    model.PayeePaymentType = 1;
                else
                    model.PayeePaymentType = 2;

                if (IsToCashChecked)
                    model.ReceiverPaymentType = 1;
                else
                    model.ReceiverPaymentType = 2;

                model.PayeeBankId = Convert.ToInt32(SelectedFromBankId);
                model.PayeeCashId = Convert.ToInt32(SelectedFromCashId);
                model.ReceiverBankId = Convert.ToInt32(SelectedToBankId);
                model.ReceiverCashId = Convert.ToInt32(SelectedToCashId);

                StartStatusMessage("Saving FundTransfer...");
                
                if (model.FundTransferId <= 0)
                    await FundTransferService.AddFundTransferAsync(model);
                else
                    await FundTransferService.UpdateFundTransferAsync(model);
                ShowPopup("success", "Fund Transfer is Saved");
                IsProcessing = false;
                EndStatusMessage("FundTransfer saved");
                LoadSelectedFundTransfer(model.FundTransferId);
                LogInformation("FundTransfer", "Save", "FundTransfer saved successfully", $"FundTransfer {model.FundTransferId}  was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                IsProcessing = false;
                ShowPopup("error", "Fund Transfer is not Saved");
                StatusError($"Error saving FundTransfer: {ex.Message}");
                LogException("FundTransfer", "Save", ex);
                return false;
            }
            finally {
                FundTransferViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = new FundTransferModel() { PayeePaymentType = 1, ReceiverPaymentType = 1 ,DateOfPayment=DateTimeOffset.Now};
            defaultSettings();
            SelectedFromBankId="0";
            SelectedFromCashId = "0";
            SelectedToBankId = "0";
            SelectedToCashId = "0";
        }
        protected override async Task<bool> DeleteItemAsync(FundTransferModel model)
        {
            try
            {
                StartStatusMessage("Deleting FundTransfer...");
                FundTransferViewModel.ShowProgressRing();
                await FundTransferService.DeleteFundTransferAsync(model);
                ShowPopup("success", "Fund Transfer is deleted");
                ClearItem();
                EndStatusMessage("FundTransfer deleted");
                LogWarning("FundTransfer", "Delete", "FundTransfer deleted", $"Taluk {model.FundTransferId}  was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "Fund Transfer is not deleted");
                StatusError($"Error deleting FundTransfer: {ex.Message}");
                LogException("FundTransfer", "Delete", ex);
                return false;
            }
            finally {
                FundTransferViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.FundTransferId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current FundTransfer?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<FundTransferModel>> GetValidationConstraints(FundTransferModel model)
        {
            yield return new ValidationConstraint<FundTransferModel>("From Company", m => m.PayeeId!=null && Convert.ToInt32(m.PayeeId)>0);
            yield return new ValidationConstraint<FundTransferModel>("From Bank/Cash", m => Convert.ToInt32(SelectedFromBankId) >0 || Convert.ToInt32(SelectedFromCashId)>0);
            yield return new ValidationConstraint<FundTransferModel>("Amount should not be empty", m => ValidateAmount(m));
            yield return new ValidationConstraint<FundTransferModel>("To Company", m => m.ReceiverId!=null && Convert.ToInt32(m.ReceiverId) > 0);
            yield return new ValidationConstraint<FundTransferModel>("To Bank/Cash", m => Convert.ToInt32(SelectedToBankId) >0 || Convert.ToInt32(SelectedToCashId) >0);
           
        }
        private bool ValidateAmount(FundTransferModel model)
        {
            return string.IsNullOrEmpty(model.Amount) ? false : Convert.ToDecimal(model.Amount) > 0;
        }
        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(FundTransferDetailsViewModel sender, string message, FundTransferModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.FundTransferId == current?.FundTransferId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await FundTransferService.GetFundTransferAsync(current.FundTransferId);
                                    item = item ?? new FundTransferModel { FundTransferId = current.FundTransferId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This FundTransfer has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("FundTransfer", "Handle Changes", ex);
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

        private async void OnListMessage(FundTransferListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<FundTransferModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.FundTransferId == current.FundTransferId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await FundTransferService.GetFundTransferAsync(current.FundTransferId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("FundTransfer", "Handle Ranges Deleted", ex);
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
