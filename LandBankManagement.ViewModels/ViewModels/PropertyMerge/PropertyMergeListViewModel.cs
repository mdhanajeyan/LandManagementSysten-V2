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
    public class PropertyMergeListArgs
    {
        static public PropertyMergeListArgs CreateEmpty() => new PropertyMergeListArgs { IsEmpty = true };

        public PropertyMergeListArgs()
        {
            OrderBy = r => r.PropertyMergeDealName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.PropertyMerge, object>> OrderBy { get; set; }
        public Expression<Func<Data.PropertyMerge, object>> OrderByDesc { get; set; }
    }
    public class PropertyMergeListViewModel : GenericListViewModel<PropertyMergeModel>
    {
        public IPropertyMergeService PropertyMergeService { get; }
        public PropertyMergeListArgs ViewModelArgs { get; private set; }

        public PropertyMergeViewModel PropertyMergeViewModel { get; set; }
        public PropertyMergeListViewModel(IPropertyMergeService propertyMergeService, ICommonServices commonServices,  PropertyMergeViewModel propertyMergeViewModel) : base(commonServices)
        {
            PropertyMergeService = propertyMergeService;
            PropertyMergeViewModel = propertyMergeViewModel;
        }
        public async Task LoadAsync(PropertyMergeListArgs args)
        {
            ViewModelArgs = args ?? PropertyMergeListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            //StartStatusMessage("Loading PropertyMerge...");
            //if (await RefreshAsync())
            //{
            //    EndStatusMessage("PropertyMerge loaded");
            //}
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PropertyMergeListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public PropertyMergeListArgs CreateArgs()
        {
            return new PropertyMergeListArgs
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
                PropertyMergeViewModel.ShowProgressRing();
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<PropertyMergeModel>();
                StatusError($"Error loading PropertyMerge: {ex.Message}");
                LogException("PropertyMerge", "Refresh", ex);
                isOk = false;
            }
            finally
            {
                PropertyMergeViewModel.HideProgressRing();
            }
            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<PropertyMergeModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.PropertyMerge> request = BuildDataRequest();
                return await PropertyMergeService.GetPropertyMergeAsync(request);
            }
            return new List<PropertyMergeModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading PropertyMerge...");
            if (await RefreshAsync())
            {
                EndStatusMessage("PropertyMerge loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected PropertyMerge?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} PropertyMerge...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} PropertyMerge...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} PropertyMerge: {ex.Message}");
                    LogException("PropertyMerges", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} PropertyMerge deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<PropertyMergeModel> models)
        {
            foreach (var model in models)
            {
                await PropertyMergeService.DeletePropertyMergeAsync(model);
            }
        }

        private DataRequest<Data.PropertyMerge> BuildDataRequest()
        {
            return new DataRequest<Data.PropertyMerge>()
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
