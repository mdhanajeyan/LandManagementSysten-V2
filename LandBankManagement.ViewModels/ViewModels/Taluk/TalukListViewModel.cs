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
    public class TalukListArgs
    {
        static public TalukListArgs CreateEmpty() => new TalukListArgs { IsEmpty = true };

        public TalukListArgs()
        {
            OrderBy = r => r.TalukName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Taluk, object>> OrderBy { get; set; }
        public Expression<Func<Data.Taluk, object>> OrderByDesc { get; set; }
    }
    public class TalukListViewModel : GenericListViewModel<TalukModel>
    {
        public ITalukService TalukService { get; }
        public TalukListArgs ViewModelArgs { get; private set; }

        public TalukListViewModel(ITalukService talukService, ICommonServices commonServices) : base(commonServices)
        {
            TalukService = talukService;
        }
        public async Task LoadAsync(TalukListArgs args)
        {
            ViewModelArgs = args ?? TalukListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Taluk...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Taluk loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<TalukListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public TalukListArgs CreateArgs()
        {
            return new TalukListArgs
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
                Items = new List<TalukModel>();
                StatusError($"Error loading Taluk: {ex.Message}");
                LogException("Taluk", "Refresh", ex);
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

        private async Task<IList<TalukModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Taluk> request = BuildDataRequest();
                return await TalukService.GetTaluksAsync(request);
            }
            return new List<TalukModel>();
        }
              

        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Taluk...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Taluk loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected Taluk?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Taluk...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Taluk...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Taluk: {ex.Message}");
                    LogException("Taluks", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Taluk deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<TalukModel> models)
        {
            foreach (var model in models)
            {
                await TalukService.DeleteTalukAsync(model);
            }
        }       

        private DataRequest<Data.Taluk> BuildDataRequest()
        {
            return new DataRequest<Data.Taluk>()
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
