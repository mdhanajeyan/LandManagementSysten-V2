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
        public PartyViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IPartyService partyService,IVendorService vendorService,IDropDownService dropDownService) : base(commonServices)
        {

            PartyService = partyService;
            PartyList = new PartyListViewModel(partyService, commonServices);
            PartyDetails = new PartyDetailsViewModel(partyService, filePickerService, commonServices, PartyList, dropDownService, vendorService);
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

        private async Task PopulateDetails(PartyModel selected)
        {
            try
            {
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
        }

    }
}
