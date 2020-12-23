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
        private ObservableCollection<ComboBoxOptions> _companyOptions = null;
        public ObservableCollection<ComboBoxOptions> CompanyOptions

        {
            get => _companyOptions;
            set => Set(ref _companyOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _acctTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> AcctTypeOptions

        {
            get => _acctTypeOptions;
            set => Set(ref _acctTypeOptions, value);
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
            Item = new BankAccountModel();
            GetCompanyOption();
            GetAccountTypeOption();
        }

        private void GetCompanyOption()
        {
            CompanyOptions = DropDownService.GetCompanyOptions();
        }
        private void GetAccountTypeOption()
        {
            AcctTypeOptions = DropDownService.GetAccountTypeOptions();
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
                BankAccountViewModel.HideProgressRing();
                await BankAccountListViewModel.RefreshAsync();
                EndStatusMessage("BankAccount saved");
                LogInformation("BankAccount", "Save", "BankAccount saved successfully", $"BankAccount {model.BankAccountId} '{model.AccountNumber}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving BankAccount: {ex.Message}");
                LogException("BankAccount", "Save", ex);
                return false;
            }
        }
        protected override void ClearItem()
        {
            Item = new BankAccountModel() { CompanyID = 0 };
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
                EndStatusMessage("BankAccount deleted");
                LogWarning("BankAccount", "Delete", "BankAccount deleted", $"BankAccount {model.BankAccountId} '{model.AccountNumber}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting BankAccount: {ex.Message}");
                LogException("BankAccount", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current BankAccount?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<BankAccountModel>> GetValidationConstraints(BankAccountModel model)
        {
            yield return new RequiredConstraint<BankAccountModel>("Comapny", m => m.CompanyID);
            yield return new RequiredConstraint<BankAccountModel>("Bank Name", m => m.BankName);
            yield return new RequiredConstraint<BankAccountModel>("Bank Name", m => m.BankName);
            yield return new RequiredConstraint<BankAccountModel>("Branck Name", m => m.BranchName);
            yield return new RequiredConstraint<BankAccountModel>("Type of account", m => m.AccountTypeId);
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
