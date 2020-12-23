using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class UserViewModel : ViewModelBase
    {


        IUserService UserService { get; }
        public UserListViewModel UserList { get; set; }
        private bool _progressRingVisibility;
        public bool ProgressRingVisibility
        {
            get => _progressRingVisibility;
            set => Set(ref _progressRingVisibility, value);
        }

        private bool _progressRingActive;
        public bool ProgressRingActive
        {
            get => _progressRingActive;
            set => Set(ref _progressRingActive, value);
        }

        public UserDetailsViewModel UserDetails { get; set; }

        public UserViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IUserService userService,IUserRoleService userRoleService) : base(commonServices)
        {
            UserService = userService;
            UserList = new UserListViewModel(userService, commonServices,this);
            UserDetails = new UserDetailsViewModel(dropDownService, userService, filePickerService, commonServices, UserList, userRoleService,this);
        }

        public async Task LoadAsync(UserListArgs args)
        {
            await UserDetails.LoadAsync();
            await UserList.LoadAsync(args);
        }
        public void Unload()
        {
            UserList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<UserListViewModel>(this, OnMessage);
            UserList.Subscribe();
        }
        public void ShowProgressRing()
        {
            ProgressRingActive = true;
            ProgressRingVisibility = true;
        }
        public void HideProgressRing()
        {
            ProgressRingActive = false;
            ProgressRingVisibility = false;
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            UserList.Unsubscribe();

        }

        private async void OnMessage(UserListViewModel viewModel, string message, object args)
        {
            if (viewModel == UserList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = UserList.SelectedItem;
            if (!UserList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(UserInfoModel selected)
        {
            try
            {
                ShowProgressRing();
                var model = await UserService.GetUserAsync(selected.UserInfoId);
                selected.Merge(model);
                UserDetails.Item = model;
                UserDetails.getUserRoles();
                SelectedPivotIndex = 1;
            }
            catch (Exception ex)
            {
                LogException("User", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }
    }
}
