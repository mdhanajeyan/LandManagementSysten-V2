using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LandBankManagement.ViewModels
{
    public class RolePermissionListArgs
    {
        static public RolePermissionListArgs CreateEmpty() => new RolePermissionListArgs { IsEmpty = true };

        public RolePermissionListArgs()
        {
            OrderBy = r => r.RolePermissionId;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.RolePermission, object>> OrderBy { get; set; }
        public Expression<Func<Data.RolePermission, object>> OrderByDesc { get; set; }
    }
    public class RolePermissionListViewModel : GenericListViewModel<RolePermissionModel>
    {
        public IRolePermissionService RolePermissionService { get; }
        public RolePermissionListArgs ViewModelArgs { get; private set; }

        public RolePermissionListViewModel(IRolePermissionService rolePermissionService, ICommonServices commonServices) : base(commonServices)
        {
            RolePermissionService = rolePermissionService;
        }
        public async Task LoadAsync(RolePermissionListArgs args)
        {
            ViewModelArgs = args ?? RolePermissionListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Role...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Role loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<RolePermissionListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public RolePermissionListArgs CreateArgs()
        {
            return new RolePermissionListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            Items = null;
            ItemsCount = 0;
            SelectedItem = null;

            try
            {
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<RolePermissionModel>();
                StatusError($"Error loading Role: {ex.Message}");
                LogException("Role", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<RolePermissionModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.RolePermission> request = BuildDataRequest();
                return await RolePermissionService.GetRolePermissionsAsync(request);
            }
            return new List<RolePermissionModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Role Permission...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Role Permission loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected Role?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Role...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Role...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Role: {ex.Message}");
                    LogException("Roles", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Role deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<RolePermissionModel> models)
        {
            foreach (var model in models)
            {
                await RolePermissionService.DeleteRolePermissionAsync(model);
            }
        }

        private DataRequest<Data.RolePermission> BuildDataRequest()
        {
            return new DataRequest<Data.RolePermission>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            switch (message)
            {
                case "NewItemSaved":
                case "ItemDeleted":
                case "ItemsDeleted":
                case "ItemRangesDeleted":
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }
    }
}
