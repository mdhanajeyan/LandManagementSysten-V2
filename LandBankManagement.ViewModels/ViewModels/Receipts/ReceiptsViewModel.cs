using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class ReceiptsViewModel : ViewModelBase
    {


        IReceiptService ReceiptsService { get; }
        public ReceiptsListViewModel ReceiptsList { get; set; }

        public ReceiptsDetailsViewModel ReceiptsDetials { get; set; }

        public ReceiptsViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IReceiptService receiptService) : base(commonServices)
        {
            ReceiptsService = receiptService;
            ReceiptsList = new ReceiptsListViewModel(receiptService, commonServices);
            ReceiptsDetials = new ReceiptsDetailsViewModel(dropDownService, receiptService, filePickerService, commonServices, ReceiptsList);
        }

        public async Task LoadAsync(ReceiptsListArgs args)
        {
            await ReceiptsDetials.LoadAsync();
            await ReceiptsList.LoadAsync(args);
        }
        public void Unload()
        {
            ReceiptsList.Unload();
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
                var model = await ReceiptsService.GetReceiptAsync(selected.ReceiptId);
                selected.Merge(model);
                ReceiptsDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("Receipts", "Load Details", ex);
            }
        }
    }
}
