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
    public class GroupsListArgs
    {
        static public GroupsListArgs CreateEmpty() => new GroupsListArgs { IsEmpty = true };

        public GroupsListArgs()
        {
            OrderBy = r => r.GroupName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Groups, object>> OrderBy { get; set; }
        public Expression<Func<Data.Groups, object>> OrderByDesc { get; set; }
    }
    
       
    public class GroupsListViewModel : GenericListViewModel<GroupsModel>
    {
        public IGroupsService GroupsService { get; }
        public GroupsListArgs ViewModelArgs { get; private set; }
        public GroupsViewModel GroupsViewModel { get; set; }

        public GroupsListViewModel(IGroupsService groupsService, ICommonServices commonServices, GroupsViewModel groupsViewModel) : base(commonServices)
        {
            GroupsService = groupsService;
            GroupsViewModel = groupsViewModel;
        }
        public async Task LoadAsync(GroupsListArgs args)
        {
            ViewModelArgs = args ?? GroupsListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Groups...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Groups loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<GroupsListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public GroupsListArgs CreateArgs()
        {
            return new GroupsListArgs
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
                GroupsViewModel.ShowProgressRing();
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<GroupsModel>();
                StatusError($"Error loading Document Type: {ex.Message}");
                LogException("Document Type", "Refresh", ex);
                isOk = false;
            }
            finally
            {
                GroupsViewModel.HideProgressRing();
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }
        public async void OnSelectedRow(GroupsModel model)
        {
            await GroupsViewModel.PopulateDetails(model);
        }
        private async Task<IList<GroupsModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Groups> request = BuildDataRequest();
                return await GroupsService.GetGroupsAsync(request);
            }
            return new List<GroupsModel>();
        }

        //public ICommand OpenInNewViewCommand => new RelayCommand(OnOpenInNewView);
        //private async void OnOpenInNewView()
        //{
        //    if (SelectedItem != null)
        //    {
        //        await NavigationService.CreateNewViewAsync<GroupsViewModel>(new PartyDetailsArgs { PartyId = SelectedItem.GroupsId });
        //    }
        //}

        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<GroupsViewModel>(new GroupsArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Groups...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Groups loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected Groups?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Groups...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Groups...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Groups: {ex.Message}");
                    LogException("Groupss", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Groups deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<GroupsModel> models)
        {
            foreach (var model in models)
            {
                await GroupsService.DeleteGroupsAsync(model);
            }
        }

        //private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        //{
        //    DataRequest<Vendor> request = BuildDataRequest();
        //    foreach (var range in ranges)
        //    {
        //        await VendorService.DeleteVendorRangeAsync(range.Index, range.Length, request);
        //    }
        //}

        private DataRequest<Data.Groups> BuildDataRequest()
        {
            return new DataRequest<Data.Groups>()
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
