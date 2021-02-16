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
    public class BankAccountDetailsViewModel : GenericDetailsViewModel<BankAccountModel>
    {
        private ObservableCollection<ComboBoxOptions> _activeCompanyOptions = null;
        public ObservableCollection<ComboBoxOptions> ActiveCompanyOptions

        {
            get => _activeCompanyOptions;
            set => Set(ref _activeCompanyOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _companyOptions = null;
        public ObservableCollection<ComboBoxOptions> CompanyOptions

        {
            get => _companyOptions;
            set => Set(ref _companyOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _allCompanyOptions = null;
        public ObservableCollection<ComboBoxOptions> AllCompanyOptions

        {
            get => _allCompanyOptions;
            set => Set(ref _allCompanyOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _acctTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> AcctTypeOptions

        {
            get => _acctTypeOptions;
            set => Set(ref _acctTypeOptions, value);
        }

        private bool _showComp = true;
        public bool ShowActiveCompany
        {
            get => _showComp;
            set => Set(ref _showComp, value);
        }

        private bool _hideComp = false;
        public bool ChangeCompany
        {
            get => _hideComp;
            set => Set(ref _hideComp, value);
        }

        public IBankAccountService BankAccountService { get; }
        public IFilePickerService FilePickerService { get; }
        public IDropDownService DropDownService { get; }
        public BankAccountListViewModel BankAccountListViewModel { get; }
        private BankAccountViewModel BankAccountViewModel { get; set; }
        public BankAccountDetailsViewModel(IBankAccountService cashAccountService, IFilePickerService filePickerService, ICommonServices commonServices, IDropDownService dropDownService, BankAccountListViewModel bankAccountListViewModel, BankAccountViewModel bankAccountViewModel) : base(commonServices)
        {
            BankAccountService = cashAccountService;
            FilePickerService = filePickerService;
            DropDownService = dropDownService;
            BankAccountListViewModel = bankAccountListViewModel;
            BankAccountViewModel = bankAccountViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New BankAccount" : TitleEdit;
        public string TitleEdit => Item == null ? "BankAccount" : $"{Item.AccountNumber}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public void Load()
        {
            Item = new BankAccountModel { IsBankAccountActive=true};
            
            GetDropDownsOption();
            ShowActiveCompany = true;
            ChangeCompany = false;
        }

        private async void GetDropDownsOption()
        {
            BankAccountViewModel.ShowProgressRing();
            ActiveCompanyOptions = await DropDownService.GetCompanyOptions();
            AllCompanyOptions = await DropDownService.GetAllCompanyOptions();
            AcctTypeOptions = await DropDownService.GetAccountTypeOptions();
            BankAccountViewModel.HideProgressRing();
            CompanyOptions = ActiveCompanyOptions;
        }

        public void ChangeCompanyOptions(string companyId) {
            var comp = ActiveCompanyOptions.Where(x => x.Id== companyId ).FirstOrDefault();
            if (comp != null)
            {
                ResetCompanyOption();
                return;
            }
            CompanyOptions = AllCompanyOptions;
            ShowActiveCompany = false;
            ChangeCompany = true;
        }

        public void ResetCompanyOption() {
            CompanyOptions = ActiveCompanyOptions;
            ShowActiveCompany = true;
            ChangeCompany = false;
        }
        public void Subscribe()
        {
            MessageService.Subscribe<BankAccountDetailsViewModel, BankAccountModel>(this, OnDetailsMessage);
            MessageService.Subscribe<BankAccountListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        protected override async Task<bool> SaveItemAsync(BankAccountModel model)
        {
            try
            {
                StartStatusMessage("Saving BankAccount...");
                BankAccountViewModel.ShowProgressRing();
                if (model.BankAccountId <= 0)
                {
                    model.BankAccountId = 1;
                    await BankAccountService.AddBankAccountAsync(model);
                }
                else
                    await BankAccountService.UpdateBankAccountAsync(model);
                ClearItem();
                ShowPopup("success", "BankAccount details is saved");
                BankAccountViewModel.HideProgressRing();
                await BankAccountListViewModel.RefreshAsync();
                EndStatusMessage("BankAccount saved");
                LogInformation("BankAccount", "Save", "BankAccount saved successfully", $"BankAccount {model.BankAccountId} '{model.AccountNumber}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "BankAccount details is not saved");
                StatusError($"Error saving BankAccount: {ex.Message}");
                LogException("BankAccount", "Save", ex);
                return false;
            }
        }
        protected override void ClearItem()
        {
            Item = new BankAccountModel() { CompanyID = "0",IsBankAccountActive=true };
            ResetCompanyOption();
        }
        protected override async Task<bool> DeleteItemAsync(BankAccountModel model)
        {
            try
            {
                StartStatusMessage("Deleting BankAccount...");
                BankAccountViewModel.ShowProgressRing();
                await BankAccountService.DeleteBankAccountAsync(model);
                ClearItem();
                BankAccountViewModel.HideProgressRing();
                await BankAccountListViewModel.RefreshAsync();
                ShowPopup("success", "BankAccount details is deleted");
                EndStatusMessage("BankAccount deleted");
                LogWarning("BankAccount", "Delete", "BankAccount deleted", $"BankAccount {model.BankAccountId} '{model.AccountNumber}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "BankAccount details is not deleted");
                StatusError($"Error deleting BankAccount: {ex.Message}");
                LogException("BankAccount", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.BankAccountId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current BankAccount?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<BankAccountModel>> GetValidationConstraints(BankAccountModel model)
        {
            yield return new ValidationConstraint<BankAccountModel>("Comapny should not be empty", x =>Convert.ToInt32( x.CompanyID)>0);
            yield return new RequiredConstraint<BankAccountModel>("Bank Name", m => m.BankName);
            yield return new RequiredConstraint<BankAccountModel>("Branck Name", m => m.BranchName);
            yield return new ValidationConstraint<BankAccountModel>("Type of account", m =>Convert.ToInt32( m.AccountTypeId) > 0);
            yield return new ValidationConstraint<BankAccountModel>("Account Number", m => m.AccountNumber.Length>=9 &&  m.AccountNumber.Length <= 18);           
            yield return new RequiredConstraint<BankAccountModel>("IFSC Code", m => m.IFSCCode);
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(BankAccountDetailsViewModel sender, string message, BankAccountModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.BankAccountId == current?.BankAccountId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await BankAccountService.GetBankAccountAsync(current.BankAccountId);
                                    item = item ?? new BankAccountModel { BankAccountId = current.BankAccountId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This BankAccount has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("BankAccount", "Handle Changes", ex);
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

        private async void OnListMessage(BankAccountListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<BankAccountModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.BankAccountId == current.BankAccountId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await BankAccountService.GetBankAccountAsync(current.BankAccountId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("BankAccount", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This BankAccount has been deleted externally");
            });
        }
    }
}
