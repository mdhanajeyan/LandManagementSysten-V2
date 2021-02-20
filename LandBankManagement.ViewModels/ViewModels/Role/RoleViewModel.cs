using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class RoleViewModel : ViewModelBase
    {


        IRoleService RoleService { get; }
        public RoleListViewModel RoleList { get; set; }

        public RoleDetailsViewModel RoleDetials { get; set; }
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
        public RoleViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IRoleService roleService) : base(commonServices)
        {
            RoleService = roleService;
            RoleList = new RoleListViewModel(roleService, commonServices,this);
            RoleDetials = new RoleDetailsViewModel(dropDownService, roleService, filePickerService, commonServices, RoleList,this);
        }

        public async Task LoadAsync(RoleListArgs args)
        {
            await RoleDetials.LoadAsync();
            await RoleList.LoadAsync(args);
        }
        public void Unload()
        {
            RoleList.Unload();
        }
        int noOfApiCalls = 0;
        public void ShowProgressRing()
        {
            noOfApiCalls++;
               ProgressRingActive = true;
            ProgressRingVisibility = true;
        }
        public void HideProgressRing()
        {
            if (noOfApiCalls > 1)
            {
                noOfApiCalls--;
                return;
            }
            else
                noOfApiCalls--;
            ProgressRingActive = false;
            ProgressRingVisibility = false;
        }
        public void Subscribe()
        {
            MessageService.Subscribe<RoleListViewModel>(this, OnMessage);
            RoleList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            RoleList.Unsubscribe();

        }

        private async void OnMessage(RoleListViewModel viewModel, string message, object args)
        {
            if (viewModel == RoleList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = RoleList.SelectedItem;
            if (!RoleList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        public async Task PopulateDetails(RoleModel selected)
        {
            try
            {
                ShowProgressRing();
                var model = await RoleService.GetRoleAsync(selected.RoleId);
                selected.Merge(model);
                RoleDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("Role", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }
    }
}
