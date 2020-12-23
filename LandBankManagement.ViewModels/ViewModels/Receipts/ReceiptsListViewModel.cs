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
    public class ReceiptsListArgs
    {
        static public ReceiptsListArgs CreateEmpty() => new ReceiptsListArgs { IsEmpty = true };

        public ReceiptsListArgs()
        {
            OrderBy = r => r.ReceiptId;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Receipt, object>> OrderBy { get; set; }
        public Expression<Func<Data.Receipt, object>> OrderByDesc { get; set; }
    }
    public class ReceiptsListViewModel : GenericListViewModel<ReceiptModel>
    {
        public IReceiptService ReceiptsService { get; }
        public ReceiptsListArgs ViewModelArgs { get; private set; }

        private ReceiptsViewModel ReceiptsViewModel { get; set; }
        public ReceiptsListViewModel(IReceiptService receiptService, ICommonServices commonServices, ReceiptsViewModel receiptsViewModel) : base(commonServices)
        {
            ReceiptsService = receiptService;
            ReceiptsViewModel = receiptsViewModel;
        }
        public async Task LoadAsync(ReceiptsListArgs args)
        {
            ViewModelArgs = args ?? ReceiptsListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Receipts...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Receipts loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<ReceiptsListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public ReceiptsListArgs CreateArgs()
        {
            return new ReceiptsListArgs
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
                ReceiptsViewModel.ShowProgressRing();
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<ReceiptModel>();
                StatusError($"Error loading Receipts: {ex.Message}");
                LogException("Receipts", "Refresh", ex);
                isOk = false;
            }
            finally {
                ReceiptsViewModel.HideProgressRing();
            }
            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<ReceiptModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Receipt> request = BuildDataRequest();
                return await ReceiptsService.GetReceiptsAsync(request);
            }
            return new List<ReceiptModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Receipts...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Receipts loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected Receipts?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Receipts...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Receipts...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Receipts: {ex.Message}");
                    LogException("Receiptss", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Receipts deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<ReceiptModel> models)
        {
            foreach (var model in models)
            {
                await ReceiptsService.DeleteReceiptAsync(model);
            }
        }

        private DataRequest<Data.Receipt> BuildDataRequest()
        {
            return new DataRequest<Data.Receipt>()
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
