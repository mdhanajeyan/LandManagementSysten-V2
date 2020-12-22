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
    public class PropertyCheckListDetailsViewModel : GenericDetailsViewModel<PropertyCheckListModel>
    {
         public IDropDownService DropDownService { get; }
        public IPropertyService PropertyService { get; }
        public IPropertyCheckListService PropertyCheckListService { get; }
        public IFilePickerService FilePickerService { get; }
        public PropertyCheckListListViewModel PropertyCheckListListViewModel { get; }
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
        private ObservableCollection<ComboBoxOptions> _hobliOptions = null;
        public ObservableCollection<ComboBoxOptions> HobliOptions
        {
            get => _hobliOptions;
            set => Set(ref _hobliOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _villageOptions = null;
        public ObservableCollection<ComboBoxOptions> VillageOptions
        {
            get => _villageOptions;
            set => Set(ref _villageOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _propertyTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> PropertyTypeOptions
        {
            get => _propertyTypeOptions;
            set => Set(ref _propertyTypeOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _checklistOptions = null;
        public ObservableCollection<ComboBoxOptions> CheckListOptions
        {
            get => _checklistOptions;
            set => Set(ref _checklistOptions, value);
        }

        private ObservableCollection<CheckListOfPropertyModel> _checkList = null;
        public ObservableCollection<CheckListOfPropertyModel> CheckList
        {
            get => _checkList;
            set => Set(ref _checkList, value);
        }
        
        private ObservableCollection<ComboBoxOptions> _vendorOptions = null;
        public ObservableCollection<ComboBoxOptions> VendorOptions
        {
            get => _vendorOptions;
            set => Set(ref _vendorOptions, value);
        }

        private ObservableCollection<PropertyCheckListVendorModel> _vendor = null;
        public ObservableCollection<PropertyCheckListVendorModel> VendorList
        {
            get => _vendor;
            set => Set(ref _vendor, value);
        }


        public string _vendorSearchQuery = null;
        public string VendorSearchQuery {
            get => _vendorSearchQuery;
            set => Set(ref _vendorSearchQuery, value);
        }

        private ObservableCollection<PropertyCheckListDocumentsModel> _docList = null;
        public ObservableCollection<PropertyCheckListDocumentsModel> DocList
        {
            get => _docList;
            set => Set(ref _docList, value);
        }

        public PropertyCheckListDetailsViewModel(IDropDownService dropDownService, IPropertyCheckListService propertyCheckListService, IPropertyService propertyService, IFilePickerService filePickerService, ICommonServices commonServices, PropertyCheckListListViewModel propertyCheckListListViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            PropertyService = propertyService;
            PropertyCheckListService = propertyCheckListService;
            PropertyCheckListListViewModel = propertyCheckListListViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Property" : TitleEdit;
        public string TitleEdit => Item == null ? "Property" : $"{Item.PropertyName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        public async Task LoadAsync()
        {
            Item = new PropertyCheckListModel();
            IsEditMode = true;
            GetDropdowns();
            PrepareCheckList();
        }

        public async Task LoadPropertyCheckList(int id) {
            StartStatusMessage("Loading Property Checklist...");
            ShowProgressRing();
            var model = await PropertyCheckListService.GetPropertyCheckListAsync(id);
            HideProgressRing();
            Item = model;           
            VendorList = model.PropertyCheckListVendors;
            DocList = model.PropertyCheckListDocuments;
            PrepareCheckList();
            RestartItem();
            StartStatusMessage("Property Checklist is loaded");
        }
       public void loadAcres(Area area,string type) {

            if (type == "Area")
            {             
                   Item.LandAreaInAcres = Math.Round(area.Acres,2).ToString();
                   Item.LandAreaInGuntas =Math.Round( area.Guntas,2).ToString();
                Item.LandAreaInputAanas = Math.Round(area.Anas, 2).ToString();
                   Item.LandAreaInSqft = Math.Round(area.SqFt,2).ToString();
                   Item.LandAreaInSqMts = Math.Round(area.SqMeters,2).ToString();
            }
            if (type == "AKarab")
            {
                    Item.AKarabAreaInAcres = Math.Round(area.Acres,2).ToString();
                    Item.AKarabAreaInGuntas = Math.Round(area.Guntas,2).ToString();
                Item.AKarabAreaInputAanas = Math.Round(area.Anas, 2).ToString();
                    Item.AKarabAreaInSqft = Math.Round(area.SqFt,2).ToString();
                    Item.AKarabAreaInSqMts = Math.Round(area.SqMeters,2).ToString();
              
            }
            if (type == "BKarab")
            {
                    Item.BKarabAreaInAcres = Math.Round(area.Acres,2).ToString();
                    Item.BKarabAreaInGuntas = Math.Round(area.Guntas,2).ToString();
                Item.BKarabAreaInputAanas = Math.Round(area.Anas, 2).ToString();
                    Item.BKarabAreaInSqft = Math.Round(area.SqFt,2).ToString();
                    Item.BKarabAreaInSqMts = Math.Round(area.SqMeters,2).ToString();
            }
            RestartItem();
        }
        private void RestartItem() {
            var old = Item;
            Item = null;
            Item = old;
        }
        private void GetDropdowns()
        {
            CompanyOptions = DropDownService.GetCompanyOptions();
            HobliOptions = DropDownService.GetHobliOptions();
            TalukOptions = DropDownService.GetTalukOptions();
            VillageOptions = DropDownService.GetVillageOptions();
            DocumentTypeOptions = DropDownService.GetDocumentTypeOptions();
            PropertyTypeOptions = DropDownService.GetPropertyTypeOptions();
            CheckListOptions = DropDownService.GetCheckListOptions();
        }
        public void PrepareCheckList() {
            ResetCheckList();
            if (Item.PropertyCheckListId > 0) {        
               
                if (Item.CheckListOfProperties != null) {
                    foreach (var obj in CheckList) {
                        var item = Item.CheckListOfProperties.Where(x => x.CheckListId == obj.CheckListId).FirstOrDefault();
                        if (item != null) { 
                            obj.IsSelected = true;
                            obj.CheckListPropertyId = item.CheckListPropertyId;
                        }
                    }                
                }
            }
        }

        public void SelectAllCheckList(bool check) {
            var models = CheckList;
            foreach (var obj in models) {
                obj.IsSelected = check;
            }
            CheckList = null;
            CheckList = models;
        }

        private void ResetCheckList() {
            CheckList = new ObservableCollection<CheckListOfPropertyModel>();
            foreach (var obj in CheckListOptions)
            {
                CheckList.Add(new CheckListOfPropertyModel
                {
                    PropertyCheckListId = 0,
                    CheckListPropertyId = 0,
                    CheckListId = obj.Id,
                    Mandatory = false,
                    Name = obj.Description
                });
            }
        }

        public void GetVendors() {

            VendorOptions = DropDownService.GetVendorOptions(VendorSearchQuery);
        }

        public void PrepareVendorList() {
            if (VendorOptions == null)
                return;

            foreach (var item in VendorOptions) {
                if (item.IsSelected) {
                    if (VendorList == null)
                        VendorList = new ObservableCollection<PropertyCheckListVendorModel>();
                    VendorList.Add(new PropertyCheckListVendorModel
                    { 
                    VendorId=item.Id,
                    VendorName=item.Description
                    });
                }
            }
        }

        public void PreparePropertyName() {
            if (string.IsNullOrEmpty(Item.PropertyName))
            {
                var village = VillageOptions.Where(x => x.Id == Item.VillageId).FirstOrDefault().Description;
                Item.PropertyName = village + " - " + Item.SurveyNo;
                RestartItem();
            }
        }
       
        public async void RemoveVendor(int id) {
            var model = VendorList.First(x => x.VendorId == id);
            if (model.PropertyCheckListId > 0) {
               await PropertyCheckListService.DeletePropertyVendorAsync(model.CheckListVendorId);
                GetPropertyVendors(Item.PropertyCheckListId);
            }
            else
            VendorList.Remove(model);
        }

        public ICommand EditPictureCommand => new RelayCommand(OnEditFile);
        private async void OnEditFile()
        {
            var result = await FilePickerService.OpenImagePickerAsync();
            if (result != null)
            {
                if (DocList == null)
                    DocList = new ObservableCollection<PropertyCheckListDocumentsModel>();

                foreach (var file in result)
                {
                    DocList.Add(new PropertyCheckListDocumentsModel { 
                    FileCategoryId=file.FileCategoryId,
                    FileName=file.FileName,
                    Size=file.Size,
                    ContentType=file.ContentType,
                    ImageBytes=file.ImageBytes,
                    DueDate=DateTimeOffset.Now,
                    ActualCompletionDate=DateTimeOffset.Now
                    });
                }
                for (int i = 0; i < DocList.Count; i++)
                {
                    DocList[i].Identity = i + 1;
                }
            }

        }

        public void DeleteDocument(int id)
        {
            if (id > 0)
            {
                if (DocList[id - 1].blobId > 0)
                {
                    PropertyCheckListService.DeletePropertyDocumentAsync(DocList[id - 1]);
                }
                DocList.RemoveAt(id - 1);
                var newlist = DocList;
                for (int i = 0; i < newlist.Count; i++)
                {
                    newlist[i].Identity = i + 1;
                }
                DocList = null;
                DocList = newlist;
            }
        }


        public void Subscribe()
        {
            MessageService.Subscribe<PropertyCheckListDetailsViewModel, PropertyCheckListModel>(this, OnDetailsMessage);
            MessageService.Subscribe<PropertyListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }
               
        protected override async Task<bool> SaveItemAsync(PropertyCheckListModel model)
        {
            try
            {
                StartStatusMessage("Saving Property Checklist...");

                PreparePRoeprtyCheckListModel(model);
                int id = 0;
                if (model.PropertyCheckListId <= 0)
                    id = await PropertyCheckListService.AddPropertyCheckListAsync(model);
                else
                    id = await PropertyCheckListService.UpdatePropertyCheckListAsync(model);
                model = new PropertyCheckListModel() { PropertyCheckListId = -1, PropertyTypeId = 0, CompanyID = 0, TalukId = 0, HobliId = 0, VillageId = 0, DocumentTypeId = 0 };
                EndStatusMessage("Property CheckList saved");
                ClearItem();
               await LoadPropertyCheckList(id);
               

                //DocList = model.PropertyCheckListDocuments;
                //VendorList = model.PropertyCheckListVendors;
                //PrepareCheckList();

               
                LogInformation("Property", "Save", "Property CheckList saved successfully", $"Property {model.PropertyCheckListId} '{model.PropertyName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Property CheckList: {ex.Message}");
                LogException("Property CheckList", "Save", ex);
                return false;
            }
        }
        private void PreparePRoeprtyCheckListModel(PropertyCheckListModel model) {
            var docList = new ObservableCollection<PropertyCheckListDocumentsModel>();
            if (DocList != null)
            {
                foreach (var obj in DocList)
                {
                    if (obj.PropertyCheckListBlobId == 0)
                    {
                        docList.Add(obj);
                    }
                }
                model.PropertyCheckListDocuments = docList;
            }
            var vendorList = new ObservableCollection<PropertyCheckListVendorModel>();
            if (VendorList != null)
            {
                foreach (var obj in VendorList)
                {
                    if (obj.CheckListVendorId == 0)
                    {
                        vendorList.Add(obj);
                    }
                }
                model.PropertyCheckListVendors = vendorList;
            }

            var checklist = new ObservableCollection<CheckListOfPropertyModel>();
            if (CheckList != null)
            {
                foreach (var obj in CheckList)
                {
                    if (obj.CheckListPropertyId == 0 && obj.IsSelected)
                        checklist.Add(obj);
                }
                model.CheckListOfProperties = checklist;
            }
        }


        public void GetPropertyVendors(int id) {
            var items= PropertyCheckListService.GetPropertyCheckListVendors(id);
            VendorList = new ObservableCollection<PropertyCheckListVendorModel>(items);            
        }

        protected override void ClearItem()
        {
            Item = new PropertyCheckListModel() { PropertyCheckListId = -1,  PropertyTypeId = 0, CompanyID = 0, TalukId = 0, HobliId = 0, VillageId = 0, DocumentTypeId = 0};
            VendorSearchQuery = "";
            VendorOptions = null;
            VendorList = null;
            if (DocList != null)
                DocList.Clear();
            ResetCheckList();
        }
        protected override async Task<bool> DeleteItemAsync(PropertyCheckListModel model)
        {
            try
            {
                StartStatusMessage("Deleting Property checklist...");

                await PropertyCheckListService.DeletePropertyCheckListAsync(model);
                ClearItem();
                //await PropertyCheckListListViewModel.RefreshAsync();
                EndStatusMessage("Property deleted");
                LogWarning("Property", "Delete", "Property deleted", $"Taluk {model.PropertyCheckListId} '{model.PropertyName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Property checklist: {ex.Message}");
                LogException("Property checklist", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Property?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<PropertyCheckListModel>> GetValidationConstraints(PropertyCheckListModel model)
        {
           yield return new RequiredConstraint<PropertyCheckListModel>("Proeprty Name", m => m.PropertyName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(PropertyCheckListDetailsViewModel sender, string message, PropertyCheckListModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.PropertyCheckListId == current?.PropertyCheckListId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await PropertyCheckListService.GetPropertyCheckListAsync(current.PropertyCheckListId);
                                    item = item ?? new PropertyCheckListModel { PropertyCheckListId = current.PropertyCheckListId, IsEmpty = true };
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
            //var current = Item;
            //if (current != null)
            //{
            //    switch (message)
            //    {
            //        case "ItemsDeleted":
            //            if (args is IList<PropertyCheckListModel> deletedModels)
            //            {
            //                if (deletedModels.Any(r => r.PropertyId == current.PropertyId))
            //                {
            //                    await OnItemDeletedExternally();
            //                }
            //            }
            //            break;
            //        case "ItemRangesDeleted":
            //            try
            //            {
            //                var model = await PropertyService.GetPropertyAsync(current.PropertyId);
            //                if (model == null)
            //                {
            //                    await OnItemDeletedExternally();
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                LogException("Property", "Handle Ranges Deleted", ex);
            //            }
            //            break;
            //    }
            //}
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
