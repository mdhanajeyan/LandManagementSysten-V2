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
    public class PropertyTypeListArgs
    {
        static public PropertyTypeListArgs CreateEmpty() => new PropertyTypeListArgs { IsEmpty = true };

        public PropertyTypeListArgs()
        {
            OrderBy = r => r.PropertyTypeText;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.PropertyType, object>> OrderBy { get; set; }
        public Expression<Func<Data.PropertyType, object>> OrderByDesc { get; set; }
    }
    public class PropertyTypeListViewModel : GenericListViewModel<PropertyTypeModel>
    {
        public IPropertyTypeService PropertyTypeService { get; }
        public PropertyTypeListArgs ViewModelArgs { get; private set; }

        private PropertyTypeViewModel PropertyTypeViewModel { get; set; }
        public PropertyTypeListViewModel(IPropertyTypeService propertyTypeService, ICommonServices commonServices, PropertyTypeViewModel propertyTypeViewModel) : base(commonServices)
        {
            PropertyTypeService = propertyTypeService;
            PropertyTypeViewModel = propertyTypeViewModel;
        }
        public async Task LoadAsync(PropertyTypeListArgs args)
        {
            ViewModelArgs = args ?? PropertyTypeListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading PropertyType...");
            if (await RefreshAsync())
            {
                EndStatusMessage("PropertyType loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PropertyTypeListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public PropertyTypeListArgs CreateArgs()
        {
            return new PropertyTypeListArgs
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
                PropertyTypeViewModel.ShowProgressRing();
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<PropertyTypeModel>();
                StatusError($"Error loading PropertyType: {ex.Message}");
                LogException("PropertyType", "Refresh", ex);
                isOk = false;
            }
            finally { PropertyTypeViewModel.HideProgressRing(); }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                //  SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<PropertyTypeModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.PropertyType> request = BuildDataRequest();
                return await PropertyTypeService.GetPropertyTypesAsync(request);
            }
            return new List<PropertyTypeModel>();
        }

        //public ICommand OpenInNewViewCommand => new RelayCommand(OnOpenInNewView);
        //private async void OnOpenInNewView()
        //{
        //    if (SelectedItem != null)
        //    {
        //        await NavigationService.CreateNewViewAsync<PropertyTypeViewModel>(new PartyDetailsArgs { PartyId = SelectedItem.PropertyTypeId });
        //    }
        //}

        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<PropertyTypeViewModel>(new PropertyTypeArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading PropertyType...");
            if (await RefreshAsync())
            {
                EndStatusMessage("PropertyType loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected PropertyType?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} PropertyType...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} PropertyType...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} PropertyType: {ex.Message}");
                    LogException("PropertyTypes", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} PropertyType deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<PropertyTypeModel> models)
        {
            foreach (var model in models)
            {
                await PropertyTypeService.DeletePropertyTypeAsync(model);
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

        private DataRequest<Data.PropertyType> BuildDataRequest()
        {
            return new DataRequest<Data.PropertyType>()
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
