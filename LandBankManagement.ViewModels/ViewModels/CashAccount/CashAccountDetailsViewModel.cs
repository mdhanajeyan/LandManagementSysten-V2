using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class CashAccountDetailsViewModel : GenericDetailsViewModel<CashAccountModel>
    {
        public List<ComboBoxOptions> CompanyOptions { get; set; }
        public ICashAccountService CashAccountService { get; }
        public IFilePickerService FilePickerService { get; }
        ICompanyService CompanyService { get; }
        public CashAccountDetailsViewModel(ICashAccountService cashAccountService, IFilePickerService filePickerService, ICommonServices commonServices, ICompanyService companyService) : base(commonServices)
        {
            CashAccountService = cashAccountService;
            FilePickerService = filePickerService;
            CompanyService = companyService;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New CashAccount" : TitleEdit;
        public string TitleEdit => Item == null ? "CashAccount" : $"{Item.CashAccountName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public void Load()
        {
            Item = new CashAccountModel();
            IsEditMode = true;
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
                StartStatusMessage("Saving CashAccount...");
                await Task.Delay(100);
                if (model.CashAccountId <= 0)
                {
                    model.CashAccountId = 1;
                    await CashAccountService.AddCashAccountAsync(model);
                }
                else
                    await CashAccountService.UpdateCashAccountAsync(model);
                EndStatusMessage("CashAccount saved");
                LogInformation("CashAccount", "Save", "CashAccount saved successfully", $"CashAccount {model.CashAccountName} '{model.CashAccountName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving CashAccount: {ex.Message}");
                LogException("CashAccount", "Save", ex);
                return false;
            }
        }

        protected override async Task<bool> DeleteItemAsync(CashAccountModel model)
        {
            try
            {
                StartStatusMessage("Deleting CashAccount...");
                await Task.Delay(100);
                await CashAccountService.DeleteCashAccountAsync(model);
                EndStatusMessage("CashAccount deleted");
                LogWarning("CashAccount", "Delete", "CashAccount deleted", $"CashAccount {model.CashAccountId} '{model.CashAccountName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting CashAccount: {ex.Message}");
                LogException("CashAccount", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current CashAccount?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<CashAccountModel>> GetValidationConstraints(CashAccountModel model)
        {
            yield return new RequiredConstraint<CashAccountModel>("Name", m => m.CashAccountName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

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
