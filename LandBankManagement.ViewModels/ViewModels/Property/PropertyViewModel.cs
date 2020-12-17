using System;
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

        public PropertyViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IPropertyService propertyService) : base(commonServices)
        {
            PropertyService = propertyService;
            PropertyList = new PropertyListViewModel(propertyService, commonServices,this);
            PropertyDetials = new PropertyDetailsViewModel(dropDownService, propertyService, filePickerService, commonServices, PropertyList);
        }

        public async Task LoadAsync(PropertyListArgs args)
        {
            await PropertyDetials.LoadAsync();
            await PropertyList.LoadAsync(args);
            
        }
        public void Unload()
        {
            PropertyList.Unload();
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

        private async Task PopulateDetails(PropertyModel selected)
        {
            try
            {
                // var model = await PropertyService.GetPropertyAsync(selected.PropertyId);
                var modelList = await PropertyService.GetPropertyByGroupGuidAsync(selected.GroupGuid.GetValueOrDefault());
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
                SelectedPivotIndex = 1;
            }
            catch (Exception ex)
            {
                LogException("Property", "Load Details", ex);
            }
        }

        public async void LoadPropertyForNewDocumentType(int id)
        {
            var model = await PropertyService.GetPropertyAsync(id);

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
