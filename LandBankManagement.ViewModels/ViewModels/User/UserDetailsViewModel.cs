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
    public class UserDetailsViewModel : GenericDetailsViewModel<UserInfoModel>
    {
        public IDropDownService DropDownService { get; }
        public IUserService UserService { get; }
        public IUserRoleService UserRoleService { get; }
        public IFilePickerService FilePickerService { get; }
        public UserListViewModel UserListViewModel { get; }
        private ObservableCollection<UserRoleModel> _roleList = null;
        public ObservableCollection<UserRoleModel> RoleList

        {
            get => _roleList;
            set => Set(ref _roleList, value);
        }

        private UserViewModel UserViewModel { get; set; }
        public UserDetailsViewModel(IDropDownService dropDownService, IUserService roleService, IFilePickerService filePickerService, ICommonServices commonServices, UserListViewModel userListViewModel, IUserRoleService userRoleService, UserViewModel userViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            UserService = roleService;
            UserListViewModel = userListViewModel;
            UserRoleService = userRoleService;
            UserViewModel = userViewModel;
        }

        override public string Title => (Item?.IsActive ?? true) ? "New User" : TitleEdit;
        public string TitleEdit => Item == null ? "User" : $"{Item.UserName}";

        public override bool ItemIsNew => Item?.IsActive ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new UserInfoModel();
            IsEditMode = true;
            getUserRoles();
        }

        public async void getUserRoles() {
          RoleList=await  UserRoleService.GetUserRolesForUserAsync(Item.UserInfoId);
        }

        public void Subscribe()
        {
            MessageService.Subscribe<UserDetailsViewModel, UserInfoModel>(this, OnDetailsMessage);
            MessageService.Subscribe<UserListViewModel>(this, OnListMessage);
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


        protected override async Task<bool> SaveItemAsync(UserInfoModel model)
        {
            try
            {
                StartStatusMessage("Saving User...");
                UserViewModel.ShowProgressRing();
                var userID = 0;
                if (model.UserInfoId <= 0)
                    userID = await UserService.AddUserAsync(model);
                else
                    await UserService.UpdateUserAsync(model);
                await UserRoleService.AddUserRoleForUserAsync(RoleList.ToList(), model.UserInfoId == 0 ? userID : model.UserInfoId);

                reloadUser(model.UserInfoId == 0 ? userID : model.UserInfoId);
                EndStatusMessage("User saved");
                LogInformation("User", "Save", "User saved successfully", $"User {model.UserInfoId} '{model.UserName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving User: {ex.Message}");
                LogException("User", "Save", ex);
                return false;
            }
            finally {
                UserViewModel.HideProgressRing();
            }
        }

        private async void reloadUser( int id) {
            UserViewModel.ShowProgressRing();
           Item = await UserService.GetUserAsync(id);            
           getUserRoles();
            UserViewModel.HideProgressRing();
        }
        protected override void ClearItem()
        {
            Item = new UserInfoModel();
            getUserRoles();
        }
        protected override async Task<bool> DeleteItemAsync(UserInfoModel model)
        {
            try
            {
                StartStatusMessage("Deleting User...");
                UserViewModel.ShowProgressRing();
                await UserService.DeleteUserInfoAsync(model);
                ClearItem();
                await UserListViewModel.RefreshAsync();
                EndStatusMessage("User deleted");
                LogWarning("User", "Delete", "User deleted", $"Taluk {model.UserInfoId} '{model.UserName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting User: {ex.Message}");
                LogException("User", "Delete", ex);
                return false;
            }
            finally {
                UserViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.UserInfoId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current User?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<UserInfoModel>> GetValidationConstraints(UserInfoModel model)
        {
            yield return new RequiredConstraint<UserInfoModel>("Name", m => m.UserName);
            yield return new RequiredConstraint<UserInfoModel>("Name", m => m.loginName);
            yield return new ValidationConstraint<UserInfoModel>("Login Name should not contain space.", x => validateLoginName(x.loginName));
            yield return new RequiredConstraint<UserInfoModel>("Password", m => m.UserPassword);
           
        }
    private bool validateLoginName(string name) {
            return name.Split(' ').Length == 1;
    }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(UserDetailsViewModel sender, string message, UserInfoModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.UserInfoId == current?.UserInfoId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await UserService.GetUserAsync(current.UserInfoId);
                                    item = item ?? new UserInfoModel { UserInfoId = current.UserInfoId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This User has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("User", "Handle Changes", ex);
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

        private async void OnListMessage(UserListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<UserInfoModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.UserInfoId == current.UserInfoId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await UserService.GetUserAsync(current.UserInfoId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("User", "Handle Ranges Deleted", ex);
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
