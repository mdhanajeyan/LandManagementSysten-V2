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

        public Expression<Func<Party, object>> OrderBy { get; set; }
        public Expression<Func<Party, object>> OrderByDesc { get; set; }
    }

    public class PartyListViewModel : GenericListViewModel<PartyModel> {
        public IPartyService PartyService { get; }

        public PartyListArgs ViewModelArgs { get; private set; }
        public PartyListViewModel(IPartyService partyService, ICommonServices commonServices) : base(commonServices)
        {
            PartyService = partyService;
        }

        public async Task LoadAsync(PartyListArgs args)
        {
            ViewModelArgs = args ?? PartyListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Parties...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Parties loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<VendorListViewModel>(this, OnMessage);
            MessageService.Subscribe<VendorDetailsViewModel>(this, OnMessage);
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
                StatusError($"Error loading Parties: {ex.Message}");
                LogException("Parties", "Refresh", ex);
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

        private async Task<IList<PartyModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Party> request = BuildDataRequest();
                return await PartyService.GetPartiesAsync(request);
            }
            return new List<PartyModel>();
        }

        public ICommand OpenInNewViewCommand => new RelayCommand(OnOpenInNewView);
        private async void OnOpenInNewView()
        {
            if (SelectedItem != null)
            {
                await NavigationService.CreateNewViewAsync<PartyDetailsViewModel>(new PartyDetailsArgs { PartyId = SelectedItem.PartyId });
            }
        }

        protected override async void OnNew()
        {

            await NavigationService.CreateNewViewAsync<PartyDetailsViewModel>(new PartyDetailsArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Vendors...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Companies loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected Parties?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Parties...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Parties...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Parties: {ex.Message}");
                    LogException("Parties", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Parties deleted");
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
        //    DataRequest<Vendor> request = BuildDataRequest();
        //    foreach (var range in ranges)
        //    {
        //        await VendorService.DeleteVendorRangeAsync(range.Index, range.Length, request);
        //    }
        //}

        private DataRequest<Party> BuildDataRequest()
        {
            return new DataRequest<Party>()
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
