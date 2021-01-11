using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{ 
    public class PropertyTypeViewModel : ViewModelBase
    {
        IPropertyTypeService PropertyTypeService { get; }
        public PropertyTypeListViewModel PropertyTypeList { get; set; }

        public PropertyTypeDetailsViewModel PropertyTypeDetials { get; set; }
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
        public PropertyTypeViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IPropertyTypeService propertyTypeService) : base(commonServices)
        {
            PropertyTypeService = propertyTypeService;
            PropertyTypeList = new PropertyTypeListViewModel(propertyTypeService, commonServices,this);
            PropertyTypeDetials = new PropertyTypeDetailsViewModel(propertyTypeService, filePickerService, commonServices, PropertyTypeList,this);
        }

        public async Task LoadAsync(PropertyTypeListArgs args)
        {
            await PropertyTypeDetials.LoadAsync();
            await PropertyTypeList.LoadAsync(args);
        }
        public void Unload()
        {
            PropertyTypeList.Unload();
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
            MessageService.Subscribe<PropertyTypeListViewModel>(this, OnMessage);
            PropertyTypeList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            PropertyTypeList.Unsubscribe();

        }

        private async void OnMessage(PropertyTypeListViewModel viewModel, string message, object args)
        {
            if (viewModel == PropertyTypeList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = PropertyTypeList.SelectedItem;
            if (!PropertyTypeList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(PropertyTypeModel selected)
        {
            try
            {
                ShowProgressRing();
                var model = await PropertyTypeService.GetPropertyTypeAsync(selected.PropertyTypeId);
                selected.Merge(model);
                PropertyTypeDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("PropertyType", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }

    }
}
