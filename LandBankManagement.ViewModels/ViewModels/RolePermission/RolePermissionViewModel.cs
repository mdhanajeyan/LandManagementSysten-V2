using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class RolePermissionViewModel : ViewModelBase
    {
        IRolePermissionService RolePermissionService { get; }
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

        public RolePermissionDetailsViewModel RolePermissionDetials { get; set; }

        public RolePermissionViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IRolePermissionService rolePermissionService) : base(commonServices)
        {
            RolePermissionService = rolePermissionService;

            RolePermissionDetials = new RolePermissionDetailsViewModel(dropDownService, rolePermissionService, filePickerService, commonServices,this);
        }

        public async Task LoadAsync()
        {
            await RolePermissionDetials.LoadAsync();
           
        }
        public void Unload()
        {
            
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
        public void Subscribe()
        {
            //MessageService.Subscribe<RoleListViewModel>(this, OnMessage);           
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
           
        }

        //private async void OnMessage(RoleListViewModel viewModel, string message, object args)
        //{
        //    if (viewModel == RoleList && message == "ItemSelected")
        //    {
        //        await ContextService.RunAsync(() =>
        //        {
        //            OnItemSelected();
        //        });
        //    }
        //}

        //private async void OnItemSelected()
        //{

        //    var selected = RoleList.SelectedItem;
        //    if (!RoleList.IsMultipleSelection)
        //    {
        //        if (selected != null && !selected.IsEmpty)
        //        {
        //            await PopulateDetails(selected);
        //        }
        //    }
        //}

        //private async Task PopulateDetails(RoleModel selected)
        //{
        //    try
        //    {
        //        var model = await RoleService.GetRoleAsync(selected.RoleId);
        //        selected.Merge(model);
        //        RoleDetials.Item = model;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogException("Role", "Load Details", ex);
        //    }
        //}
    }
}
