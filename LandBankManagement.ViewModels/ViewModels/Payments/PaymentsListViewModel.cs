using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LandBankManagement.ViewModels
{
    public class PaymentsListArgs
    {
        static public PaymentsListArgs CreateEmpty() => new PaymentsListArgs { IsEmpty = true };

        public PaymentsListArgs()
        {
            OrderBy = r => r.DateOfPayment;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.Payment, object>> OrderBy { get; set; }
        public Expression<Func<Data.Payment, object>> OrderByDesc { get; set; }
    }
    public class PaymentsListViewModel : GenericListViewModel<PaymentModel>
    {
        public IPaymentService PaymentsService { get; }
        public PaymentsListArgs ViewModelArgs { get; private set; }

        public PaymentsListViewModel(IPaymentService villageService, ICommonServices commonServices) : base(commonServices)
        {
            PaymentsService = villageService;
        }
        public async Task LoadAsync(PaymentsListArgs args)
        {
            ViewModelArgs = args ?? PaymentsListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            //StartStatusMessage("Loading Payments...");
            //if (await RefreshAsync())
            //{
            //    EndStatusMessage("Payments loaded");
            //}
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PaymentsListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public PaymentsListArgs CreateArgs()
        {
            return new PaymentsListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            Items = null;
            ItemsCount = 0;
            SelectedItem = null;

            try
            {
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<PaymentModel>();
                StatusError($"Error loading Payments: {ex.Message}");
                LogException("Payments", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<PaymentModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.Payment> request = BuildDataRequest();
                return await PaymentsService.GetPaymentsAsync(request);
            }
            return new List<PaymentModel>();
        }


        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<ExpenseHeadViewModel>(new ExpenseHeadArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading Payments...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Payments loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete selected Payments?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} Payments...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} Payments...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} Payments: {ex.Message}");
                    LogException("Paymentss", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Payments deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<PaymentModel> models)
        {
            foreach (var model in models)
            {
                await PaymentsService.DeletePaymentAsync(model);
            }
        }

        private DataRequest<Data.Payment> BuildDataRequest()
        {
            return new DataRequest<Data.Payment>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            switch (message)
            {
                case "NewItemSaved":
                case "ItemDeleted":
                case "ItemsDeleted":
                case "ItemRangesDeleted":
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }
    }
}
