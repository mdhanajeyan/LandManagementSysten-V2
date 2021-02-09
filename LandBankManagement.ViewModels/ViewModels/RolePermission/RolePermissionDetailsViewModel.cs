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
    public class RolePermissionDetailsViewModel : GenericDetailsViewModel<RolePermissionModel>
    {
        public IDropDownService DropDownService { get; }
        public IRolePermissionService RolePermissionService { get; }
        public IFilePickerService FilePickerService { get; }
        private ObservableCollection<ComboBoxOptions> _roleOptions = null;
        public ObservableCollection<ComboBoxOptions> RoleOptions
        {
            get => _roleOptions;
            set => Set(ref _roleOptions, value);
        }

        private ObservableCollection<RolePermissionModel> _rolePermissionList = null;
        public ObservableCollection<RolePermissionModel> RolePermissionList
        {
            get => _rolePermissionList;
            set => Set(ref _rolePermissionList, value);
        }

        private RolePermissionViewModel RolePermissionViewModel { get; set; }
        public RolePermissionDetailsViewModel(IDropDownService dropDownService, IRolePermissionService rolePermissionService, IFilePickerService filePickerService, ICommonServices commonServices, RolePermissionViewModel rolePermissionViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            RolePermissionService = rolePermissionService;
            RolePermissionViewModel = rolePermissionViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New RoleP ermission" : TitleEdit;
        public string TitleEdit => Item == null ? "Role permission" : $"{Item.RolePermissionId}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new RolePermissionModel();
            GetDropdowns();
        }

        private async void GetDropdowns() {
            RolePermissionViewModel.ShowProgressRing();
            RoleOptions = await DropDownService.GetRoleOptions();
            RolePermissionViewModel.HideProgressRing();
        }

        public async void GetRolePermissionForRole(int id) {
            RolePermissionViewModel.ShowProgressRing();
            var list =await RolePermissionService.GetRolePermissionsByRoleIDAsync(id);
            RolePermissionList = list;
            RolePermissionViewModel.HideProgressRing();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<RolePermissionDetailsViewModel,RolePermissionModel>(this, OnDetailsMessage);
            MessageService.Subscribe<RolePermissionListViewModel>(this, OnListMessage);
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


        protected override async Task<bool> SaveItemAsync(RolePermissionModel model)
        {
            try
            {
                if (RolePermissionList == null || RolePermissionList.Count == 0)
                    return false;
                RolePermissionViewModel.ShowProgressRing();
                if (Convert.ToInt32( RolePermissionList[0].RoleInfoId )== 0)
                {
                    foreach (var rolePerm in RolePermissionList)
                    {
                        rolePerm.RoleInfoId = EditableItem.RoleInfoId;
                    }
                }
                StartStatusMessage("Saving Role Permission...");
                await RolePermissionService.AddRolePermissionsAsync(RolePermissionList);
                ShowPopup("success", "Role Permission is Saved");
                EndStatusMessage("Role Permission saved");
                GetRolePermissionForRole(Convert.ToInt32(EditableItem.RoleInfoId));
                LogInformation("Role", "Save", "Role saved successfully", $"Role {model.RolePermissionId}  was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("success", "Role Permission is not Saved");
                StatusError($"Error saving RolePermission: {ex.Message}");
                LogException("Role", "Save", ex);
                return false;
            }
            finally {
                RolePermissionViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = new RolePermissionModel();
            RolePermissionList = new ObservableCollection<RolePermissionModel>();
        }
        protected override async Task<bool> DeleteItemAsync(RolePermissionModel model)
        {
            //try
            //{
            //    StartStatusMessage("DeletingRolePermission...");

            //    await RolePermissionService.DeleteRolePermissionAsync(model);
            //    ClearItem();
            //    EndStatusMessage("Role deleted");
            //    LogWarning("Role", "Delete", "Role deleted", $"Taluk {model.RolePermissionId}  was deleted.");
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    StatusError($"Error deletingRolePermission: {ex.Message}");
            //    LogException("Role", "Delete", ex);
            //    return false;
            //}
            return true;
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return false;
            //return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete currentRolePermission?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<RolePermissionModel>> GetValidationConstraints(RolePermissionModel model)
        {
            yield return new RequiredConstraint<RolePermissionModel>("Name", m => m.RolePermissionId);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(RolePermissionDetailsViewModel sender, string message,RolePermissionModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.RolePermissionId == current?.RolePermissionId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await RolePermissionService.GetRolePermissionAsync(current.RolePermissionId);
                                    item = item ?? new RolePermissionModel {RolePermissionId = current.RolePermissionId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This RolePermission has been modified externally");
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

        private async void OnListMessage(RolePermissionListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<RolePermissionModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.RolePermissionId == current.RolePermissionId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await RolePermissionService.GetRolePermissionAsync(current.RolePermissionId);
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
