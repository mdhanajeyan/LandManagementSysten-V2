using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
   public class PaymentsViewModel : ViewModelBase
    {
        IPaymentService PaymentsService { get; }
        public PaymentsListViewModel PaymentsList { get; set; }

        public PaymentsDetailsViewModel PaymentsDetails { get; set; }

        public PaymentsViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IPaymentService paymentsService) : base(commonServices)
        {
            PaymentsService = paymentsService;
            PaymentsList = new PaymentsListViewModel(PaymentsService, commonServices);
            PaymentsDetails = new PaymentsDetailsViewModel(dropDownService, PaymentsService, filePickerService, commonServices, PaymentsList);
        }

        public async Task LoadAsync(PaymentsListArgs args)
        {
            //await PaymentsDetails.LoadAsync();
            await PaymentsList.LoadAsync(args);
        }
        public void Unload()
        {
            PaymentsList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PaymentsListViewModel>(this, OnMessage);
            PaymentsList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            PaymentsList.Unsubscribe();

        }

        private async void OnMessage(PaymentsListViewModel viewModel, string message, object args)
        {
            if (viewModel == PaymentsList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = PaymentsList.SelectedItem;
            if (!PaymentsList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(PaymentModel selected)
        {
            try
            {
                var model = await PaymentsService.GetPaymentAsync(selected.PaymentId);
                selected.Merge(model);
                PaymentsDetails.Item = model;
                PaymentsDetails.defaultSettings();
                SelectedPivotIndex = 0;
            }
            catch (Exception ex)
            {
                LogException("Payments", "Load Details", ex);
            }
        }
    }
}
