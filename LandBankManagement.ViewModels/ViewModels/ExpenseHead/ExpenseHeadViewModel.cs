using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System.Windows.Input;

namespace LandBankManagement.ViewModels
{
    public class ExpenseHeadArgs
    {
        static public ExpenseHeadArgs CreateEmpty() => new ExpenseHeadArgs { IsEmpty = true };

        public ExpenseHeadArgs()
        {
            OrderBy = r => r.ExpenseHeadName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<ExpenseHead, object>> OrderBy { get; set; }
        public Expression<Func<ExpenseHead, object>> OrderByDesc { get; set; }
    }
    public class ExpenseHeadViewModel : GenericListViewModel<ExpenseHeadModel>
    {
        public IExpenseHeadService ExpenseHeadService { get; }
        public ExpenseHeadArgs ViewModelArgs { get; private set; }

        public ExpenseHeadViewModel(IExpenseHeadService expenseHeadService, ICommonServices commonServices) : base(commonServices)
        {
            ExpenseHeadService = expenseHeadService;
        }
        public async Task LoadAsync(ExpenseHeadArgs args)
        {
            ViewModelArgs = args ?? ExpenseHeadArgs.CreateEmpty();
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
            MessageService.Subscribe<ExpenseHeadViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public ExpenseHeadArgs CreateArgs()
        {
            return new ExpenseHeadArgs
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
                Items = new List<ExpenseHeadModel>();
                StatusError($"Error loading Expense Head: {ex.Message}");
                LogException("Expense Head", "Refresh", ex);
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

        private async Task<IList<ExpenseHeadModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.ExpenseHead> request = BuildDataRequest();
                return await ExpenseHeadService.GetExpenseHeadsAsync(request);
            }
            return new List<ExpenseHeadModel>();
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
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected ExpenseHead?", "Ok", "Cancel"))
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

        private DataRequest<ExpenseHead> BuildDataRequest()
        {
            return new DataRequest<ExpenseHead>()
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

        public ICommand SaveCommand => new RelayCommand(OnSave);

        public async Task<bool> SaveItemsAsync()
        {
            try
            {

                foreach (ExpenseHeadModel model in Items)
                {
                    StartStatusMessage($"Saving ExpenseHead '{model.ExpenseHeadName}'...");

                    await Task.Delay(100);
                    if (model.ExpenseHeadId <= 0)
                        await ExpenseHeadService.AddExpenseHeadAsync(model);
                    else
                        await ExpenseHeadService.UpdateExpenseHeadAsync(model);
                    EndStatusMessage("ExpenseHead saved");
                    LogInformation("ExpenseHead", "Save", "ExpenseHead saved successfully", $"ExpenseHead {model.ExpenseHeadId} '{model.ExpenseHeadName}' was saved successfully.");
                }

            }
            catch (Exception ex)
            {
                StatusError($"Error saving ExpenseHead Items : {ex.Message}");
                LogException("ExpenseHead", "Save All", ex);
                return false;
            }

            return true;
        }

        public void OnSave()
        {
            SaveCommand.Execute(SelectedItem);
        }
        public async Task<bool> SaveItemAsync(ExpenseHeadModel model)
        {
            try
            {
                StartStatusMessage("Saving ExpenseHead...");
                await Task.Delay(100);
                if (model.ExpenseHeadId <= 0)
                    await ExpenseHeadService.AddExpenseHeadAsync(model);
                else
                    await ExpenseHeadService.UpdateExpenseHeadAsync(model);
                EndStatusMessage("ExpenseHead saved");
                LogInformation("ExpenseHead", "Save", "ExpenseHead saved successfully", $"ExpenseHead {model.ExpenseHeadId} '{model.ExpenseHeadName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving ExpenseHead: {ex.Message}");
                LogException("ExpenseHead", "Save", ex);
                return false;
            }
        }

        protected override void OnNew()
        {
            //Implement adding new row to the data grid
        }
    }

}

