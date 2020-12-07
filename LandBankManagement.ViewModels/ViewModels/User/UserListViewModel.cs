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
    public class UserListArgs
    {
        static public UserListArgs CreateEmpty() => new UserListArgs { IsEmpty = true };

        public UserListArgs()
        {
            OrderBy = r => r.UserInfoId;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.UserInfo, object>> OrderBy { get; set; }
        public Expression<Func<Data.UserInfo, object>> OrderByDesc { get; set; }
    }
    public class UserListViewModel : GenericListViewModel<UserInfoModel>
    {
        public IUserService UserService { get; }
        public UserListArgs ViewModelArgs { get; private set; }

        public UserListViewModel(IUserService receiptService, ICommonServices commonServices) : base(commonServices)
        {
            UserService = receiptService;
        }
        public async Task LoadAsync(UserListArgs args)
        {
            ViewModelArgs = args ?? UserListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading User...");
            if (await RefreshAsync())
            {
                EndStatusMessage("User loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<UserListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public UserListArgs CreateArgs()
        {
            return new UserListArgs
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
                Items = new List<UserInfoModel>();
                StatusError($"Error loading User: {ex.Message}");
                LogException("User", "Refresh", ex);
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

        private async Task<IList<UserInfoModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.UserInfo> request = BuildDataRequest();
                return await UserService.GetUsersAsync(request);
            }
            return new List<UserInfoModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading User...");
            if (await RefreshAsync())
            {
                EndStatusMessage("User loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected User?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} User...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} User...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} User: {ex.Message}");
                    LogException("Users", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} User deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<UserInfoModel> models)
        {
            foreach (var model in models)
            {
                await UserService.DeleteUserInfoAsync(model);
            }
        }

        private DataRequest<Data.UserInfo> BuildDataRequest()
        {
            return new DataRequest<Data.UserInfo>()
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
