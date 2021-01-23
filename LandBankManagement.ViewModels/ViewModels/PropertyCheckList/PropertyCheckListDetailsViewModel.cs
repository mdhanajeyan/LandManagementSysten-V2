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
        private PropertyCheckListViewModel PropertyCheckListViewModel { get; set; }
        public PropertyCheckListDetailsViewModel(IDropDownService dropDownService, IPropertyCheckListService propertyCheckListService, IPropertyService propertyService, IFilePickerService filePickerService, ICommonServices commonServices, PropertyCheckListListViewModel propertyCheckListListViewModel, PropertyCheckListViewModel propertyCheckListViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            PropertyService = propertyService;
            PropertyCheckListService = propertyCheckListService;
            PropertyCheckListListViewModel = propertyCheckListListViewModel;
            PropertyCheckListViewModel = propertyCheckListViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Property" : TitleEdit;
        public string TitleEdit => Item == null ? "Property" : $"{Item.PropertyName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        public async Task LoadAsync()
        {
            Item = new PropertyCheckListModel() { PropertyCheckListId = -1, PropertyTypeId = 0, CompanyID = 0, TalukId = 0, HobliId = 0, VillageId = 0, DocumentTypeId = 0 };
            IsEditMode = true;
            await GetDropdowns();
            PrepareCheckList();
        }

        public async Task LoadPropertyCheckList(int id) {
            StartStatusMessage("Loading Property Checklist...");
            PropertyCheckListViewModel.ShowProgressRing();
            var model = await PropertyCheckListService.GetPropertyCheckListAsync(id);
           // Item = null;
            Item = model;           
            VendorList = model.PropertyCheckListVendors;
            if (model.PropertyCheckListDocuments != null)
            {
                for (int i = 0; i < model.PropertyCheckListDocuments.Count; i++)
                {
                    model.PropertyCheckListDocuments[i].Identity = i + 1;
                }
            }
            DocList = model.PropertyCheckListDocuments;
            PrepareCheckList();
            RestartItem();
            PropertyCheckListViewModel.HideProgressRing();
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
        private async Task GetDropdowns()
        {
            PropertyCheckListViewModel.ShowProgressRing();
            CompanyOptions = await DropDownService.GetCompanyOptions();
            HobliOptions = await DropDownService.GetHobliOptions();
            TalukOptions = await DropDownService.GetTalukOptions();
            VillageOptions = await DropDownService.GetVillageOptions();
            DocumentTypeOptions = await DropDownService.GetDocumentTypeOptions();
            PropertyTypeOptions = await DropDownService.GetPropertyTypeOptions();
            CheckListOptions = await DropDownService.GetCheckListOptions();
            PropertyCheckListViewModel.HideProgressRing();
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
                            obj.Mandatory = item.Mandatory;
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
            if (CheckListOptions == null)
                CheckListOptions = new ObservableCollection<ComboBoxOptions>();
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

        public async void GetVendors() {
            PropertyCheckListViewModel.ShowProgressRing();
            VendorOptions = await DropDownService.GetVendorOptions(VendorSearchQuery);
            PropertyCheckListViewModel.HideProgressRing();
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
                PropertyCheckListViewModel.ShowProgressRing();
               await PropertyCheckListService.DeletePropertyVendorAsync(model.CheckListVendorId);
                GetPropertyVendors(Item.PropertyCheckListId);
                PropertyCheckListViewModel.HideProgressRing();
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

        public ICommand SavePictureCommand => new RelayCommand(OnSaveFile);
        private async void OnSaveFile()
        {
            if (Item.PropertyCheckListId > 0)
            {

                List<PropertyCheckListDocumentsModel> docs = new List<PropertyCheckListDocumentsModel>();
                foreach (var doc in DocList)
                {
                    if (doc.PropertyCheckListBlobId == 0)
                    {
                        docs.Add(doc);
                    }
                }

                if (docs.Count > 0)
                {
                    StartStatusMessage("Saving Property Documents...");
                    PropertyCheckListViewModel.ShowProgressRing();
                    await PropertyCheckListService.SaveDocuments(docs, Item.PropertyGuid);
                    DocList = await PropertyCheckListService.GetDocuments(Item.PropertyGuid);
                    for (int i = 0; i < DocList.Count; i++)
                    {
                        DocList[i].Identity = i + 1;
                    }
                    PropertyCheckListViewModel.HideProgressRing();
                    EndStatusMessage("Property Documents saved");
                }
            }

        }

        public async Task DeleteDocument(int id)
        {
            if (id > 0)
            {
                StartStatusMessage("Deleting Property Documents...");
                if (DocList[id - 1].PropertyCheckListBlobId > 0)
                {
                    PropertyCheckListViewModel.ShowProgressRing();
                      await PropertyCheckListService.DeletePropertyDocumentAsync(DocList[id - 1]);
                    PropertyCheckListViewModel.HideProgressRing();
                }
                DocList.RemoveAt(id - 1);
                var newlist = DocList;
                for (int i = 0; i < newlist.Count; i++)
                {
                    newlist[i].Identity = i + 1;
                }
                DocList = null;
                DocList = newlist;
                EndStatusMessage("Property Documents deleted");
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
                PropertyCheckListViewModel.ShowProgressRing();
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
            finally {
                PropertyCheckListViewModel.HideProgressRing();
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
                    else if (obj.CheckListPropertyId > 0 && obj.IsSelected)
                        checklist.Add(obj);
                    else if (obj.CheckListPropertyId > 0 && !obj.IsSelected)
                    {
                        obj.Delete = true;
                        checklist.Add(obj);
                    }
                }
                model.CheckListOfProperties = checklist;
            }
        }


        public void GetPropertyVendors(int id) {
            PropertyCheckListViewModel.ShowProgressRing();
            var items= PropertyCheckListService.GetPropertyCheckListVendors(id);
            PropertyCheckListViewModel.HideProgressRing();
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
                PropertyCheckListViewModel.ShowProgressRing();
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
            finally {
                PropertyCheckListViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.PropertyCheckListId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Property?", "Ok", "Cancel");
        }
        public async Task ValidationMeassge(string message)
        {
            await DialogService.ShowAsync("Validation Error", message, "Ok");
        }
        override protected IEnumerable<IValidationConstraint<PropertyCheckListModel>> GetValidationConstraints(PropertyCheckListModel model)
        {
            yield return new ValidationConstraint<PropertyCheckListModel>("Company must be selected", m => m.CompanyID > 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Taluk must be selected", m => m.TalukId > 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Hobli must be selected", m => m.HobliId > 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Village must be selected", m => m.VillageId > 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Document Type must be selected", m => m.DocumentTypeId > 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Property Type must be selected", m => m.PropertyTypeId > 0);
            yield return new RequiredConstraint<PropertyCheckListModel>("Survey No", m => m.SurveyNo);           
            yield return new RequiredConstraint<PropertyCheckListModel>("Proeprty Name", m => m.PropertyName);
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
