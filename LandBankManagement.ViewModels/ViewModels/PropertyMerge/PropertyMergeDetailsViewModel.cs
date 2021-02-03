using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LandBankManagement.Extensions;
using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class PropertyMergeDetailsViewModel : GenericDetailsViewModel<PropertyMergeModel>
    {
        public IDropDownService DropDownService { get; }
        public IPropertyMergeService PropertyMergeService { get; }
        public IFilePickerService FilePickerService { get; }

        private ObservableCollection<ComboBoxOptions> _companyOptions = null;
        public ObservableCollection<ComboBoxOptions> CompanyOptions
        {
            get => _companyOptions;
            set => Set(ref _companyOptions, value);
        }
       
        private ObservableCollection<ComboBoxOptions> _propertyOptions = null;
        public ObservableCollection<ComboBoxOptions> PropertyOptions
        {
            get => _propertyOptions;
            set => Set(ref _propertyOptions, value);
        } 
        
        private ObservableCollection<ComboBoxOptions> _propertyDocumentTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> PropertyDocumentOptions
        {
            get => _propertyDocumentTypeOptions;
            set => Set(ref _propertyDocumentTypeOptions, value);
        }

        private ObservableCollection<PropertyMergeListModel> _propertyListOptions = null;
        public ObservableCollection<PropertyMergeListModel> PropertyList
        {
            get => _propertyListOptions;
            set => Set(ref _propertyListOptions, value);
        }

        private PropertyMergeListModel _currentPropertyListOptions = null;
        public PropertyMergeListModel CurrentProperty
        {
            get => _currentPropertyListOptions;
            set => Set(ref _currentPropertyListOptions, value);
        }


        private string _companyId="0" ;
        public string selectedCompany
        {
            get => _companyId;
            set => Set(ref _companyId, value);
        }

        private string _propertyId="0";
        public string selectedProperty
        {
            get => _propertyId;
            set => Set(ref _propertyId, value);
        }
        private string _documentTypeId="0";
        public string selectedDocumentType
        {
            get => _documentTypeId;
            set => Set(ref _documentTypeId, value);
        }

        private PropertyMergeViewModel PropertyMergesViewModel { get; set; }
        public PropertyMergeDetailsViewModel(IDropDownService dropDownService, IPropertyMergeService propertMergeService, IFilePickerService filePickerService, ICommonServices commonServices, PropertyMergeViewModel propertyMergeViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            PropertyMergeService = propertMergeService;
            PropertyMergesViewModel = propertyMergeViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New PropertyMerges" : TitleEdit;
        public string TitleEdit = "PropertyMerges";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new PropertyMergeModel() ;
            GetDropdowns();
        }
       
        private async void GetDropdowns()
        {
            PropertyMergesViewModel.ShowProgressRing();
            CompanyOptions = await DropDownService.GetCompanyOptions();
            PropertyOptions = await DropDownService.GetUnSoldPropertyOptions();           
            PropertyMergesViewModel.HideProgressRing();
        }

        public async Task GetDocumentType() {
            if (selectedProperty == "0"|| selectedProperty==null)
                return;
            PropertyDocumentOptions = await DropDownService.GetDocumentTypesByPropertyID(Convert.ToInt32( selectedProperty));
        }

        public async Task LoadedSelectedProperty() {
            if (selectedDocumentType == "0" || selectedDocumentType == null)
                return;
            if (Convert.ToInt32(selectedDocumentType) > 0) {
                PropertyMergesViewModel.ShowProgressRing();
                var model = await PropertyMergeService.GetPropertyListItemForProeprty(Convert.ToInt32(selectedProperty), Convert.ToInt32(selectedDocumentType));
                PropertyMergesViewModel.HideProgressRing();
                if (CurrentProperty == null)
                    CurrentProperty = new PropertyMergeListModel();
                CurrentProperty = model;
            }
        }

        public async void LoadPropertyOptionByCompany() {
            if (selectedCompany == "0" || selectedCompany == null)
                return;
            PropertyMergesViewModel.ShowProgressRing();
            PropertyOptions = await DropDownService.GetPropertyOptionsByCompanyID(Convert.ToInt32(selectedCompany));
            PropertyMergesViewModel.HideProgressRing();
        }

        public void AddPropertyToList() {          

            if (PropertyList == null)
                PropertyList = new ObservableCollection<PropertyMergeListModel>();

            if (CurrentProperty==null || CurrentProperty.PropertyGuid == Guid.Empty)
                return;

            var isExist = PropertyList.Where(x => x.PropertyGuid == CurrentProperty.PropertyGuid).Count();
            if (isExist > 0)
                return;

            PropertyList.Add(CurrentProperty);
            CurrentProperty = new PropertyMergeListModel();
            selectedProperty = "0";
            selectedCompany = "0";
            selectedDocumentType = "0";
            var temp = PropertyDocumentOptions;
            PropertyDocumentOptions = null;
            PropertyDocumentOptions = temp;
            
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PropertyMergeDetailsViewModel, PropertyMergeModel>(this, OnDetailsMessage);
            MessageService.Subscribe<PropertyMergeListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public async void DeletePropertyMergeList(Guid guid)
        {

            var item = PropertyList.Where(x => x.PropertyGuid == guid).FirstOrDefault();
            if (item.PropertyMergeListId>0)
            {
                PropertyMergesViewModel.ShowProgressRing();
                await PropertyMergeService.DeletePropertyMergeItemAsync(item.PropertyMergeListId);
                PropertyMergesViewModel.HideProgressRing();
            }
            PropertyList.Remove(item);
            
            var newList = PropertyList;
            PropertyList = null;
            PropertyList = newList;
        }


        protected override async Task<bool> SaveItemAsync(PropertyMergeModel model)
        {
            try
            {
                if (PropertyList == null || PropertyList.Count==0)
                    return false;

                model.propertyMergeLists = PropertyList;
                decimal totalArea=0 ;
                decimal totalGuntas = 0;
                decimal totalAnas = 0;
                foreach (var obj in model.propertyMergeLists) {
                    var area = obj.TotalArea.Split('-');
                    //totalArea += AreaConvertor.AcreToSqft(Convert.ToDecimal(area[0])) + AreaConvertor.GuntasToSqft(Convert.ToDecimal(area[1])) + AreaConvertor.AanasToSqft(Convert.ToDecimal(area[2]));
                    totalArea += Convert.ToDecimal(area[0]);
                    totalGuntas += Convert.ToDecimal(area[1]);
                    totalAnas += Convert.ToDecimal(area[2]);


                    model.MergedSaleValue1 = model.MergedSaleValue1 + Convert.ToDecimal(string.IsNullOrEmpty( obj.SaleValue1)?"0" : obj.SaleValue1);
                    model.MergedSaleValue2 = model.MergedSaleValue2 + Convert.ToDecimal(string.IsNullOrEmpty( obj.SaleValue2)?"0" : obj.SaleValue2);
                    model.MergedAmountPaid1 = model.MergedAmountPaid1 + Convert.ToDecimal(string.IsNullOrEmpty( obj.Amount1)?"0" : obj.Amount1);
                    model.MergedAmountPaid2 = model.MergedAmountPaid2 + Convert.ToDecimal(string.IsNullOrEmpty( obj.Amount2)?"0" : obj.Amount2);
                }

               var finalArea = AreaConvertor.ConvertArea(totalArea, totalGuntas, totalAnas);
                model.FormattedTotalArea = finalArea.Acres + " - " + finalArea.Guntas + " - " + finalArea.Anas;
                StartStatusMessage("Saving PropertyMerges...");
                PropertyMergesViewModel.ShowProgressRing();
                int mergeId = 0;
                if (model.PropertyMergeId <= 0)
                    mergeId = await PropertyMergeService.AddPropertyMergeAsync(model);
                else
                    await PropertyMergeService.UpdatePropertyMergeAsync(model);

                var item = await PropertyMergeService.GetPropertyMergeAsync(mergeId == 0 ? model.PropertyMergeId : mergeId);
                Item = item;
                PropertyList = item.propertyMergeLists;

                EndStatusMessage("PropertyMerges saved");
                LogInformation("PropertyMerges", "Save", "PropertyMerges saved successfully", $"PropertyMerges {model.PropertyMergeId}  was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving PropertyMerges: {ex.Message}");
                LogException("PropertyMerges", "Save", ex);
                return false;
            }
            finally { PropertyMergesViewModel.HideProgressRing(); }
        }
              

        protected override void ClearItem()
        {
            Item = new PropertyMergeModel() ;
            PropertyList = new ObservableCollection<PropertyMergeListModel>();
        }
        protected override async Task<bool> DeleteItemAsync(PropertyMergeModel model)
        {
            try
            {
                if (model == null || model.PropertyMergeId == 0)
                    return false;

                StartStatusMessage("Deleting PropertyMerges...");
                PropertyMergesViewModel.ShowProgressRing();
               
                var isDeleted= await PropertyMergeService.DeletePropertyMergeAsync(model);
                if (isDeleted == 0) {
                     await DialogService.ShowAsync("", "Deal is exist for this merged Property", "Ok");
                    StatusError($"This property is not deleted ");
                    return false;
                }
                ClearItem();
                EndStatusMessage("PropertyMerges deleted");
                LogWarning("PropertyMerges", "Delete", "PropertyMerges deleted", $"Taluk {model.PropertyMergeId}  was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting PropertyMerges: {ex.Message}");
                LogException("PropertyMerges", "Delete", ex);
                return false;
            }
            finally { PropertyMergesViewModel.HideProgressRing(); }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.PropertyMergeId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current PropertyMerges?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<PropertyMergeModel>> GetValidationConstraints(PropertyMergeModel model)
        {
            yield return new RequiredGreaterThanZeroConstraint<PropertyMergeModel>("Deal Name", m => m.PropertyMergeDealName);
            
        }

       
        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(PropertyMergeDetailsViewModel sender, string message, PropertyMergeModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.PropertyMergeId == current?.PropertyMergeId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await PropertyMergeService.GetPropertyMergeAsync(current.PropertyMergeId);
                                    item = item ?? new PropertyMergeModel { PropertyMergeId = current.PropertyMergeId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This PropertyMerges has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("PropertyMerges", "Handle Changes", ex);
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

        private async void OnListMessage(PropertyMergeListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<PropertyMergeModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.PropertyMergeId == current.PropertyMergeId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await PropertyMergeService.GetPropertyMergeAsync(current.PropertyMergeId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("PropertyMerges", "Handle Ranges Deleted", ex);
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
