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
    public class CashAccountListArgs
    {
        static public CashAccountListArgs CreateEmpty() => new CashAccountListArgs { IsEmpty = true };

        public CashAccountListArgs()
        {
            OrderBy = r => r.CashAccountName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.CashAccount, object>> OrderBy { get; set; }
        public Expression<Func<Data.CashAccount, object>> OrderByDesc { get; set; }
    }
    public class CashAccountListViewModel : GenericListViewModel<CashAccountModel>
    {
        public ICashAccountService CashAccountService { get; }
        public CashAccountListArgs ViewModelArgs { get; private set; }

        public CashAccountListViewModel(ICashAccountService cashAccountService, ICommonServices commonServices) : base(commonServices)
        {
            CashAccountService = cashAccountService;
        }
        public async Task LoadAsync(CashAccountListArgs args)
        {
            ViewModelArgs = args ?? CashAccountListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading CashAccount...");
            if (await RefreshAsync())
            {
                EndStatusMessage("CashAccount loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CashAccountListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public CashAccountListArgs CreateArgs()
        {
            return new CashAccountListArgs
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
                Items = new List<CashAccountModel>();
                StatusError($"Error loading CashAccount: {ex.Message}");
                LogException("CashAccount", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<CashAccountModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.CashAccount> request = BuildDataRequest();
                return await CashAccountService.GetCashAccountsAsync(request);
            }
            return new List<CashAccountModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading CashAccount...");
            if (await RefreshAsync())
            {
                EndStatusMessage("CashAccount loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected CashAccount?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} CashAccount...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} CashAccount...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} CashAccount: {ex.Message}");
                    LogException("CashAccounts", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} CashAccount deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<CashAccountModel> models)
        {
            foreach (var model in models)
            {
                await CashAccountService.DeleteCashAccountAsync(model);
            }
        }

        private DataRequest<Data.CashAccount> BuildDataRequest()
        {
            return new DataRequest<Data.CashAccount>()
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
