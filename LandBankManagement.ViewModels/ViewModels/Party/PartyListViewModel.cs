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

    public class PartyListArgs
    {
        static public PartyListArgs CreateEmpty() => new PartyListArgs { IsEmpty = true };

        public PartyListArgs()
        {
            OrderBy = r => r.PartyFirstName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Party, object>> OrderBy { get; set; }
        public Expression<Func<Data.Party, object>> OrderByDesc { get; set; }
    }

    public class PartyListViewModel : GenericListViewModel<PartyModel>
    {

        public PartyListViewModel(IPartyService partyService, ICommonServices commonServices) : base(commonServices)
        {
            PartyService = partyService;
        }

        public IPartyService PartyService { get; }

        public PartyListArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(PartyListArgs args)
        {
            ViewModelArgs = args ?? PartyListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Partys...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Partys loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PartyListViewModel>(this, OnMessage);
            // MessageService.Subscribe<PartyDetailsViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public PartyListArgs CreateArgs()
        {
            return new PartyListArgs
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
                Items = new List<PartyModel>();
                StatusError($"Error loading Partys: {ex.Message}");
                LogException("Partys", "Refresh", ex);
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

        private async Task<IList<PartyModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Party> request = BuildDataRequest();
                return await PartyService.GetPartiesAsync(request);
            }
            return new List<PartyModel>();
        }
        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<CompanyViewModel>(new CompanyArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Party List...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Party List Loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected Party?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Partys...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Partys...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Partys: {ex.Message}");
                    LogException("Partys", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Partys deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<PartyModel> models)
        {
            foreach (var model in models)
            {
                await PartyService.DeletePartyAsync(model);
            }
        }

        //private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        //{
        //    DataRequest<Party> request = BuildDataRequest();
        //    foreach (var range in ranges)
        //    {
        //        await PartyService.DeletePartyRangeAsync(range.Index, range.Length, request);
        //    }
        //}

        private DataRequest<Data.Party> BuildDataRequest()
        {
            return new DataRequest<Data.Party>()
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
