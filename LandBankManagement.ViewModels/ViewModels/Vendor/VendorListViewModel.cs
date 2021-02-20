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
    public class VendorListArgs
    {
        static public VendorListArgs CreateEmpty() => new VendorListArgs { IsEmpty = true };

        public VendorListArgs()
        {
            OrderBy = r => r.VendorName;
        }
        public bool FromPropertyCheckList { get; set; } = false;
        public int SelectedPageIndex { get; set; } = 0;
        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Vendor, object>> OrderBy { get; set; }
        public Expression<Func<Vendor, object>> OrderByDesc { get; set; }
    }

    public class VendorListViewModel : GenericListViewModel<VendorModel>
    {
        private VendorViewModel VendorViewModel { get; set; }
        public VendorListViewModel( IVendorService vendorService, ICommonServices commonServices, VendorViewModel vendorViewModel) : base(commonServices)
        {
            VendorService = vendorService;
            VendorViewModel = vendorViewModel;
        }

        public IVendorService VendorService { get; }

        public VendorListArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(VendorListArgs args)
        {
            ViewModelArgs = args ?? VendorListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Vendors...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Vendors loaded");
            }
        }
        public void Unload()
        {
            if(ViewModelArgs!=null)
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<VendorListViewModel>(this, OnMessage);
           // MessageService.Subscribe<VendorDetailsViewModel>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public VendorListArgs CreateArgs()
        {
            return new VendorListArgs
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
                VendorViewModel.ShowProgressRing();
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<VendorModel>();
                StatusError($"Error loading Vendors: {ex.Message}");
                LogException("Vendors", "Refresh", ex);
                isOk = false;
            }
            finally {
                VendorViewModel.HideProgressRing();
            }
            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                //  SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }
        public async void OnSelectedRow(VendorModel model)
        {
            await VendorViewModel.PopulateDetails(model);
        }
        private async Task<IList<VendorModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Vendor> request = BuildDataRequest();
                return await VendorService.GetVendorsAsync(request);
            }
            return new List<VendorModel>();
        }
        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<CompanyViewModel>(new CompanyArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Vendors...");
            if (await RefreshAsync())
            {
                EndStatusMessage("vendor loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected Vendors?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Vendors...");
                       // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Vendors...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Vendors: {ex.Message}");
                    LogException("Vendors", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Vendors deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<VendorModel> models)
        {
            foreach (var model in models)
            {
                await VendorService.DeleteVendorAsync(model);
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

        private DataRequest<Vendor> BuildDataRequest()
        {
            return new DataRequest<Vendor>()
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
