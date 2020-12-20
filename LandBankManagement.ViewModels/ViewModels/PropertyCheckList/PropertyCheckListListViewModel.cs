using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LandBankManagement.ViewModels
{
    public class PropertyCheckListListArgs
    {
        static public PropertyCheckListListArgs CreateEmpty() => new PropertyCheckListListArgs { IsEmpty = true };

        public PropertyCheckListListArgs()
        {
            OrderBy = r => r.PropertyCheckListId;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.PropertyCheckList, object>> OrderBy { get; set; }
        public Expression<Func<Data.PropertyCheckList, object>> OrderByDesc { get; set; }
    }
    
    public class PropertyCheckListListViewModel : GenericListViewModel<PropertyCheckListModel>
    {
        public IPropertyCheckListService PropertyCheckListService { get; }
        public PropertyCheckListListArgs ViewModelArgs { get; private set; }       
               
        public PropertyCheckListListViewModel(IPropertyCheckListService propertyService, ICommonServices commonServices) : base(commonServices)
        {
            PropertyCheckListService = propertyService;
            
        }
        public async Task LoadAsync(PropertyCheckListListArgs args)
        {
            ViewModelArgs = args ?? PropertyCheckListListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Property...");
            EndStatusMessage("Property loaded");
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PropertyListViewModel>(this, OnMessage);

        }

        public async void SaveStatusAndRemarks(int id) {
            var modal = Items.Where(x => x.PropertyCheckListId == id).FirstOrDefault();
            await PropertyCheckListService.UpdatePropertyCheckListStatusAsync(modal.PropertyCheckListId, modal.Status, modal.Remarks);
            await RefreshAsync();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public PropertyCheckListListArgs CreateArgs()
        {
            return new PropertyCheckListListArgs
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
                StartStatusMessage("Loading PropertyCheckList  List...");
                var StatusList = new ObservableCollection<ComboBoxOptions>();
                StatusList.Add(new ComboBoxOptions { Id = 0, Description = "" });
                StatusList.Add(new ComboBoxOptions { Id = 1, Description = "Pending" });
                StatusList.Add(new ComboBoxOptions { Id = 2, Description = "Dropped" });
                StatusList.Add(new ComboBoxOptions { Id = 3, Description = "Procured" });

                var modals = await GetItemsAsync();             

                foreach (var obj in modals) {
                    obj.StatusOption = StatusList;
                }

                Items = modals;

                EndStatusMessage("PropertyCheckList List loaded");
            }
            catch (Exception ex)
            {
                Items = new List<PropertyCheckListModel>();
                StatusError($"Error loading PropertyCheckList: {ex.Message}");
                LogException("PropertyCheckList", "Refresh", ex);
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

        private async Task<IList<PropertyCheckListModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.PropertyCheckList> request = BuildDataRequest();
                return await PropertyCheckListService.GetPropertyCheckListAsync(request);
            }
            return new List<PropertyCheckListModel>();
        }


        protected override async void OnNew()
        {
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
            //StatusReady();
            //if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected Property?", "Ok", "Cancel"))
            //{
            //    int count = 0;
            //    try
            //    {
            //        if (SelectedIndexRanges != null)
            //        {
            //            count = SelectedIndexRanges.Sum(r => r.Length);
            //            StartStatusMessage($"Deleting {count} Property...");
            //            // await DeleteRangesAsync(SelectedIndexRanges);
            //            MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
            //        }
            //        else if (SelectedItems != null)
            //        {
            //            count = SelectedItems.Count();
            //            StartStatusMessage($"Deleting {count} Property...");
            //            await DeleteItemsAsync(SelectedItems);
            //            MessageService.Send(this, "ItemsDeleted", SelectedItems);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        StatusError($"Error deleting {count} Property: {ex.Message}");
            //        LogException("Propertys", "Delete", ex);
            //        count = 0;
            //    }
            //    await RefreshAsync();
            //    SelectedIndexRanges = null;
            //    SelectedItems = null;
            //    if (count > 0)
            //    {
            //        EndStatusMessage($"{count} Property deleted");
            //    }
            //}
        }

        private async Task DeleteItemsAsync(IEnumerable<PropertyModel> models)
        {
            //foreach (var model in models)
            //{
            //    await PropertyService.DeletePropertyAsync(model);
            //}
        }

        private DataRequest<Data.PropertyCheckList> BuildDataRequest()
        {
            return new DataRequest<Data.PropertyCheckList>()
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
