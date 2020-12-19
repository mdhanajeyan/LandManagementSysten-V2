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
using System.Threading.Tasks;

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
        private IList<PropertyCheckListModel> _propertyModelList = null;
        public IList<PropertyCheckListModel> PropertyModelCheckList
        {
            get => _propertyModelList;
            set => Set(ref _propertyModelList, value);
        }

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

                Items = await GetItemsAsync();           


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

        public async Task LoadData()
        {
            var items = new List<PropertyCheckListModel>
            if (await RefreshAsync())
            {
                new PropertyCheckListModel
                {
                    PropertyName="Property CheckList 1",
                    PropertyDescription="test@test.com",
                    LandAreaInSqft="9087654400",
                    CompanyID=1,
                    AKarabAreaInAcres="CBE"
                }
            };
            Items = items;
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
