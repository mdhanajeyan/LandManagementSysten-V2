using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;
using Windows.UI.Xaml;

namespace LandBankManagement.ViewModels
{
   public class PaymentsDetailsViewModel : GenericDetailsViewModel<PaymentModel>
    {
        public IDropDownService DropDownService { get; }
        public IPaymentService PaymentsService { get; }
        public IFilePickerService FilePickerService { get; }
        public PaymentsListViewModel PaymentsListViewModel { get; }
        private ObservableCollection<ComboBoxOptions> _companyOptions = null;
        public ObservableCollection<ComboBoxOptions> CompanyOptions
        {
            get => _companyOptions;
            set => Set(ref _companyOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _expenseOptions = null;
        public ObservableCollection<ComboBoxOptions> ExpenseOptions
        {
            get => _expenseOptions;
            set => Set(ref _expenseOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _partyOptions = null;
        public ObservableCollection<ComboBoxOptions> PartyOptions
        {
            get => _partyOptions;
            set => Set(ref _partyOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _propertyOptions = null;
        public ObservableCollection<ComboBoxOptions> PropertyOptions
        {
            get => _propertyOptions;
            set => Set(ref _propertyOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _documentTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> DocumentTypeOptions
        {
            get => _documentTypeOptions;
            set => Set(ref _documentTypeOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _cashOptions = null;
        public ObservableCollection<ComboBoxOptions> CashOptions
        {
            get => _cashOptions;
            set => Set(ref _cashOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _bankOptions = null;
        public ObservableCollection<ComboBoxOptions> BankOptions
        {
            get => _bankOptions;
            set => Set(ref _bankOptions, value);
        }

        private Visibility _expenseVisibility ;
        public Visibility ExpenseVisibility
        {
            get => _expenseVisibility;
            set => Set(ref _expenseVisibility, value);
        }

        private Visibility _partyVisibility;
        public Visibility PartyVisibility
        {
            get => _partyVisibility;
            set => Set(ref _partyVisibility, value);
        }

        private Visibility _cashVisibility;
        public Visibility CashVisibility
        {
            get => _cashVisibility;
            set => Set(ref _cashVisibility, value);
        }

        private Visibility _bankVisibility;
        public Visibility BankVisibility
        {
            get => _bankVisibility;
            set => Set(ref _bankVisibility, value);
        }

        private bool _isExpenseChecked;
        public bool IsExpenseChecked
        {
            get => _isExpenseChecked;
            set => Set(ref _isExpenseChecked, value);
        }
        private bool _isPartyChecked;
        public bool IsPartyChecked
        {
            get => _isPartyChecked;
            set => Set(ref _isPartyChecked, value);
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

        public PaymentsDetailsViewModel(IDropDownService dropDownService, IPaymentService villageService, IFilePickerService filePickerService, ICommonServices commonServices, PaymentsListViewModel villageListViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            PaymentsService = villageService;
            PaymentsListViewModel = villageListViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Payments" : TitleEdit;
        public string TitleEdit  ="Payments";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new PaymentModel() { PayeeTypeId=1,PaymentTypeId=1};
            Item.DateOfPayment = DateTime.Now;
            GetDropdowns();
            defaultSettings();
        }
        public void defaultSettings() {
            if (Item.PayeeTypeId == 1)
            {
                IsExpenseChecked = true;
                IsPartyChecked = false;
            }
            else
            {
                IsExpenseChecked = false;
                IsPartyChecked = true;
            }
            if (Item.PaymentTypeId == 1)
            {
                IsCashChecked = true;
                IsBankChecked = false;
            }
            else
            {
                IsCashChecked = false;
                IsBankChecked = true;
            }

            OnExpenseRadioChecked();
            OnCashRadioChecked();

           
        }
        private void GetDropdowns()
        {
           CompanyOptions = DropDownService.GetCompanyOptions();
            ExpenseOptions = DropDownService.GetExpenseHeadOptions();
            PartyOptions= DropDownService.GetPartyOptions();
            PropertyOptions= DropDownService.GetPropertyOptions();
            DocumentTypeOptions = DropDownService.GetDocumentTypeOptions();
            CashOptions = DropDownService.GetCashOptions();
            BankOptions = DropDownService.GetBankOptions();
        }
        

        public ICommand ExpenseCheckedCommand => new RelayCommand(OnExpenseRadioChecked);
        virtual protected void OnExpenseRadioChecked()
        {
            if (IsExpenseChecked)
            {
                ExpenseVisibility = Visibility.Visible;
                PartyVisibility = Visibility.Collapsed;
            }
            else {
                ExpenseVisibility = Visibility.Collapsed;
                PartyVisibility = Visibility.Visible;
            }            
        }
        public ICommand CashCheckedCommand => new RelayCommand(OnCashRadioChecked);
        virtual protected void OnCashRadioChecked()
        {
            if (IsCashChecked)
            {
                CashVisibility = Visibility.Visible;
                BankVisibility = Visibility.Collapsed;
            }
            else
            {
                CashVisibility = Visibility.Collapsed;
                BankVisibility = Visibility.Visible;
            }
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PaymentsDetailsViewModel, PaymentModel>(this, OnDetailsMessage);
            MessageService.Subscribe<PaymentsListViewModel>(this, OnListMessage);
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


        protected override async Task<bool> SaveItemAsync(PaymentModel model)
        {
            try
            {
                if (IsExpenseChecked)
                    model.PayeeTypeId = 1;
                else
                    model.PayeeTypeId = 2;
                if (IsCashChecked)
                    model.PayeeTypeId = 1;
                else
                    model.PaymentTypeId = 2;

                StartStatusMessage("Saving Payments...");
                await Task.Delay(100);
                if (model.PaymentId <= 0)
                    await PaymentsService.AddPaymentAsync(model);
                else
                    await PaymentsService.UpdatePaymentAsync(model);
                await PaymentsListViewModel.RefreshAsync();
                EndStatusMessage("Payments saved");
                LogInformation("Payments", "Save", "Payments saved successfully", $"Payments {model.PaymentId}  was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Payments: {ex.Message}");
                LogException("Payments", "Save", ex);
                return false;
            }
        }
        protected override void ClearItem()
        {
            Item = new PaymentModel() { PayeeTypeId = 1, PaymentTypeId = 1 };
            defaultSettings();
            Item.DateOfPayment = DateTime.Now.AddMinutes(1);
        }
        protected override async Task<bool> DeleteItemAsync(PaymentModel model)
        {
            try
            {
                StartStatusMessage("Deleting Payments...");
                await Task.Delay(100);
                await PaymentsService.DeletePaymentAsync(model);
                ClearItem();
                await PaymentsListViewModel.RefreshAsync();
                EndStatusMessage("Payments deleted");
                LogWarning("Payments", "Delete", "Payments deleted", $"Taluk {model.PaymentId}  was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Payments: {ex.Message}");
                LogException("Payments", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current Payments?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<PaymentModel>> GetValidationConstraints(PaymentModel model)
        {
            yield return new RequiredConstraint<PaymentModel>("Name", m => m.PropertyId);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(PaymentsDetailsViewModel sender, string message, PaymentModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.PaymentId == current?.PaymentId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await PaymentsService.GetPaymentAsync(current.PaymentId);
                                    item = item ?? new PaymentModel { PaymentId = current.PaymentId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Payments has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Payments", "Handle Changes", ex);
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

        private async void OnListMessage(PaymentsListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<PaymentModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.PaymentId == current.PaymentId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await PaymentsService.GetPaymentAsync(current.PaymentId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Payments", "Handle Ranges Deleted", ex);
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
