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
    public class VillageListArgs
    {
        static public VillageListArgs CreateEmpty() => new VillageListArgs { IsEmpty = true };

        public VillageListArgs()
        {
            OrderBy = r => r.VillageName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Village, object>> OrderBy { get; set; }
        public Expression<Func<Data.Village, object>> OrderByDesc { get; set; }
    }
   public class VillageListViewModel : GenericListViewModel<VillageModel>
    {
        public IVillageService VillageService { get; }
        public VillageListArgs ViewModelArgs { get; private set; }

        public VillageListViewModel(IVillageService villageService, ICommonServices commonServices) : base(commonServices)
        {
            VillageService = villageService;
        }
        public async Task LoadAsync(VillageListArgs args)
        {
            ViewModelArgs = args ?? VillageListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Village...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Village loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<VillageListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public VillageListArgs CreateArgs()
        {
            return new VillageListArgs
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
                Items = new List<VillageModel>();
                StatusError($"Error loading Village: {ex.Message}");
                LogException("Village", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<VillageModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Village> request = BuildDataRequest();
                return await VillageService.GetVillagesAsync(request);
            }
            return new List<VillageModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Village...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Village loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected Village?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Village...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Village...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Village: {ex.Message}");
                    LogException("Villages", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Village deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<VillageModel> models)
        {
            foreach (var model in models)
            {
                await VillageService.DeleteVillageAsync(model);
            }
        }

        private DataRequest<Data.Village> BuildDataRequest()
        {
            return new DataRequest<Data.Village>()
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
