using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class PartyViewModel : ViewModelBase
    {
        public IPartyService PartyService { get; }

        public PartyListViewModel PartyList { get; set; }
        public PartyDetailsViewModel PartyDetails { get; set; }
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
        public PartyViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IPartyService partyService,IVendorService vendorService,IDropDownService dropDownService,IPropertyService propertyService) : base(commonServices)
        {

            PartyService = partyService;
            PartyList = new PartyListViewModel(partyService, commonServices,this);
            PartyDetails = new PartyDetailsViewModel(partyService, filePickerService, commonServices, PartyList, dropDownService, vendorService,this,propertyService);
        }

        public async Task LoadAsync(PartyListArgs args)
        {
            await PartyList.LoadAsync(args);
        }
        public void Unload()
        {
            PartyList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PartyListViewModel>(this, OnMessage);
            PartyList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            PartyList.Unsubscribe();
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
        private async void OnMessage(PartyListViewModel viewModel, string message, object args)
        {
            if (viewModel == PartyList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            var selected = PartyList.SelectedItem;
            if (!PartyList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            //if (PartyDetails.IsEditMode)
            //{
            //    StatusReady();
            //    PartyDetails.CancelEdit();
            //}

            //var selected = PartyList.SelectedItem;
            //if (!PartyList.IsMultipleSelection)
            //{
            //    if (selected != null && !selected.IsEmpty)
            //    {
            //        await PopulateDetails(selected);

            //    }
            //}
            //PartyDetails.Item = selected;
        }

        public async Task PopulateDetails(PartyModel selected)
        {
            try
            {
                ShowProgressRing();
                var model = await PartyService.GetPartyAsync(selected.PartyId);
                selected.Merge(model);
                PartyDetails.Item = model;
                PartyDetails.DocList = model.partyDocuments;
                if (model.partyDocuments != null)
                {
                    for (int i = 0; i < PartyDetails.DocList.Count; i++)
                    {
                        PartyDetails.DocList[i].Identity = i + 1;
                    }
                }
                SelectedPivotIndex = 1;
            }
            catch (Exception ex)
            {
                LogException("Partys", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }

    }
}
