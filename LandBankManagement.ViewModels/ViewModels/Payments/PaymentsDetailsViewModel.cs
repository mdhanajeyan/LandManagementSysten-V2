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
   public class PaymentsDetailsViewModel : GenericDetailsViewModel<PaymentModel>
    {
        public IDropDownService DropDownService { get; }
        public IPaymentService PaymentsService { get; }
        public IFilePickerService FilePickerService { get; }
      
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

        private ObservableCollection<ComboBoxOptions> _activeDocumentTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> ActiveDocumentTypeOptions
        {
            get => _activeDocumentTypeOptions;
            set => Set(ref _activeDocumentTypeOptions, value);
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

        private ObservableCollection<ComboBoxOptions> _groupsOptions = null;
        public ObservableCollection<ComboBoxOptions> GroupsOptions
        {
            get => _groupsOptions;
            set => Set(ref _groupsOptions, value);
        }

        private ObservableCollection<PaymentListModel> _paymentList = null;
        public ObservableCollection<PaymentListModel> PaymentList
        {
            get => _paymentList;
            set => Set(ref _paymentList, value);
        }
        private PaymentListModel _currentpayment = null;
        public PaymentListModel CurrentPayment
        {
            get => _currentpayment;
            set => Set(ref _currentpayment, value);
        }


        private bool _expenseVisibility ;
        public bool ExpenseVisibility
        {
            get => _expenseVisibility;
            set => Set(ref _expenseVisibility, value);
        }

        private bool _partyVisibility;
        public bool PartyVisibility
        {
            get => _partyVisibility;
            set => Set(ref _partyVisibility, value);
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

        private string _selectedDocType;
        public string SelectedDocType
        {
            get => _selectedDocType;
            set => Set(ref _selectedDocType, value);
        }

        private string _selectedBank;
        public string SelectedBank
        {
            get => _selectedBank;
            set => Set(ref _selectedBank, value);
        }
        private string _selectedCash;
        public string SelectedCash
        {
            get => _selectedCash;
            set => Set(ref _selectedCash, value);
        }

        private string _selectedParty;
        public string SelectedParty
        {
            get => _selectedParty;
            set => Set(ref _selectedParty, value);
        }

        private PaymentsViewModel PaymentsViewModel { get; set; }
        public PaymentsDetailsViewModel(IDropDownService dropDownService, IPaymentService villageService, IFilePickerService filePickerService, ICommonServices commonServices, PaymentsViewModel paymentsViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            PaymentsService = villageService;
            PaymentsViewModel = paymentsViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Payments" : TitleEdit;
        public string TitleEdit  ="Payments";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            IsEditMode = true;
            Item = new PaymentModel() { PayeeTypeId=1,PaymentTypeId=1};
            Item.DateOfPayment = DateTime.Now;
           await GetDropdowns();
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
            CurrentPayment = new PaymentListModel { DateOfPayment = DateTimeOffset.Now };
        }

        private async Task GetDropdowns()
        {
            PaymentsViewModel.ShowProgressRing();
           CompanyOptions =await DropDownService.GetCompanyOptions();
            ExpenseOptions = await DropDownService.GetExpenseHeadOptions();
            //PartyOptions= await DropDownService.GetPartyOptions();
            PropertyOptions= await DropDownService.GetPropertyOptions();
            GroupsOptions= await DropDownService.GetGroupsOptionsForParty();
            ActiveDocumentTypeOptions = await DropDownService.GetDocumentTypeOptions();           
            PaymentsViewModel.HideProgressRing();
        }

        public async Task LoadBankAndCompany() {
            if (Item.PayeeId == null || Item.PayeeId == "0")
                return;
            CashOptions = await DropDownService.GetCashOptionsByCompany(Convert.ToInt32( Item.PayeeId));
            BankOptions = await DropDownService.GetBankOptionsByCompany(Convert.ToInt32(Item.PayeeId));
        }

        public async Task LoadDocumentTypedByProperty() {
            if (!IsExpenseChecked &&(Item.PropertyId != null && Item.PropertyId != "0") )
            DocumentTypeOptions = await DropDownService.GetDocumentTypesByPropertyID(Convert.ToInt32(Item.PropertyId));
            PartyOptions = await DropDownService.GetPartyOptionsByProperty(Convert.ToInt32(Item.PropertyId));
        }

        public async Task LoadParty()
        {
            //if (!IsExpenseChecked && (Item.PropertyId != null && Item.PropertyId != "0"))
            //PartyOptions = await DropDownService.GetPartyOptionsByProperty(Convert.ToInt32(Item.PropertyId));
            if (!IsExpenseChecked && (Item.GroupId != null && Item.GroupId != "0"))
            {
                var items = await DropDownService.GetPartyOptionsByGroup(Convert.ToInt32(Item.GroupId));
                PartyOptions = items;
            }
            if (Item.PartyId != null && Item.PartyId != "0" && PartyOptions!=null)
            {
                SelectedParty = null;
                SelectedParty = Item.PartyId;
            }
        }

        public ICommand ExpenseCheckedCommand => new RelayCommand(OnExpenseRadioChecked);
        virtual protected async void OnExpenseRadioChecked()
        {
            if (IsExpenseChecked)
            {
                DocumentTypeOptions = ActiveDocumentTypeOptions;
                ExpenseVisibility = true;
                PartyVisibility = false;
            }
            else {               
                await LoadDocumentTypedByProperty();
                ExpenseVisibility = false;
                PartyVisibility = true;
            }            
        }

        public async Task LoadDocTypes() {
            if (IsExpenseChecked)
            {
                DocumentTypeOptions = ActiveDocumentTypeOptions;
                ExpenseVisibility = true;
                PartyVisibility = false;
            }
            else
            {
                await LoadDocumentTypedByProperty();
                ExpenseVisibility = false;
                PartyVisibility = true;
            }
        }

        public void ReloadBankAndCash() {
            
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
                BankVisibility =true;
            }
        }

        public void AddPaymentToList() {
            if (PaymentList == null)
                PaymentList = new ObservableCollection<PaymentListModel>();

            if (IsCashChecked)
            {
                CurrentPayment.PaymentTypeId = true;
                CurrentPayment.BankAccountId = 0;
            }
            else
            {
                CurrentPayment.PaymentTypeId = false;
                CurrentPayment.CashAccountId = 0;
            }

            PaymentList.Add(CurrentPayment);
            CurrentPayment = new PaymentListModel { DateOfPayment=DateTimeOffset.Now,PDC=true};
            for (int i = 0; i < PaymentList.Count; i++) {
                PaymentList[i].identity = i + 1;
            }
            var newList= PaymentList;
            PaymentList = null;
            PaymentList = newList;
        }
        public void ClearCurrentPayment() {
            CurrentPayment = new PaymentListModel { DateOfPayment = DateTimeOffset.Now, PDC = true };
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

        public async void DeletePaymentList(int id) {

            if (PaymentList[id-1].PaymentListId > 0) {
                PaymentsViewModel.ShowProgressRing();
                await PaymentsService.DeletePaymentListAsync((int)PaymentList[id - 1].PaymentListId);
                PaymentsViewModel.HideProgressRing();
            }
            PaymentList.RemoveAt(id - 1);
            for (int i = 0; i < PaymentList.Count; i++)
            {
                PaymentList[i].identity = i + 1;
            }
            var newList = PaymentList;
            PaymentList = null;
            PaymentList = newList;
        }


        protected override async Task<bool> SaveItemAsync(PaymentModel model)
        {
            try
            {
                if (IsExpenseChecked)
                    model.PayeeTypeId = 1;
                else
                    model.PayeeTypeId = 2;

                if (IsCashChecked)
                    model.PaymentTypeId = 1;
                else
                    model.PaymentTypeId = 2;

                //if (!ValidatePayments())
                //    return false;

              //  model.PaymentListModel = PaymentList;

                StartStatusMessage("Saving Payments...");
                PaymentsViewModel.ShowProgressRing();
                model.DocumentTypeId = SelectedDocType;
                model.PartyId = SelectedParty;
                model.BankAccountId = SelectedBank;
                model.CashAccountId = SelectedCash;
                int paymentId = 0;
                if (model.PaymentId <= 0)
                    paymentId = await PaymentsService.AddPaymentAsync(model);
                else
                    await PaymentsService.UpdatePaymentAsync(model);

                var item = await PaymentsService.GetPaymentAsync(paymentId == 0 ? model.PaymentId : paymentId);
                Item = item;
               // PaymentList = item.PaymentListModel;

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
            finally { PaymentsViewModel.HideProgressRing(); }
        }

        //private bool ValidatePayments() {

        //    if (PaymentList != null && PaymentList.Count > 0)
        //        return true;

        //    if (CurrentPayment.CashAccountId <= 0 && CurrentPayment.BankAccountId <= 0) {
        //        DialogService.ShowAsync("Validation Error", "Please select either Cash OR Bank account", "Ok");
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(CurrentPayment.Amount) || Convert.ToDecimal(CurrentPayment.Amount) <= 0) {
        //        DialogService.ShowAsync("Validation Error", "Please enter the amount", "Ok");
        //        return false;
        //    }
        //    if ( string.IsNullOrEmpty( CurrentPayment.ChequeNo))
        //    {
        //        DialogService.ShowAsync("Validation Error", "Please enter the Cheque / Ref.No", "Ok");
        //        return false;
        //    }
        //    if (PaymentList == null) {
        //        PaymentList = new ObservableCollection<PaymentListModel>();
        //    }
        //    PaymentList.Add(CurrentPayment);
        //    CurrentPayment = new PaymentListModel { DateOfPayment = DateTimeOffset.Now, PDC = true };
        //    for (int i = 0; i < PaymentList.Count; i++)
        //    {
        //        PaymentList[i].identity = i + 1;
        //    }
        //    var newList = PaymentList;
        //    PaymentList = null;
        //    PaymentList = newList;

        //    return true;
        //}

        protected override void ClearItem()
        {
            Item = new PaymentModel() { PayeeTypeId = 1, PaymentTypeId = 1,DateOfPayment=DateTimeOffset.Now };
            PaymentList = new ObservableCollection<PaymentListModel>();
            defaultSettings();
            SelectedBank = "0";
            SelectedCash = "0";
            SelectedParty = "0";
        }
        protected override async Task<bool> DeleteItemAsync(PaymentModel model)
        {
            try
            {
                StartStatusMessage("Deleting Payments...");
                PaymentsViewModel.ShowProgressRing();
                await PaymentsService.DeletePaymentAsync(model);
                ClearItem();
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
            finally { PaymentsViewModel.HideProgressRing(); }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Payments?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<PaymentModel>> GetValidationConstraints(PaymentModel model)
        {
            yield return new ValidationConstraint<PaymentModel>("Company must be selected", m => Convert.ToInt32(m.PayeeId)>0);
            yield return new ValidationConstraint<PaymentModel>("Proeprty Name must be selected", m => Convert.ToInt32(m.PropertyId)>0);
            yield return new ValidationConstraint<PaymentModel>("Expense head / Party must be selected", m => (Convert.ToInt32(m.ExpenseHeadId) > 0 || Convert.ToInt32(SelectedParty)>0));
            yield return new ValidationConstraint<PaymentModel>("Document Type must be selected", m => ValidateDocumentType(m));
           yield return new ValidationConstraint<PaymentModel>("Cash / Bank must be selected", m => (Convert.ToInt32(SelectedBank )> 0 || Convert.ToInt32(SelectedCash)>0));
            yield return new ValidationConstraint<PaymentModel>("Amount should not be empty", m => ValidateAmount(m));
            yield return new RequiredConstraint<PaymentModel>("Cheque / Ref No", m => m.ChequeNo);
            // yield return new ValidationConstraint<PaymentModel>("Expense head Or Party Not to be empty", x => ValidateExpenseHeadAndParty(x));
        }

        private bool ValidateExpenseHeadAndParty(PaymentModel model) {
            return Convert.ToInt32(model.ExpenseHeadId) > 0 || Convert.ToInt32(model.PartyId) > 0;
        }

        private bool ValidateDocumentType(PaymentModel model)
        {
            if (IsExpenseChecked) {
                return true;
            }
            return Convert.ToInt32(SelectedDocType) > 0 ;
        }

        private bool ValidateCashAndBank(PaymentModel model)
        {
            return Convert.ToInt32(model.CashAccountId) > 0 || Convert.ToInt32(model.BankAccountId) > 0;
        }

        private bool ValidateAmount(PaymentModel model)
        {
            return string.IsNullOrEmpty(model.Amount) ? false : Convert.ToDecimal(model.Amount) > 0;            
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
