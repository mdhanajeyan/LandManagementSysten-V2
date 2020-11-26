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
    public class HobliListArgs
    {
        static public HobliListArgs CreateEmpty() => new HobliListArgs { IsEmpty = true };

        public HobliListArgs()
        {
            OrderBy = r => r.HobliName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Hobli, object>> OrderBy { get; set; }
        public Expression<Func<Data.Hobli, object>> OrderByDesc { get; set; }
    }
    public class HobliListViewModel : GenericListViewModel<HobliModel>
    {
        public IHobliService HobliService { get; }
        public HobliListArgs ViewModelArgs { get; private set; }

        public HobliListViewModel(IHobliService hobliService, ICommonServices commonServices) : base(commonServices)
        {
            HobliService = hobliService;
        }
        public async Task LoadAsync(HobliListArgs args)
        {
            ViewModelArgs = args ?? HobliListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Hobli...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Hobli loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<HobliListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public HobliListArgs CreateArgs()
        {
            return new HobliListArgs
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
                Items = new List<HobliModel>();
                StatusError($"Error loading Hobli: {ex.Message}");
                LogException("Hobli", "Refresh", ex);
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

        private async Task<IList<HobliModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Hobli> request = BuildDataRequest();
                return await HobliService.GetHoblisAsync(request);
            }
            return new List<HobliModel>();
        }
              

        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Hobli...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Hobli loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected Hobli?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Hobli...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Hobli...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Hobli: {ex.Message}");
                    LogException("Hoblis", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Hobli deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<HobliModel> models)
        {
            foreach (var model in models)
            {
                await HobliService.DeleteHobliAsync(model);
            }
        }       

        private DataRequest<Data.Hobli> BuildDataRequest()
        {
            return new DataRequest<Data.Hobli>()
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
