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
      
        //private ObservableCollection<ComboBoxOptions> _cashOptions = null;
        //public ObservableCollection<ComboBoxOptions> CashOptions
        //{
        //    get => _cashOptions;
        //    set => Set(ref _cashOptions, value);
        //}
        private ObservableCollection<ComboBoxOptions> _bankOptions = null;
        public ObservableCollection<ComboBoxOptions> BankOptions
        {
            get => _bankOptions;
            set => Set(ref _bankOptions, value);
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
        private FundTransferViewModel FundTransferViewModel { get; set; }
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
            Item = new FundTransferModel() {  PayeePaymentType = 1,ReceiverPaymentType=1,DateOfPayment=DateTimeOffset.Now };
            GetDropdowns();
            defaultSettings();
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
            BankOptions =await DropDownService.GetBankOptions();
            FundTransferViewModel.HideProgressRing();
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
                FundTransferViewModel.ShowProgressRing();
                if (IsFromCashChecked)
                    model.PayeePaymentType = 1;
                else
                    model.PayeePaymentType = 2;

                if (IsToCashChecked)
                    model.ReceiverPaymentType = 1;
                else
                    model.ReceiverPaymentType = 2;

                StartStatusMessage("Saving FundTransfer...");

                if (model.FundTransferId <= 0)
                    await FundTransferService.AddFundTransferAsync(model);
                else
                    await FundTransferService.UpdateFundTransferAsync(model);
                EndStatusMessage("FundTransfer saved");
                LogInformation("FundTransfer", "Save", "FundTransfer saved successfully", $"FundTransfer {model.FundTransferId}  was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
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
        }
        protected override async Task<bool> DeleteItemAsync(FundTransferModel model)
        {
            try
            {
                StartStatusMessage("Deleting FundTransfer...");
                FundTransferViewModel.ShowProgressRing();
                await FundTransferService.DeleteFundTransferAsync(model);
                ClearItem();
                EndStatusMessage("FundTransfer deleted");
                LogWarning("FundTransfer", "Delete", "FundTransfer deleted", $"Taluk {model.FundTransferId}  was deleted.");
                return true;
            }
            catch (Exception ex)
            {
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
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current FundTransfer?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<FundTransferModel>> GetValidationConstraints(FundTransferModel model)
        {
            yield return new RequiredConstraint<FundTransferModel>("Name", m => m.FundTransferId);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

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
