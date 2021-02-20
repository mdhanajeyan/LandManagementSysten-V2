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
    public class FundTransferListArgs
    {
        static public FundTransferListArgs CreateEmpty() => new FundTransferListArgs { IsEmpty = true };

        public FundTransferListArgs()
        {
            OrderBy = r => r.DateOfPayment;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.FundTransfer, object>> OrderBy { get; set; }
        public Expression<Func<Data.FundTransfer, object>> OrderByDesc { get; set; }
    }
    public class FundTransferListViewModel : GenericListViewModel<FundTransferModel>
    {
        public IFundTransferService FundTransferService { get; }
        public FundTransferListArgs ViewModelArgs { get; private set; }
        private FundTransferViewModel FundTransferViewModel { get; set; }
        public FundTransferListViewModel(IFundTransferService fundTransferService, ICommonServices commonServices, FundTransferViewModel fundTransferViewModel) : base(commonServices)
        {
            FundTransferService = fundTransferService;
            FundTransferViewModel = fundTransferViewModel;
        }
        public async Task LoadAsync(FundTransferListArgs args)
        {
            ViewModelArgs = args ?? FundTransferListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;
           
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<FundTransferListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public FundTransferListArgs CreateArgs()
        {
            return new FundTransferListArgs
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
                FundTransferViewModel.ShowProgressRing();
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<FundTransferModel>();
                StatusError($"Error loading FundTransfer: {ex.Message}");
                LogException("FundTransfer", "Refresh", ex);
                isOk = false;
            }
            finally {
                FundTransferViewModel.HideProgressRing();
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }
        public async void OnSelectedRow(FundTransferModel model)
        {
            await FundTransferViewModel.PopulateDetails(model);
        }
        private async Task<IList<FundTransferModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.FundTransfer> request = BuildDataRequest();
                return await FundTransferService.GetFundTransfersAsync(request);
            }
            return new List<FundTransferModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading FundTransfer...");
            if (await RefreshAsync())
            {
                EndStatusMessage("FundTransfer loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected FundTransfer?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} FundTransfer...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} FundTransfer...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} FundTransfer: {ex.Message}");
                    LogException("FundTransfers", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} FundTransfer deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<FundTransferModel> models)
        {
            foreach (var model in models)
            {
                await FundTransferService.DeleteFundTransferAsync(model);
            }
        }

        private DataRequest<Data.FundTransfer> BuildDataRequest()
        {
            return new DataRequest<Data.FundTransfer>()
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
