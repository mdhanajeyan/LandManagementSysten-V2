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

        public RoleViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IRoleService roleService) : base(commonServices)
        {
            RoleService = roleService;
            RoleList = new RoleListViewModel(roleService, commonServices);
            RoleDetials = new RoleDetailsViewModel(dropDownService, roleService, filePickerService, commonServices, RoleList);
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

        private async Task PopulateDetails(RoleModel selected)
        {
            try
            {
                var model = await RoleService.GetRoleAsync(selected.RoleId);
                selected.Merge(model);
                RoleDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("Role", "Load Details", ex);
            }
        }
    }
}
