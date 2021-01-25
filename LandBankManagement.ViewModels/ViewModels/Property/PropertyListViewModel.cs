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
    public class PropertyListArgs
    {
        static public PropertyListArgs CreateEmpty() => new PropertyListArgs { IsEmpty = true };

        public PropertyListArgs()
        {
            OrderBy = r => r.PropertyId;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }
        public bool FromParty { get; set; } = false;
        
        public Expression<Func<Data.Property, object>> OrderBy { get; set; }
        public Expression<Func<Data.Property, object>> OrderByDesc { get; set; }
    }
   
    public class PropertyListViewModel : GenericListViewModel<PropertyModel>
    {
        public IPropertyService PropertyService { get; }
        public PropertyListArgs ViewModelArgs { get; private set; }
        public CostDetailsViewModel CostDetails { get; set; }
        private bool _popupOpened = false;
        public bool PopupOpened
        {
            get => _popupOpened;
            set => Set(ref _popupOpened, value);
        }
        
        private IList<PropertyModel> _selectedProperty = null;
        public IList<PropertyModel> SelectedProperty
        {
            get => _selectedProperty;
            set => Set(ref _selectedProperty, value);
        }

        public PropertyViewModel PropertyView { get; set; }

        private ObservableCollection<PropertyListingModel>  _properties = null;
        public ObservableCollection<PropertyListingModel> Properties
        {
            get => _properties;
            set => Set(ref _properties, value);
        }
        public PropertyListViewModel(IPropertyService propertyService, ICommonServices commonServices, PropertyViewModel propertyView) : base(commonServices)
        {
            PropertyService = propertyService;
            CostDetails = new CostDetailsViewModel(propertyService, commonServices,this);
            PropertyView = propertyView;
        }
        public async Task LoadAsync(PropertyListArgs args)
        {
            ViewModelArgs = args ?? PropertyListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            //StartStatusMessage("Loading Property...");
            //await RefreshAsync();
            //EndStatusMessage("Property loaded");            
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
                StartStatusMessage("Loading Property List...");
                PropertyView.ShowProgressRing();
                var ItemsList = await GetItemsAsync();
              
                var list = ItemsList.GroupBy(x => x.GroupGuid).Select(x => x);
                List<PropertyModel> propertyModels = new List<PropertyModel>();
                foreach (var obj in list)
                {
                    //var item = new PropertyModel();
                    //if (obj.Count() > 1)
                    //{
                    //    item = obj.First();
                    //    item.Children = obj.Skip(1);
                    //}
                    //else
                    //    item = obj.First();

                    var item = new PropertyModel();
                    item = obj.First();
                    item.Children = obj;

                    propertyModels.Add(item);
                }
                Items = propertyModels;



                Properties = new ObservableCollection<PropertyListingModel>();
                int i = 0;
                foreach (var obj in list)
                {
                    var item = new PropertyListingModel();
                    item.id = i;
                    item.GroupGuid = obj.First().GroupGuid.Value;
                    item.Children = obj;
                    item.ShowChildren = false;
                    item.HideChildren = true;
                    Properties.Add(item);
                    i++;
                }

                EndStatusMessage("Property List loaded");
            }
            catch (Exception ex)
            {
                Items = new List<PropertyModel>();
                StatusError($"Error loading Property: {ex.Message}");
                LogException("Property", "Refresh", ex);
                isOk = false;
            }
            finally {
                PropertyView.HideProgressRing();
            }
            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyChanges();
            //NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        public void ChangeVisibility(int id) {
            Properties[id].ShowChildren = !Properties[id].ShowChildren;
            Properties[id].HideChildren = !Properties[id].HideChildren;
            var temp = Properties;
            Properties = null;
            Properties = temp;
        }

        private async Task<IList<PropertyModel>> GetItemsAsync()
        {
            ViewModelArgs = new PropertyListArgs { IsEmpty = false };
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Property> request = BuildDataRequest();
                return await PropertyService.GetPropertiesAsync(request);
            }
            return new List<PropertyModel>();
        }

        public async void PopulateProperty(PropertyModel model) {
            await PropertyView.PopulateDetails(model);
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
