using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LandBankManagement.ViewModels
{
    public class DealListArgs
    {
        static public DealListArgs CreateEmpty() => new DealListArgs { IsEmpty = true };

        public DealListArgs()
        {
            OrderBy = r => r.DealName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Deal, object>> OrderBy { get; set; }
        public Expression<Func<Data.Deal, object>> OrderByDesc { get; set; }
    }
   public class DealListViewModel : GenericListViewModel<DealModel>
    {
        public IDealService DealService { get; }
        public DealListArgs ViewModelArgs { get; private set; }

        public DealViewModel DealViewModel { get; set; }
        public DealListViewModel(IDealService propertyMergeService, ICommonServices commonServices, DealViewModel propertyMergeViewModel) : base(commonServices)
        {
            DealService = propertyMergeService;
            DealViewModel = propertyMergeViewModel;
        }
        public async Task LoadAsync(DealListArgs args)
        {
            ViewModelArgs = args ?? DealListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            //StartStatusMessage("Loading Deal...");
            //if (await RefreshAsync())
            //{
            //    EndStatusMessage("Deal loaded");
            //}
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<DealListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public DealListArgs CreateArgs()
        {
            return new DealListArgs
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
                DealViewModel.ShowProgressRing();
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<DealModel>();
                StatusError($"Error loading Deal: {ex.Message}");
                LogException("Deal", "Refresh", ex);
                isOk = false;
            }
            finally
            {
                DealViewModel.HideProgressRing();
            }
            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<DealModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Deal> request = BuildDataRequest();
                return await DealService.GetDealsAsync(request);
            }
            return new List<DealModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Deal...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Deal loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected Deal?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Deal...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Deal...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Deal: {ex.Message}");
                    LogException("Deals", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Deal deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<DealModel> models)
        {
            foreach (var model in models)
            {
                await DealService.DeleteDealAsync(model);
            }
        }

        private DataRequest<Data.Deal> BuildDataRequest()
        {
            return new DataRequest<Data.Deal>()
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
