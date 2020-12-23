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
    public class ExpenseHeadListArgs
    {
        static public ExpenseHeadListArgs CreateEmpty() => new ExpenseHeadListArgs { IsEmpty = true };

        public ExpenseHeadListArgs()
        {
            OrderBy = r => r.ExpenseHeadName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.ExpenseHead, object>> OrderBy { get; set; }
        public Expression<Func<Data.ExpenseHead, object>> OrderByDesc { get; set; }
    }
    public class ExpenseHeadListViewModel : GenericListViewModel<ExpenseHeadModel>
    {
        public IExpenseHeadService ExpenseHeadService { get; }
        public ExpenseHeadListArgs ViewModelArgs { get; private set; }
        private ExpenseHeadViewModel ExpenseHeadViewModel { get; set; }
        public ExpenseHeadListViewModel(IExpenseHeadService expenseHeadService, ICommonServices commonServices, ExpenseHeadViewModel expenseHeadViewModel) : base(commonServices)
        {
            ExpenseHeadService = expenseHeadService;
            ExpenseHeadViewModel = expenseHeadViewModel;
        }
        public async Task LoadAsync(ExpenseHeadListArgs args)
        {
            ViewModelArgs = args ?? ExpenseHeadListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Expense Head...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Expense Head loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<ExpenseHeadListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public ExpenseHeadListArgs CreateArgs()
        {
            return new ExpenseHeadListArgs
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
                ExpenseHeadViewModel.ShowProgressRing();
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<ExpenseHeadModel>();
                StatusError($"Error loading Expense Head: {ex.Message}");
                LogException("Document Type", "Refresh", ex);
                isOk = false;
            }
            finally {
                ExpenseHeadViewModel.HideProgressRing();
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<ExpenseHeadModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.ExpenseHead> request = BuildDataRequest();
                return await ExpenseHeadService.GetExpenseHeadsAsync(request);
            }
            return new List<ExpenseHeadModel>();
        }

        //public ICommand OpenInNewViewCommand => new RelayCommand(OnOpenInNewView);
        //private async void OnOpenInNewView()
        //{
        //    if (SelectedItem != null)
        //    {
        //        await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new PartyDetailsArgs { PartyId = SelectedItem.ExpenseHeadId });
        //    }
        //}

        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading ExpenseHead...");
            if (await RefreshAsync())
            {
                EndStatusMessage("ExpenseHead loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected ExpenseHead?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} ExpenseHead...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} ExpenseHead...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} ExpenseHead: {ex.Message}");
                    LogException("ExpenseHeads", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} ExpenseHead deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<ExpenseHeadModel> models)
        {
            foreach (var model in models)
            {
                await ExpenseHeadService.DeleteExpenseHeadAsync(model);
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

        private DataRequest<Data.ExpenseHead> BuildDataRequest()
        {
            return new DataRequest<Data.ExpenseHead>()
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
