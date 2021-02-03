using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class PropertyMergeViewModel : ViewModelBase
    {
        IPropertyMergeService PropertyMergeService { get; }
        public PropertyMergeListViewModel PropertyMergeList { get; set; }

        public PropertyMergeDetailsViewModel PropertyMergeDetails { get; set; }
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
        public PropertyMergeViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IPropertyMergeService propertyMergeService) : base(commonServices)
        {
            PropertyMergeService = propertyMergeService;
           
            PropertyMergeDetails = new PropertyMergeDetailsViewModel(dropDownService, PropertyMergeService, filePickerService, commonServices, this);
            PropertyMergeList = new PropertyMergeListViewModel(PropertyMergeService, commonServices, this);
        }

        public async Task LoadAsync(PropertyMergeListArgs args)
        {
            await PropertyMergeDetails.LoadAsync();
            await PropertyMergeList.LoadAsync(args);
        }
        public void Unload()
        {
            PropertyMergeList.Unload();
        }
        int noOfApiCalls = 0;
        public void ShowProgressRing()
        {
            noOfApiCalls++;
               ProgressRingActive = true;
            ProgressRingVisibility = true;
        }
        public void HideProgressRing()
        {
            if (noOfApiCalls > 1)
            {
                noOfApiCalls--;
                return;
            }
            else
                noOfApiCalls--;
            ProgressRingActive = false;
            ProgressRingVisibility = false;
        }
        public void Subscribe()
        {
            MessageService.Subscribe<PropertyMergeListViewModel>(this, OnMessage);
            PropertyMergeList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            PropertyMergeList.Unsubscribe();

        }

        private async void OnMessage(PropertyMergeListViewModel viewModel, string message, object args)
        {
            if (viewModel == PropertyMergeList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = PropertyMergeList.SelectedItem;
            if (!PropertyMergeList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(PropertyMergeModel selected)
        {
            try
            {
                SelectedPivotIndex = 1;
                ShowProgressRing();
                var model = await PropertyMergeService.GetPropertyMergeAsync(selected.PropertyMergeId);
                selected.Merge(model);
                PropertyMergeDetails.Item = model;
                
                PropertyMergeDetails.PropertyList = model.propertyMergeLists;
               
            }
            catch (Exception ex)
            {
                LogException("PropertyMerge", "Load Details", ex);
            }
            finally
            {
                HideProgressRing();
            }
        }

        public async Task ClonePropertyMerge(int id)
        {

            ShowProgressRing();
            var item = await PropertyMergeService.GetPropertyMergeAsync(id);
            HideProgressRing();
            item.PropertyMergeId = 0;
            item.PropertyMergeDealName = "";
            item.PropertyMergeGuid = Guid.Empty;
            foreach (var obj in item.propertyMergeLists)
            {
                obj.PropertyMergeListId = 0;
                obj.PropertyMergeGuid = Guid.Empty;
            }
            PropertyMergeDetails.Item = item;
            PropertyMergeDetails.PropertyList = item.propertyMergeLists;
            SelectedPivotIndex = 1;
        }
    }
}
