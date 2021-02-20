using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class FundTransferViewModel : ViewModelBase
    {
        IFundTransferService FundTransferService { get; }
        public FundTransferListViewModel FundTransferList { get; set; }

        public FundTransferDetailsViewModel FundTransferDetails { get; set; }
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

        public FundTransferViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IFundTransferService fundTransferService) : base(commonServices)
        {
            FundTransferService = fundTransferService;
            FundTransferList = new FundTransferListViewModel(FundTransferService, commonServices,this);
            FundTransferDetails = new FundTransferDetailsViewModel(dropDownService, FundTransferService, filePickerService, commonServices,this);
        }

        public async Task LoadAsync(FundTransferListArgs args)
        {
            await FundTransferDetails.LoadAsync();
            await FundTransferList.LoadAsync(args);
        }
        public void Unload()
        {
            FundTransferList.Unload();
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
            MessageService.Subscribe<FundTransferListViewModel>(this, OnMessage);
            FundTransferList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            FundTransferList.Unsubscribe();

        }

        private async void OnMessage(FundTransferListViewModel viewModel, string message, object args)
        {
            if (viewModel == FundTransferList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = FundTransferList.SelectedItem;
            if (!FundTransferList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        public async Task PopulateDetails(FundTransferModel selected)
        {
            try
            {
                SelectedPivotIndex = 1;
                ShowProgressRing();
                FundTransferDetails.LoadSelectedFundTransfer(selected.FundTransferId);
             
                
            }
            catch (Exception ex)
            {
                LogException("FundTransfer", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }
    }

}
