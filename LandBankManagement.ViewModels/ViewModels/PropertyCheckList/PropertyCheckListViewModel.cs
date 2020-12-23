using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    
    public class PropertyCheckListViewModel : ViewModelBase
    {
        IPropertyService PropertyService { get; }
        IPropertyCheckListService PropertyCheckListService { get; }
        public PropertyCheckListListViewModel ViewModelList { get; set; }
        public PropertyCheckListDetailsViewModel PropertyCheckListDetials { get; set; }
        private bool _progressRingVisibility;
        public bool ProgressRingVisibility
        {
            get => _progressRingVisibility;
            set => Set(ref _progressRingVisibility, value);
        }

        private bool _progressRingActive;
        public bool ProgressRingActive
        {
            get => _progressRingActive;
            set => Set(ref _progressRingActive, value);
        }
        public PropertyCheckListViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IPropertyService propertyService, IPropertyCheckListService propertyCheckListService) : base(commonServices)
        {
            PropertyService = propertyService;
            PropertyCheckListService = propertyCheckListService;
            ViewModelList = new PropertyCheckListListViewModel(PropertyCheckListService,commonServices,this);
            PropertyCheckListDetials = new PropertyCheckListDetailsViewModel(dropDownService, PropertyCheckListService, propertyService, filePickerService, commonServices, ViewModelList,this);
        }

        public async void LoadAsync(PropertyCheckListListArgs args)
        {
           await PropertyCheckListDetials.LoadAsync();
           await ViewModelList.LoadAsync(args);
        }
        public void ShowProgressRing()
        {
            ProgressRingActive = true;
            ProgressRingVisibility = true;
        }
        public void HideProgressRing()
        {
            ProgressRingActive = false;
            ProgressRingVisibility = false;
        }
        public void Unload()
        {
            ViewModelList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PropertyCheckListListViewModel>(this, OnMessage);
            ViewModelList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            ViewModelList.Unsubscribe();

        }
        private async void OnMessage(PropertyCheckListListViewModel viewModel, string message, object args)
        {
            if (viewModel == ViewModelList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = ViewModelList.SelectedItem;
            if (!ViewModelList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(PropertyCheckListModel selected)
        {
            try
            {
                ShowProgressRing();
                // selected.Merge(model);
                await PropertyCheckListDetials.LoadPropertyCheckList(selected.PropertyCheckListId);
                SelectedPivotIndex = 1;
            }
            catch (Exception ex)
            {
                LogException("Payments", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }




    }
}
