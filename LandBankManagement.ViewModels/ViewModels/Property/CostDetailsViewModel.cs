using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LandBankManagement.Data.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
namespace LandBankManagement.ViewModels
{
    public class CostDetailsViewModel : GenericDetailsViewModel<PropertyCostDetailsModel>
    {
        public IPropertyService PropertyService { get; }
        public PropertyListViewModel PropertyListViewModel { get; }
        private ObservableCollection<PaymentScheduleModel> _paymentSchedule = null;
        public ObservableCollection<PaymentScheduleModel> PaymentScheduleList
        {
            get => _paymentSchedule;
            set => Set(ref _paymentSchedule, value);
        }

        private List<PropertyPartyModel> _parties = null;
        public List<PropertyPartyModel> Parties
        {
            get => _parties;
            set => Set(ref _parties, value);
        }

        private PaymentScheduleModel _currentPayment = null;
        public PaymentScheduleModel CurrentPayment {
            get => _currentPayment;
            set => Set(ref _currentPayment, value);
        }

        private string _totalAmount1 = null;
        public string TotalAmount1
        {
            get => _totalAmount1;
            set => Set(ref _totalAmount1, value);
        }
        private string _totalAmount2 = null;
        public string TotalAmount2
        {
            get => _totalAmount2;
            set => Set(ref _totalAmount2, value);
        }


        public CostDetailsViewModel( IPropertyService propertyService, ICommonServices commonServices, PropertyListViewModel propertyListViewModel) : base(commonServices)
        {           
            PropertyService = propertyService;
            PropertyListViewModel = propertyListViewModel;
        }

        public async void LoadAsync(int id) {
            CurrentPayment = new PaymentScheduleModel() { ScheduleDate =DateTimeOffset.Now };
            Item =await PropertyService.GetPropertyCostDetails(id);
            Parties = Item.Parties;
            PaymentScheduleList = new ObservableCollection<PaymentScheduleModel>(Item.PropPaySchedules);
            CurrentPayment = new PaymentScheduleModel() { ScheduleDate = DateTimeOffset.Now,PropertyId=Item.PropertyId };
            CalculateTotalAMounts();
        }

        private void CalculateTotalAMounts() {
            decimal totalAmt1 = 0;
            decimal totalAmt2 = 0;
            foreach (var model in PaymentScheduleList)
            {
                totalAmt1 += model.Amount1;
                totalAmt2 += model.Amount2;
            }

            TotalAmount1 = totalAmt1.ToString();
            TotalAmount2 = totalAmt2.ToString();
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Property" : TitleEdit;
        public string TitleEdit => Item == null ? "Property" : $"{Item.PropertyName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new PropertyCostDetailsModel();           
        }

        public void AddPaymentToList() {
            if (PaymentScheduleList == null)
                PaymentScheduleList = new ObservableCollection<PaymentScheduleModel>();

            CurrentPayment.Total = CurrentPayment.Amount1 + CurrentPayment.Amount2;
            PaymentScheduleList.Add(CurrentPayment);
            CurrentPayment = new PaymentScheduleModel() { ScheduleDate = DateTimeOffset.Now, PropertyId = Item.PropertyId };
            CalculateTotalAMounts();
        }
        public void ClearPayment() {
            CurrentPayment = new PaymentScheduleModel() { ScheduleDate = DateTimeOffset.Now , PropertyId = Item.PropertyId };
        }
        protected override void ClearItem()
        {
            throw new NotImplementedException();
        }

        public async void SavePaymentSequence() {
            if (PaymentScheduleList.Count == 0)
                return;

           await PropertyService.AddPropPaySchedule(PaymentScheduleList.ToList(),Convert.ToDecimal( Item.SaleValue1), Convert.ToDecimal(Item.SaleValue2));
        }


        public void Subscribe()
        {
            //MessageService.Subscribe<PropertyDetailsViewModel, PropertyModel>(this, OnDetailsMessage);
            //MessageService.Subscribe<PropertyListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }


        protected override async Task<bool> SaveItemAsync(PropertyCostDetailsModel model)
        {
            try
            {
                //StartStatusMessage("Saving Property...");

                //if (model.PropertyId <= 0)
                //    model = await PropertyService.AddPropertyAsync(model);
                //else
                //    await PropertyService.UpdatePropertyAsync(model);

                //SaveParties(model);
                //GetPropertyParties(model.PropertyId);
                //EndStatusMessage("Property saved");
                //LogInformation("Property", "Save", "Property saved successfully", $"Property {model.PropertyId} '{model.PropertyName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Property: {ex.Message}");
                LogException("Property", "Save", ex);
                return false;
            }
        }
      
        protected override async Task<bool> DeleteItemAsync(PropertyCostDetailsModel model)
        {
            try
            {
                //StartStatusMessage("Deleting Property...");

                //await PropertyService.DeletePropertyAsync(model);
                //ClearItem();
                //await PropertyListViewModel.RefreshAsync();
                //EndStatusMessage("Property deleted");
                //LogWarning("Property", "Delete", "Property deleted", $"Taluk {model.PropertyId} '{model.PropertyName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Property: {ex.Message}");
                LogException("Property", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Property?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<PropertyCostDetailsModel>> GetValidationConstraints(PropertyCostDetailsModel model)
        {
            yield return new RequiredConstraint<PropertyCostDetailsModel>("Name", m => m.PropertyName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        //private async void OnDetailsMessage(PropertyDetailsViewModel sender, string message, PropertyModel changed)
        //{
        //    var current = Item;
        //    if (current != null)
        //    {
        //        if (changed != null && changed.PropertyId == current?.PropertyId)
        //        {
        //            switch (message)
        //            {
        //                case "ItemChanged":
        //                    await ContextService.RunAsync(async () =>
        //                    {
        //                        try
        //                        {
        //                            var item = await PropertyService.GetPropertyAsync(current.PropertyId);
        //                            item = item ?? new PropertyModel { PropertyId = current.PropertyId, IsEmpty = true };
        //                            current.Merge(item);
        //                            current.NotifyChanges();
        //                            NotifyPropertyChanged(nameof(Title));
        //                            if (IsEditMode)
        //                            {
        //                                StatusMessage("WARNING: This Property has been modified externally");
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            LogException("Property", "Handle Changes", ex);
        //                        }
        //                    });
        //                    break;
        //                case "ItemDeleted":
        //                    await OnItemDeletedExternally();
        //                    break;
        //            }
        //        }
        //    }
        //}

        //private async void OnListMessage(PropertyListViewModel sender, string message, object args)
        //{
        //    var current = Item;
        //    if (current != null)
        //    {
        //        switch (message)
        //        {
        //            case "ItemsDeleted":
        //                if (args is IList<PropertyModel> deletedModels)
        //                {
        //                    if (deletedModels.Any(r => r.PropertyId == current.PropertyId))
        //                    {
        //                        await OnItemDeletedExternally();
        //                    }
        //                }
        //                break;
        //            case "ItemRangesDeleted":
        //                try
        //                {
        //                    var model = await PropertyService.GetPropertyAsync(current.PropertyId);
        //                    if (model == null)
        //                    {
        //                        await OnItemDeletedExternally();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogException("Property", "Handle Ranges Deleted", ex);
        //                }
        //                break;
        //        }
        //    }
        //}

        //private async Task OnItemDeletedExternally()
        //{
        //    await ContextService.RunAsync(() =>
        //    {
        //        CancelEdit();
        //        IsEnabled = false;
        //        StatusMessage("WARNING: This Taluk has been deleted externally");
        //    });
        //}
    }

}
