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
    public class CashAccountDetailsViewModel : GenericDetailsViewModel<CashAccountModel>
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


        public ICashAccountService CashAccountService { get; }
        public IFilePickerService FilePickerService { get; }
        public IDropDownService DropDownService { get; }
        public CashAccountListViewModel CashAccountListViewModel { get; }
        private CashAccountViewModel CashAccountViewModel { get; set; }
        private bool IsProcessing = false;
        public CashAccountDetailsViewModel(ICashAccountService cashAccountService, IFilePickerService filePickerService, ICommonServices commonServices, IDropDownService dropDownService, CashAccountListViewModel cashAccountListViewModel, CashAccountViewModel cashAccountViewModel) : base(commonServices)
        {
            CashAccountService = cashAccountService;
            FilePickerService = filePickerService;
            DropDownService = dropDownService;
            CashAccountListViewModel = cashAccountListViewModel;
            CashAccountViewModel = cashAccountViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New CashAccount" : TitleEdit;
        public string TitleEdit => Item == null ? "CashAccount" : $"{Item.CashAccountName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public void Load()
        {
            Item = new CashAccountModel { IsCashAccountActive=true};
            GetDropDownsOption();
            ShowActiveCompany = true;
            ChangeCompany = false;
        }

        private async void GetDropDownsOption()
        {
            CashAccountViewModel.ShowProgressRing();
            ActiveCompanyOptions = await DropDownService.GetCompanyOptions();
            AllCompanyOptions = await DropDownService.GetAllCompanyOptions();
            AcctTypeOptions = await DropDownService.GetAccountTypeOptions();
            CashAccountViewModel.HideProgressRing();
            CompanyOptions = ActiveCompanyOptions;
        }

        public void ChangeCompanyOptions(int companyId)
        {
            var comp = ActiveCompanyOptions.Where(x => Convert.ToInt32(x.Id) == companyId).FirstOrDefault();
            if (comp != null)
            {
                ResetCompanyOption();
                return;
            }
            CompanyOptions = AllCompanyOptions;
            ShowActiveCompany = false;
            ChangeCompany = true;
        }

        public void ResetCompanyOption()
        {
            CompanyOptions = ActiveCompanyOptions;
            ShowActiveCompany = true;
            ChangeCompany = false;
        }
        public void Subscribe()
        {
            MessageService.Subscribe<CashAccountDetailsViewModel, CashAccountModel>(this, OnDetailsMessage);
            MessageService.Subscribe<CashAccountListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        protected override async Task<bool> SaveItemAsync(CashAccountModel model)
        {
            try
            {
                if (IsProcessing)
                    return false;
                IsProcessing = true;
                StartStatusMessage("Saving CashAccount...");
                CashAccountViewModel.ShowProgressRing();
                if (model.CashAccountId <= 0)
                {
                    model.CashAccountId = 1;
                    await CashAccountService.AddCashAccountAsync(model);
                }
                else
                    await CashAccountService.UpdateCashAccountAsync(model);
                ClearItem();
                IsProcessing = false ;
                ShowPopup("success", "Cash Account details is Saved");
                CashAccountViewModel.HideProgressRing();
                await CashAccountListViewModel.RefreshAsync();
                EndStatusMessage("CashAccount saved");
                LogInformation("CashAccount", "Save", "CashAccount saved successfully", $"CashAccount {model.CashAccountName} '{model.CashAccountName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                IsProcessing = false;
                ShowPopup("error", "Cash Account details is not Saved");
                StatusError($"Error saving CashAccount: {ex.Message}");
                LogException("CashAccount", "Save", ex);
                return false;
            }
            finally {
                CashAccountViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = new CashAccountModel() { CompanyID = "0" ,IsCashAccountActive=true};
            ResetCompanyOption();
        }
        protected override async Task<bool> DeleteItemAsync(CashAccountModel model)
        {
            try
            {
                StartStatusMessage("Deleting CashAccount...");
                CashAccountViewModel.ShowProgressRing();
                await CashAccountService.DeleteCashAccountAsync(model);
                ClearItem();
                ShowPopup("success", "Cash Account details is deleted");
                await CashAccountListViewModel.RefreshAsync();
                EndStatusMessage("CashAccount deleted");
                LogWarning("CashAccount", "Delete", "CashAccount deleted", $"CashAccount {model.CashAccountId} '{model.CashAccountName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "Cash Account details is not deleted");
                StatusError($"Error deleting CashAccount: {ex.Message}");
                LogException("CashAccount", "Delete", ex);
                return false;
            }
            finally {
                CashAccountViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.CashAccountId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current CashAccount?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<CashAccountModel>> GetValidationConstraints(CashAccountModel model)
        {
            yield return new ValidationConstraint<CashAccountModel>("Company should not be empty", m =>Convert.ToInt32( m.CompanyID)>0);
            yield return new RequiredConstraint<CashAccountModel>("Name", m => m.CashAccountName);         

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(CashAccountDetailsViewModel sender, string message, CashAccountModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.CashAccountId == current?.CashAccountId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await CashAccountService.GetCashAccountAsync(current.CashAccountId);
                                    item = item ?? new CashAccountModel { CashAccountId = current.CashAccountId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This CashAccount has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("CashAccount", "Handle Changes", ex);
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

        private async void OnListMessage(CashAccountListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<CashAccountModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.CashAccountId == current.CashAccountId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await CashAccountService.GetCashAccountAsync(current.CashAccountId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("CashAccount", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This CashAccount has been deleted externally");
            });
        }
    }
}
