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
    public class BankAccountListArgs
    {
        static public BankAccountListArgs CreateEmpty() => new BankAccountListArgs { IsEmpty = true };

        public BankAccountListArgs()
        {
            OrderBy = r => r.AccountNumber;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.BankAccount, object>> OrderBy { get; set; }
        public Expression<Func<Data.BankAccount, object>> OrderByDesc { get; set; }
    }
    public class BankAccountListViewModel : GenericListViewModel<BankAccountModel>
    {
        public IBankAccountService BankAccountService { get; }
        public BankAccountListArgs ViewModelArgs { get; private set; }

        public BankAccountListViewModel(IBankAccountService bankAccountService, ICommonServices commonServices) : base(commonServices)
        {
            BankAccountService = bankAccountService;
        }
        public async Task LoadAsync(BankAccountListArgs args)
        {
            ViewModelArgs = args ?? BankAccountListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading BankAccount...");
            if (await RefreshAsync())
            {
                EndStatusMessage("BankAccount loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<BankAccountListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public BankAccountListArgs CreateArgs()
        {
            return new BankAccountListArgs
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
                Items = new List<BankAccountModel>();
                StatusError($"Error loading BankAccount: {ex.Message}");
                LogException("BankAccount", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                //SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<BankAccountModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.BankAccount> request = BuildDataRequest();
                return await BankAccountService.GetBankAccountsAsync(request);
            }
            return new List<BankAccountModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading BankAccount...");
            if (await RefreshAsync())
            {
                EndStatusMessage("BankAccount loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected BankAccount?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} BankAccount...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} BankAccount...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} BankAccount: {ex.Message}");
                    LogException("BankAccounts", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} BankAccount deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<BankAccountModel> models)
        {
            foreach (var model in models)
            {
                await BankAccountService.DeleteBankAccountAsync(model);
            }
        }

        private DataRequest<Data.BankAccount> BuildDataRequest()
        {
            return new DataRequest<Data.BankAccount>()
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
