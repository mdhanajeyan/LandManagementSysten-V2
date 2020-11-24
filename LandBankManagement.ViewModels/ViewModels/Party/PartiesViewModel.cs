using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class PartiesViewModel : ViewModelBase
    {
        IPartyService PartyService { get; }
        public PartyDetailsViewModel PartyDetails{get;set;}
        public PartyListViewModel PartyList { get; set; }

        public PartiesViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IPartyService partyService) : base(commonServices)
        {
            PartyService = partyService;
            PartyDetails = new PartyDetailsViewModel(partyService, filePickerService, commonServices);
            PartyList = new PartyListViewModel(partyService, commonServices);
        }

        public async Task LoadAsync(PartyListArgs args)
        {
            await PartyList.LoadAsync(args);
        }
        public void Unload()
        {
            PartyDetails.CancelEdit();
            PartyList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PartyListViewModel>(this, OnMessage);
            PartyList.Subscribe();
            PartyDetails.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            PartyList.Unsubscribe();
            PartyDetails.Unsubscribe();

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
            if (PartyDetails.IsEditMode)
            {
                StatusReady();
                PartyDetails.CancelEdit();
            }

            var selected = PartyList.SelectedItem;
            if (!PartyList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);

                }
            }
            PartyDetails.Item = selected;
        }

        private async Task PopulateDetails(PartyModel selected)
        {
            try
            {
                var model = await PartyService.GetPartyAsync(selected.PartyId);
                selected.Merge(model);
            }
            catch (Exception ex)
            {
                LogException("Parties", "Load Details", ex);
            }
        }
    }
}
