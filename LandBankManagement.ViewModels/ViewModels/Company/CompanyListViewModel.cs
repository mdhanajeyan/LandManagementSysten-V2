using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class CompanyListArgs
    {
        static public CompanyListArgs CreateEmpty() => new CompanyListArgs { IsEmpty = true };

        public CompanyListArgs()
        {
            OrderBy = r => r.Name;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Company, object>> OrderBy { get; set; }
        public Expression<Func<Data.Company, object>> OrderByDesc { get; set; }
    }
    public class CompanyListViewModel : GenericListViewModel<CompanyModel>
    {

        public ICompanyService CompanyService { get; }
        public CompanyListArgs ViewModelArgs { get; private set; }

        public CompanyListViewModel(ICompanyService companyService, ICommonServices commonServices) : base(commonServices)
        {
            CompanyService = companyService;
        }
        public async Task LoadAsync(CompanyListArgs args)
        {
            ViewModelArgs = args ?? CompanyListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Company...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Company loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CompanyListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public CompanyListArgs CreateArgs()
        {
            return new CompanyListArgs
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
                ShowProgressRing();
                await Task.Delay(100);
                Items = await GetItemsAsync();
                HideProgressRing();
            }
            catch (Exception ex)
            {
                HideProgressRing();
                Items = new List<CompanyModel>();
                StatusError($"Error loading Company: {ex.Message}");
                LogException("Company", "Refresh", ex);
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

        private async Task<IList<CompanyModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Company> request = BuildDataRequest();
                return await CompanyService.GetCompaniesAsync(request);
            }
            return new List<CompanyModel>();
        }

        //public ICommand OpenInNewViewCommand => new RelayCommand(OnOpenInNewView);
        //private async void OnOpenInNewView()
        //{
        //    if (SelectedItem != null)
        //    {
        //        await NavigationService.CreateNewViewAsync<CompanyViewModel>(new PartyDetailsArgs { PartyId = SelectedItem.CompanyId });
        //    }
        //}

        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<CompanyViewModel>(new CompanyArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Company...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Company loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected Company?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Company...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Company...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Company: {ex.Message}");
                    LogException("Companys", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Company deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<CompanyModel> models)
        {
            foreach (var model in models)
            {
                await CompanyService.DeleteCompanyAsync(model);
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

        private DataRequest<Data.Company> BuildDataRequest()
        {
            return new DataRequest<Data.Company>()
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
