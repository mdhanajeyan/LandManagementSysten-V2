using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class PropertyDetailsViewModel : GenericDetailsViewModel<PropertyModel>
    {
        public IDropDownService DropDownService { get; }
        public IPropertyService PropertyService { get; }
        public IFilePickerService FilePickerService { get; }
        public PropertyListViewModel PropertyListViewModel { get; }
        private ObservableCollection<ComboBoxOptions> _companyOptions = null;
        public ObservableCollection<ComboBoxOptions> CompanyOptions
        {
            get => _companyOptions;
            set => Set(ref _companyOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _documentTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> DocumentTypeOptions
        {
            get => _documentTypeOptions;
            set => Set(ref _documentTypeOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _talukOptions = null;
        public ObservableCollection<ComboBoxOptions> TalukOptions
        {
            get => _talukOptions;
            set => Set(ref _talukOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _partyOptions = null;
        public ObservableCollection<ComboBoxOptions> PartyOptions
        {
            get => _partyOptions;
            set => Set(ref _partyOptions, value);
        }
        public PropertyDetailsViewModel(IDropDownService dropDownService, IPropertyService propertyService, IFilePickerService filePickerService, ICommonServices commonServices, PropertyListViewModel villageListViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            PropertyService = propertyService;
            PropertyListViewModel = villageListViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Property" : TitleEdit;
        public string TitleEdit => Item == null ? "Property" : $"{Item.PropertyName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new PropertyModel();
            IsEditMode = true;
            GetDropdowns();
        }
        private void GetDropdowns()
        {
            PartyOptions = DropDownService.GetPartyOptions();
            CompanyOptions = DropDownService.GetCompanyOptions();
            // DealOptions = DropDownService.GetDealOptions();
        }


        public void Subscribe()
        {
            MessageService.Subscribe<PropertyDetailsViewModel, PropertyModel>(this, OnDetailsMessage);
            MessageService.Subscribe<PropertyListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        //public ExpenseHeadDetailsArgs CreateArgs()
        //{
        //    return new ExpenseHeadDetailsArgs
        //    {
        //        ExpenseHeadId = Item?.ExpenseHeadId ?? 0
        //    };
        //}


        protected override async Task<bool> SaveItemAsync(PropertyModel model)
        {
            try
            {
                StartStatusMessage("Saving Property...");

                if (model.PropertyId <= 0)
                    await PropertyService.AddPropertyAsync(model);
                else
                    await PropertyService.UpdatePropertyAsync(model);
                await PropertyListViewModel.RefreshAsync();
                EndStatusMessage("Property saved");
                LogInformation("Property", "Save", "Property saved successfully", $"Property {model.PropertyId} '{model.PropertyName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Property: {ex.Message}");
                LogException("Property", "Save", ex);
                return false;
            }
        }
        protected override void ClearItem()
        {
            Item = new PropertyModel();
        }
        protected override async Task<bool> DeleteItemAsync(PropertyModel model)
        {
            try
            {
                StartStatusMessage("Deleting Property...");

                await PropertyService.DeletePropertyAsync(model);
                ClearItem();
                await PropertyListViewModel.RefreshAsync();
                EndStatusMessage("Property deleted");
                LogWarning("Property", "Delete", "Property deleted", $"Taluk {model.PropertyId} '{model.PropertyName}' was deleted.");
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

        override protected IEnumerable<IValidationConstraint<PropertyModel>> GetValidationConstraints(PropertyModel model)
        {
            yield return new RequiredConstraint<PropertyModel>("Name", m => m.PropertyName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(PropertyDetailsViewModel sender, string message, PropertyModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.PropertyId == current?.PropertyId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await PropertyService.GetPropertyAsync(current.PropertyId);
                                    item = item ?? new PropertyModel { PropertyId = current.PropertyId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Property has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Property", "Handle Changes", ex);
                                }
                            });
                            break;
                        case "ItemDeleted":
                            await OnItemDeletedExternally();
                            break;
                    }
                }
            }
        }

        private async void OnListMessage(PropertyListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<PropertyModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.PropertyId == current.PropertyId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await PropertyService.GetPropertyAsync(current.PropertyId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Property", "Handle Ranges Deleted", ex);
                        }
                        break;
                }
            }
        }

        private async Task OnItemDeletedExternally()
        {
            await ContextService.RunAsync(() =>
            {
                CancelEdit();
                IsEnabled = false;
                StatusMessage("WARNING: This Taluk has been deleted externally");
            });
        }
    }
}
