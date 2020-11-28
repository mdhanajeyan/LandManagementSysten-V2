using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class CheckListListArgs
    {
        static public CheckListListArgs CreateEmpty() => new CheckListListArgs { IsEmpty = true };

        public CheckListListArgs()
        {
            OrderBy = r => r.CheckListName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.CheckList, object>> OrderBy { get; set; }
        public Expression<Func<Data.CheckList, object>> OrderByDesc { get; set; }
    }
    public class CheckListListViewModel : GenericListViewModel<CheckListModel>
    {
        public ICheckListService CheckListService { get; }
        public CheckListListArgs ViewModelArgs { get; private set; }

        public CheckListListViewModel(ICheckListService checkListService, ICommonServices commonServices) : base(commonServices)
        {
            CheckListService = checkListService;
        }
        public async Task LoadAsync(CheckListListArgs args)
        {
            ViewModelArgs = args ?? CheckListListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading CheckList...");
            if (await RefreshAsync())
            {
                EndStatusMessage("CheckList loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CheckListListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public CheckListListArgs CreateArgs()
        {
            return new CheckListListArgs
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
                Items = new List<CheckListModel>();
                StatusError($"Error loading CheckList: {ex.Message}");
                LogException("CheckList", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                //  SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<CheckListModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.CheckList> request = BuildDataRequest();
                return await CheckListService.GetCheckListsAsync(request);
            }
            return new List<CheckListModel>();
        }

        //public ICommand OpenInNewViewCommand => new RelayCommand(OnOpenInNewView);
        //private async void OnOpenInNewView()
        //{
        //    if (SelectedItem != null)
        //    {
        //        await NavigationService.CreateNewViewAsync<CheckListViewModel>(new PartyDetailsArgs { PartyId = SelectedItem.CheckListId });
        //    }
        //}

        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<CheckListViewModel>(new CheckListArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading CheckList...");
            if (await RefreshAsync())
            {
                EndStatusMessage("CheckList loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected CheckList?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} CheckList...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} CheckList...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} CheckList: {ex.Message}");
                    LogException("CheckLists", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} CheckList deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<CheckListModel> models)
        {
            foreach (var model in models)
            {
                await CheckListService.DeleteCheckListAsync(model);
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

        private DataRequest<Data.CheckList> BuildDataRequest()
        {
            return new DataRequest<Data.CheckList>()
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
