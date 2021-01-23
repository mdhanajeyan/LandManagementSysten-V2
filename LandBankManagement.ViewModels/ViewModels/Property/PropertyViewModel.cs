﻿using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class PropertyViewModel : ViewModelBase
    {


        IPropertyService PropertyService { get; }
        public PropertyListViewModel PropertyList { get; set; }

        public PropertyDetailsViewModel PropertyDetials { get; set; }
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
        public PropertyViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IPropertyService propertyService) : base(commonServices)
        {
            PropertyService = propertyService;
            PropertyList = new PropertyListViewModel(propertyService, commonServices,this);
            PropertyDetials = new PropertyDetailsViewModel(dropDownService, propertyService, filePickerService, commonServices, PropertyList,this);
        }

        public async Task LoadAsync(PropertyListArgs args)
        {
            if (args.FromParty)
            {
                SelectedPivotIndex = 1;
            }
            await PropertyDetials.LoadAsync(args.FromParty);           
            await PropertyList.LoadAsync(args);
            
        }
        public void Unload()
        {
            PropertyList.Unload();
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
            MessageService.Subscribe<PropertyListViewModel>(this, OnMessage);
            PropertyList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            PropertyList.Unsubscribe();

        }

        private async void OnMessage(PropertyListViewModel viewModel, string message, object args)
        {
            if (viewModel == PropertyList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = PropertyList.SelectedItem;
            if (!PropertyList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        public async Task PopulateDetails(PropertyModel selected)
        {
            try
            {
                if (selected == null)
                    return;
                SelectedPivotIndex = 1;
                ShowProgressRing();
                // var model = await PropertyService.GetPropertyAsync(selected.PropertyId);
                var modelList = await PropertyService.GetPropertyByGroupGuidAsync(selected.GroupGuid.GetValueOrDefault());
                HideProgressRing();
                PropertyDetials.PropertyList = modelList;
                var model = modelList[0];
                //selected.Merge(model);

                PropertyDetials.Item = model;
                await PropertyDetials.GetPropertyParties(model.PropertyId);
                PropertyDetials.DocList = model.PropertyDocuments;
                if (model.PropertyDocuments != null)
                {
                    for (int i = 0; i < PropertyDetials.DocList.Count; i++)
                    {
                        PropertyDetials.DocList[i].Identity = i + 1;
                    }
                }
               
            }
            catch (Exception ex)
            {
                LogException("Property", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }

        public async void LoadPropertyForNewDocumentType(int id)
        {
            ShowProgressRing();
            var model = await PropertyService.GetPropertyAsync(id);
            HideProgressRing();
            model.DocumentTypeId = 0;
            PropertyDetials.Item = model;
            await PropertyDetials.GetPropertyParties(model.PropertyId);
            PropertyDetials.DocList = model.PropertyDocuments;
            if (model.PropertyDocuments != null)
            {
                for (int i = 0; i < PropertyDetials.DocList.Count; i++)
                {
                    PropertyDetials.DocList[i].Identity = i + 1;
                    PropertyDetials.DocList[i].blobId = 0;
                }
            }
            if (PropertyDetials.PartyList != null) {
                foreach (var party in PropertyDetials.PartyList) {
                    party.PropertyId = 0;
                    party.PropertyPartyId = 0;
                }
            }
            PropertyDetials.Item.PropertyId=0;
            PropertyDetials.Item.GroupGuid =null;
            SelectedPivotIndex = 1;
        }
    }
}
