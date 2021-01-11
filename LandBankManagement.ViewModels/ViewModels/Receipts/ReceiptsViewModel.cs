using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class ReceiptsViewModel : ViewModelBase
    {


        IReceiptService ReceiptsService { get; }
        IDealService DealService { get; }
        public ReceiptsListViewModel ReceiptsList { get; set; }

        public ReceiptsDetailsViewModel ReceiptsDetials { get; set; }
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
        public ReceiptsViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IReceiptService receiptService, IDealService dealService) : base(commonServices)
        {
            ReceiptsService = receiptService;
            ReceiptsList = new ReceiptsListViewModel(receiptService, commonServices,this);
            ReceiptsDetials = new ReceiptsDetailsViewModel(dropDownService, receiptService, filePickerService, commonServices, ReceiptsList, this,dealService);
        }

        public async Task LoadAsync(ReceiptsListArgs args)
        {
            await ReceiptsDetials.LoadAsync();            
        }
        public void Unload()
        {
            ReceiptsList.Unload();
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
            MessageService.Subscribe<ReceiptsListViewModel>(this, OnMessage);
            ReceiptsList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            ReceiptsList.Unsubscribe();
        }

        private async void OnMessage(ReceiptsListViewModel viewModel, string message, object args)
        {
            if (viewModel == ReceiptsList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = ReceiptsList.SelectedItem;
            if (!ReceiptsList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(ReceiptModel selected)
        {
            try
            {
                //ShowProgressRing();
                //var model = await ReceiptsService.GetReceiptAsync(selected.ReceiptId);
                //selected.Merge(model);
                //ReceiptsDetials.Item = model;
                //if (model.PaymentTypeId == 1)
                //    ReceiptsDetials.IsCashChecked = true;
                //else
                //    ReceiptsDetials.IsBankChecked = true;
                //ReceiptsDetials.LoadDealParties();
                //ReceiptsDetials.Item = ReceiptsDetials.Item;
                SelectedPivotIndex = 1;
                ReceiptsDetials.LoadSelectedReceipt(selected.ReceiptId);
               
            }
            catch (Exception ex)
            {
                LogException("Receipts", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }
    }
}
