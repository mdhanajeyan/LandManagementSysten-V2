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
    public class RoleDetailsViewModel : GenericDetailsViewModel<RoleModel>
    {
        public IDropDownService DropDownService { get; }
        public IRoleService RoleService { get; }
        public IFilePickerService FilePickerService { get; }
        public RoleListViewModel RoleListViewModel { get; }
      
        public RoleDetailsViewModel(IDropDownService dropDownService, IRoleService roleService, IFilePickerService filePickerService, ICommonServices commonServices, RoleListViewModel villageListViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            RoleService = roleService;
            RoleListViewModel = villageListViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Role" : TitleEdit;
        public string TitleEdit => Item == null ? "Role" : $"{Item.Name}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new RoleModel();
            IsEditMode = true;
        }
       

        public void Subscribe()
        {
            MessageService.Subscribe<RoleDetailsViewModel, RoleModel>(this, OnDetailsMessage);
            MessageService.Subscribe<RoleListViewModel>(this, OnListMessage);
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


        protected override async Task<bool> SaveItemAsync(RoleModel model)
        {
            try
            {
                StartStatusMessage("Saving Role...");

                if (model.RoleId <= 0)
                    await RoleService.AddRoleAsync(model);
                else
                    await RoleService.UpdateRoleAsync(model);
                ClearItem();
                await RoleListViewModel.RefreshAsync();
                EndStatusMessage("Role saved");
                LogInformation("Role", "Save", "Role saved successfully", $"Role {model.RoleId} '{model.Name}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Role: {ex.Message}");
                LogException("Role", "Save", ex);
                return false;
            }
        }
        protected override void ClearItem()
        {
            Item = new RoleModel() ;
        }
        protected override async Task<bool> DeleteItemAsync(RoleModel model)
        {
            try
            {
                StartStatusMessage("Deleting Role...");

                await RoleService.DeleteRoleAsync(model);
                ClearItem();
                await RoleListViewModel.RefreshAsync();
                EndStatusMessage("Role deleted");
                LogWarning("Role", "Delete", "Role deleted", $"Taluk {model.RoleId} '{model.Name}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Role: {ex.Message}");
                LogException("Role", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Role?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<RoleModel>> GetValidationConstraints(RoleModel model)
        {
            yield return new RequiredConstraint<RoleModel>("Name", m => m.Name);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(RoleDetailsViewModel sender, string message, RoleModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.RoleId == current?.RoleId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await RoleService.GetRoleAsync(current.RoleId);
                                    item = item ?? new RoleModel { RoleId = current.RoleId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Role has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Role", "Handle Changes", ex);
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

        private async void OnListMessage(RoleListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<RoleModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.RoleId == current.RoleId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await RoleService.GetRoleAsync(current.RoleId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Role", "Handle Ranges Deleted", ex);
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
