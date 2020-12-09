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
    public class RoleListArgs
    {
        static public RoleListArgs CreateEmpty() => new RoleListArgs { IsEmpty = true };

        public RoleListArgs()
        {
            OrderBy = r => r.RoleId;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Role, object>> OrderBy { get; set; }
        public Expression<Func<Data.Role, object>> OrderByDesc { get; set; }
    }
    public class RoleListViewModel : GenericListViewModel<RoleModel>
    {
        public IRoleService RoleService { get; }
        public RoleListArgs ViewModelArgs { get; private set; }

        public RoleListViewModel(IRoleService receiptService, ICommonServices commonServices) : base(commonServices)
        {
            RoleService = receiptService;
        }
        public async Task LoadAsync(RoleListArgs args)
        {
            ViewModelArgs = args ?? RoleListArgs.CreateEmpty();
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
            MessageService.Subscribe<RoleListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public RoleListArgs CreateArgs()
        {
            return new RoleListArgs
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
                Items = new List<RoleModel>();
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

        private async Task<IList<RoleModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Role> request = BuildDataRequest();
                return await RoleService.GetRolesAsync(request);
            }
            return new List<RoleModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Role...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Role loaded");
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

        private async Task DeleteItemsAsync(IEnumerable<RoleModel> models)
        {
            foreach (var model in models)
            {
                await RoleService.DeleteRoleAsync(model);
            }
        }

        private DataRequest<Data.Role> BuildDataRequest()
        {
            return new DataRequest<Data.Role>()
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
