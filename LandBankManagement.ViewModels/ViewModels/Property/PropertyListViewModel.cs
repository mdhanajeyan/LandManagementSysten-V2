﻿using LandBankManagement.Data;
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
    public class PropertyListArgs
    {
        static public PropertyListArgs CreateEmpty() => new PropertyListArgs { IsEmpty = true };

        public PropertyListArgs()
        {
            OrderBy = r => r.PropertyId;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Property, object>> OrderBy { get; set; }
        public Expression<Func<Data.Property, object>> OrderByDesc { get; set; }
    }
    public class PropertyListViewModel : GenericListViewModel<PropertyModel>
    {
        public IPropertyService PropertyService { get; }
        public PropertyListArgs ViewModelArgs { get; private set; }

        public PropertyListViewModel(IPropertyService propertyService, ICommonServices commonServices) : base(commonServices)
        {
            PropertyService = propertyService;
        }
        public async Task LoadAsync(PropertyListArgs args)
        {
            ViewModelArgs = args ?? PropertyListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Property...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Property loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PropertyListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public PropertyListArgs CreateArgs()
        {
            return new PropertyListArgs
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
                Items = new List<PropertyModel>();
                StatusError($"Error loading Property: {ex.Message}");
                LogException("Property", "Refresh", ex);
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

        private async Task<IList<PropertyModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Property> request = BuildDataRequest();
                return await PropertyService.GetPropertiesAsync(request);
            }
            return new List<PropertyModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Property...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Property loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected Property?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Property...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Property...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Property: {ex.Message}");
                    LogException("Propertys", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Property deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<PropertyModel> models)
        {
            foreach (var model in models)
            {
                await PropertyService.DeletePropertyAsync(model);
            }
        }

        private DataRequest<Data.Property> BuildDataRequest()
        {
            return new DataRequest<Data.Property>()
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