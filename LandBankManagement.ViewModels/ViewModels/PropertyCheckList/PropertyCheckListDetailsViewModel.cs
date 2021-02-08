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
        private ObservableCollection<ComboBoxOptions> _activeCompanyOptions = null;
        public ObservableCollection<ComboBoxOptions> ActiveCompanyOptions

        {
            get => _activeCompanyOptions;
            set => Set(ref _activeCompanyOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _companyOptions = null;
        public ObservableCollection<ComboBoxOptions> CompanyOptions

        {
            get => _companyOptions;
            set => Set(ref _companyOptions, value);
        }

        private ObservableCollection<ComboBoxOptions> _allCompanyOptions = null;
        public ObservableCollection<ComboBoxOptions> AllCompanyOptions

        {
            get => _allCompanyOptions;
            set => Set(ref _allCompanyOptions, value);
        }
        private bool _showComp = true;
        public bool ShowActiveCompany
        {
            get => _showComp;
            set => Set(ref _showComp, value);
        }

        private bool _hideComp = false;
        public bool ChangeCompany
        {
            get => _hideComp;
            set => Set(ref _hideComp, value);
        }


        private ObservableCollection<ComboBoxOptions> _documentTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> DocumentTypeOptions
        {
            get => _documentTypeOptions;
            set => Set(ref _documentTypeOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _activeDocumentTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> ActiveDocumentTypeOptions
        {
            get => _activeDocumentTypeOptions;
            set => Set(ref _activeDocumentTypeOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _allDocumentTypeOptions = null;
        public ObservableCollection<ComboBoxOptions> AllDocumentTypeOptions
        {
            get => _allDocumentTypeOptions;
            set => Set(ref _allDocumentTypeOptions, value);
        }
        private bool _showDocType = true;
        public bool ShowActiveDocType
        {
            get => _showDocType;
            set => Set(ref _showDocType, value);
        }

        private bool _hideDocType = false;
        public bool ChangeDocType
        {
            get => _hideDocType;
            set => Set(ref _hideDocType, value);
        }


        private ObservableCollection<ComboBoxOptions> _talukOptions = null;
        public ObservableCollection<ComboBoxOptions> TalukOptions
        {
            get => _talukOptions;
            set => Set(ref _talukOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _activeTalukOptions = null;
        public ObservableCollection<ComboBoxOptions> ActiveTalukOptions
        {
            get => _activeTalukOptions;
            set => Set(ref _activeTalukOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _allTalukOptions = null;
        public ObservableCollection<ComboBoxOptions> AllTalukOptions
        {
            get => _allTalukOptions;
            set => Set(ref _allTalukOptions, value);
        }
        private bool _showTaluk = true;
        public bool ShowActiveTaluk
        {
            get => _showTaluk;
            set => Set(ref _showTaluk, value);
        }

        private bool _hideTaluk = false;
        public bool ChangeTaluk
        {
            get => _hideTaluk;
            set => Set(ref _hideTaluk, value);
        }


        private ObservableCollection<ComboBoxOptions> _hobliOptions = null;
        public ObservableCollection<ComboBoxOptions> HobliOptions
        {
            get => _hobliOptions;
            set => Set(ref _hobliOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _activeHobliOptions = null;
        public ObservableCollection<ComboBoxOptions> ActiveHobliOptions
        {
            get => _activeHobliOptions;
            set => Set(ref _activeHobliOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _AllHobliOptions = null;
        public ObservableCollection<ComboBoxOptions> AllHobliOptions
        {
            get => _AllHobliOptions;
            set => Set(ref _AllHobliOptions, value);
        }
        private bool _showHobli = true;
        public bool ShowActiveHobli
        {
            get => _showHobli;
            set => Set(ref _showHobli, value);
        }

        private bool _hideHobli = false;
        public bool ChangeHobli
        {
            get => _hideHobli;
            set => Set(ref _hideHobli, value);
        }


        private ObservableCollection<ComboBoxOptions> _villageOptions = null;
        public ObservableCollection<ComboBoxOptions> VillageOptions
        {
            get => _villageOptions;
            set => Set(ref _villageOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _activeVillageOptions = null;
        public ObservableCollection<ComboBoxOptions> ActiveVillageOptions
        {
            get => _activeVillageOptions;
            set => Set(ref _activeVillageOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _allVillageOptions = null;
        public ObservableCollection<ComboBoxOptions> AllVillageOptions
        {
            get => _allVillageOptions;
            set => Set(ref _allVillageOptions, value);
        }
        private bool _showVillage = true;
        public bool ShowActiveVillage
        {
            get => _showVillage;
            set => Set(ref _showVillage, value);
        }

        private bool _hideVillage = false;
        public bool ChangeVillage
        {
            get => _hideVillage;
            set => Set(ref _hideVillage, value);
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

        private ObservableCollection<ComboBoxOptions> _propertyCheckList = null;
        public ObservableCollection<ComboBoxOptions> PropertyCheckListOptions
        {
            get => _propertyCheckList;
            set => Set(ref _propertyCheckList, value);
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

        private bool _isFromMasterChecked;
        public bool IsFromMasterChecked
        {
            get => _isFromMasterChecked;
            set => Set(ref _isFromMasterChecked, value);
        }

        private bool _isFromPropertyChecked ;
        public bool IsFromPropertyChecked
        {
            get => _isFromMasterChecked;
            set => Set(ref _isFromPropertyChecked, value);
        }

        private bool _showNewCheckListCombo ;
        public bool ShowNewCheckListCombo
        {
            get => _showNewCheckListCombo;
            set => Set(ref _showNewCheckListCombo, value);
        } 
        private bool _showExistCheckListCombo ;
        public bool ShowExistCheckListCombo
        {
            get => _showExistCheckListCombo;
            set => Set(ref _showExistCheckListCombo, value);
        }

        private ObservableCollection<PropertyCheckListDocumentsModel> _docList = null;
        public ObservableCollection<PropertyCheckListDocumentsModel> DocList
        {
            get => _docList;
            set => Set(ref _docList, value);
        }

        private string _selectedPropertyCheckList;
        public string SelectedPropertyCheckList
        {
            get => _selectedPropertyCheckList;
            set => Set(ref _selectedPropertyCheckList, value);
        }

        private string _selectedNewCheckListItem;
        public string SelectedNewCheckListItem
        {
            get => _selectedNewCheckListItem;
            set => Set(ref _selectedNewCheckListItem, value);
        }

        private string _selectedHobli = "0";
        public string SelectedHobli
        {
            get => _selectedHobli;
            set => Set(ref _selectedHobli, value);
        }

        private string _selectedVillage = "0";
        public string SelectedVillage
        {
            get => _selectedVillage;
            set => Set(ref _selectedVillage, value);
        }

        private bool _popupOpened = false;
        public bool PopupOpened
        {
            get => _popupOpened;
            set => Set(ref _popupOpened, value);
        }

        private bool _noRecords = true;
        public bool NoRecords
        {
            get => _noRecords;
            set => Set(ref _noRecords, value);
        }

        private bool _showVendors = false;
        public bool ShowVendors
        {
            get => _showVendors;
            set => Set(ref _showVendors, value);
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
            Item = new PropertyCheckListModel() { PropertyCheckListId = -1, PropertyTypeId = "0", CompanyID = "0", TalukId = "0", HobliId = "0", VillageId = "0", DocumentTypeId = "0" };
            IsEditMode = true;
            await GetDropdowns();

            ResetCompanyOption();
            ResetTalukOption();
            ResetHobliOption(null);
            ResetVillageOption(null);
            ResetDocumentTypeOption();
            //  PrepareCheckList();
        }

        public async Task LoadPropertyCheckList(int id) {
            ResetCompanyOption();
            ResetTalukOption();
            ResetHobliOption(null);
            ResetVillageOption(null);
            ResetDocumentTypeOption();
            StartStatusMessage("Loading Property Checklist...");
            PropertyCheckListViewModel.ShowProgressRing();
            var model = await PropertyCheckListService.GetPropertyCheckListAsync(id);
            var hobliId = model.HobliId;
            var villageId = model.VillageId;
           // Item = null;
            Item = model;           
            VendorList = model.PropertyCheckListVendors;
            if (model.CheckListOfProperties != null) {
                CheckList = new ObservableCollection<CheckListOfPropertyModel>();

                foreach (var item in model.CheckListOfProperties) {
                    for (int i = 0; i < item.Documents.Count; i++)
                    {
                        item.Documents[i].DeleteAt = item.CheckListId + "_" + i + 1;

                    }
                }
                CheckList = model.CheckListOfProperties;
            }
            //if (model.PropertyCheckListDocuments != null)
            //{
            //    for (int i = 0; i < model.PropertyCheckListDocuments.Count; i++)
            //    {
            //        model.PropertyCheckListDocuments[i].Identity = i + 1;
            //    }
            //}
            //DocList = model.PropertyCheckListDocuments;
           // PrepareCheckList();
           
            RestartItem();
            SelectedHobli = "0";
            SelectedVillage = "0";
           SelectedHobli = hobliId;
            SelectedVillage = villageId;
            Item.HobliId = hobliId;
            Item.VillageId = villageId;
            PropertyCheckListViewModel.HideProgressRing();
            StartStatusMessage("Property Checklist is loaded");
        }
       public void loadAcres(Area area,string type) {

            if (type == "Area")
            {
                Item.LandAreaInAcres = Math.Round(area.Acres, 2).ToString();
                Item.LandAreaInGuntas = Math.Round(area.Guntas, 2).ToString();
                var areaAnas = Math.Round(area.Anas, 2).ToString();
                Item.LandAreaInputAanas = (areaAnas == "0" )? "" : areaAnas;
                Item.LandAreaInSqft = Math.Round(area.SqFt, 2).ToString();
                Item.LandAreaInSqMts = Math.Round(area.SqMeters, 2).ToString();
            }
            if (type == "AKarab")
            {
                Item.AKarabAreaInAcres = Math.Round(area.Acres, 2).ToString();
                Item.AKarabAreaInGuntas = Math.Round(area.Guntas, 2).ToString();
                var akrabAnas = Math.Round(area.Anas, 2).ToString();
                Item.AKarabAreaInputAanas = (akrabAnas == "0") ? "" : akrabAnas;
                Item.AKarabAreaInSqft = Math.Round(area.SqFt, 2).ToString();
                Item.AKarabAreaInSqMts = Math.Round(area.SqMeters, 2).ToString();

            }
            if (type == "BKarab")
            {
                Item.BKarabAreaInAcres = Math.Round(area.Acres, 2).ToString();
                Item.BKarabAreaInGuntas = Math.Round(area.Guntas, 2).ToString();
                var bkrabAnas = Math.Round(area.Anas, 2).ToString();
                Item.BKarabAreaInputAanas = (bkrabAnas == "0") ? "" : bkrabAnas;
                Item.BKarabAreaInSqft = Math.Round(area.SqFt, 2).ToString();
                Item.BKarabAreaInSqMts = Math.Round(area.SqMeters, 2).ToString();
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
            ActiveCompanyOptions = await DropDownService.GetCompanyOptions();
            AllCompanyOptions = await DropDownService.GetAllCompanyOptions();
            ActiveTalukOptions = await DropDownService.GetTalukOptions();
            AllTalukOptions = await DropDownService.GetAllTalukOptions();
            ActiveDocumentTypeOptions = await DropDownService.GetDocumentTypeOptions();
            AllDocumentTypeOptions = await DropDownService.GetAllDocumentTypeOptions();
            AllHobliOptions = await DropDownService.GetAllHobliOptions();
            ActiveHobliOptions = await DropDownService.GetHobliOptions();
            AllVillageOptions = await DropDownService.GetAllVillageOptions();
            ActiveVillageOptions = await DropDownService.GetVillageOptions();
            PropertyTypeOptions = await DropDownService.GetPropertyTypeOptions();
            CheckListOptions = await DropDownService.GetCheckListOptions();
            PropertyCheckListOptions = await DropDownService.GetPropertyCheckListOptions();
            PropertyCheckListViewModel.HideProgressRing();
        }

        public void ChangeCompanyOptions(string companyId)
        {
            var comp = ActiveCompanyOptions.Where(x => x.Id == companyId).FirstOrDefault();
            if (comp != null)
            {
                ResetCompanyOption();
                return;
            }
            CompanyOptions = AllCompanyOptions;
            ShowActiveCompany = false;
            ChangeCompany = true;
        }

        public void ResetCompanyOption()
        {
            CompanyOptions = ActiveCompanyOptions;
            ShowActiveCompany = true;
            ChangeCompany = false;
        }

        public void ChangeTalukOptions(string talukId)
        {
            var comp = ActiveTalukOptions.Where(x => x.Id == talukId).FirstOrDefault();
            if (comp != null)
            {
                ResetTalukOption();
                return;
            }
            TalukOptions = AllTalukOptions;
            ShowActiveTaluk = false;
            ChangeTaluk = true;
        }

        public void ResetTalukOption()
        {
            TalukOptions = ActiveTalukOptions;
            ShowActiveTaluk = true;
            ChangeTaluk = false;
        }

        public void ChangeHobliOptions(string hobliId)
        {

            var comp = ActiveHobliOptions.Where(x => x.Id == hobliId).FirstOrDefault();
            if (comp != null || hobliId == null)
            {
                ResetHobliOption(hobliId);
                return;
            }
            HobliOptions = AllHobliOptions;
            SelectedHobli = "0";
            SelectedHobli = hobliId;
            ShowActiveHobli = false;
            ChangeHobli = true;
          
        }

        public void ResetHobliOption(string hobliId)
        {
            HobliOptions = ActiveHobliOptions;
            ShowActiveHobli = true;
            ChangeHobli = false;
            if (hobliId != null)
                SelectedHobli = hobliId;
        }

        public void ChangeVillageOptions(string villageId)
        {
            var comp = ActiveVillageOptions.Where(x => x.Id == villageId).FirstOrDefault();
            if (comp != null||  villageId==null)
            {
                ResetVillageOption(villageId);
                return;
            }
            VillageOptions = AllVillageOptions;
            SelectedVillage = "0";
            SelectedVillage = villageId;
            ShowActiveVillage = false;
            ChangeVillage = true;
           
        }

        public void ResetVillageOption(string villageId)
        {
            VillageOptions = ActiveVillageOptions;
            ShowActiveVillage = true;
            ChangeVillage = false;
            if (villageId != null)
                SelectedVillage = villageId;
        }

        public void ChangeDocumentTypeOptions(int docTypeId)
        {
            var comp = ActiveDocumentTypeOptions.Where(x => Convert.ToInt32(x.Id) == docTypeId).FirstOrDefault();
            if (comp != null)
            {
                ResetDocumentTypeOption();
                return;
            }
            DocumentTypeOptions = AllDocumentTypeOptions;
            ShowActiveDocType = false;
            ChangeDocType = true;
        }

        public void ResetDocumentTypeOption()
        {
            DocumentTypeOptions = ActiveDocumentTypeOptions;
            ShowActiveDocType = true;
            ChangeDocType = false;
        }

        public async Task LoadHobli()
        {
            var hobliId = Item.HobliId;
            int id = Convert.ToInt32(Item.TalukId);
            HobliOptions = await DropDownService.GetHobliOptionsByTaluk(id);
            if (HobliOptions.Count == 1 &&( hobliId == "0" || hobliId == null))
                return;
            var isExist = HobliOptions.Where(x => x.Id == hobliId).FirstOrDefault();
            var isValid = hobliId == null ? null : isExist;
            if (HobliOptions.Count <= 1 || isValid==null )
            {
                ChangeHobliOptions(hobliId);
            }
            else
            {
                SelectedHobli = "0";
                SelectedHobli = hobliId;
            }
           
        }
        public async Task LoadVillage()
        {
            var villageId = Item.VillageId;
            VillageOptions = await DropDownService.GetVillageOptionsByHobli(Convert.ToInt32( SelectedHobli));
            if (VillageOptions.Count == 1 && (villageId == "0" || villageId == null))
                return;
            var isExist = VillageOptions.Where(x => x.Id == villageId).FirstOrDefault();
            var isValid = villageId == null ? null : isExist;
            if (VillageOptions.Count <= 1 || isValid == null)
            {
                ChangeVillageOptions(villageId);
            }
            else
            {
                SelectedVillage = "0";
                SelectedVillage = villageId;
            }
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
                    CheckListId =Convert.ToInt32( obj.Id),
                    Mandatory = false,
                    Name = obj.Description
                });
            }
        }

        public void AddNewCheckListItem()
        {
            if (CheckListOptions == null)
                return;
            var item = CheckListOptions.Where(x =>x.Id == SelectedNewCheckListItem).First();
            if (CheckList != null)
            {
                var isExist = CheckList.Where(x => x.CheckListId ==Convert.ToInt32( SelectedNewCheckListItem)).FirstOrDefault();
                if (isExist != null)
                    return;
            }
            else
                CheckList = new ObservableCollection<CheckListOfPropertyModel>();

            CheckList.Add(new CheckListOfPropertyModel
            {
                PropertyCheckListId = 0,
                CheckListPropertyId = 0,
                CheckListId =Convert.ToInt32( item.Id),
                Mandatory = false,
                Name = item.Description,
                IsSelected = true
            });           
        }

        public void RemoveCheckListItem(int checkListId)
        {
            var item = CheckList.Where(x => x.CheckListId == checkListId).FirstOrDefault();
            if (item == null)
                return;
            if (item.CheckListPropertyId > 0)
            {
                PropertyCheckListService.DeleteCheckListOfPropertyAsync(item.CheckListPropertyId);
                CheckList.Remove(item);
            }
            else
                CheckList.Remove(item);
        }

        public async Task LoadCheckList(string type)
        {
            if (type == "master")
            {
                ShowExistCheckListCombo = false;
                ShowNewCheckListCombo = true;
            }
            else
            {
                ShowExistCheckListCombo = true;
                ShowNewCheckListCombo = false;
                await LoadCheckListByProeprty();
            }
        }

        public async Task LoadCheckListByProeprty() {
            var items = await PropertyCheckListService.GetCheckListOfProperty(Convert.ToInt32( SelectedPropertyCheckList));
            foreach (var obj in items) {
                obj.CheckListPropertyId = 0;
                obj.PropertyCheckListId = 0;
            }
            CheckList = items;
        }
        public async void GetVendors() {
            PropertyCheckListViewModel.ShowProgressRing();
            VendorOptions = await DropDownService.GetVendorOptions(VendorSearchQuery);
            if (VendorOptions == null|| VendorOptions.Count==0)
            {
                ShowVendors = false;
                NoRecords = true;
            }
            else
            {
                ShowVendors = true;
                NoRecords = false;
            }
            PopupOpened = true;
            PropertyCheckListViewModel.HideProgressRing();
        }

        public void PrepareVendorList() {
            PopupOpened = false;
            if (VendorOptions == null)
                return;

            foreach (var item in VendorOptions) {
                if (item.IsSelected) {
                    if (VendorList == null)
                        VendorList = new ObservableCollection<PropertyCheckListVendorModel>();
                    VendorList.Add(new PropertyCheckListVendorModel
                    { 
                    VendorId=Convert.ToInt32( item.Id),
                    VendorName=item.Description
                    });
                }
            }
            if (VendorList.Count == 1) {
                VendorList[0].IsPrimaryVendor = true;
            }
        }

        public void PreparePropertyName() {
            if (string.IsNullOrEmpty(Item.PropertyName))
            {
                var village = VillageOptions.Where(x => x.Id == SelectedVillage).FirstOrDefault().Description;
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

       // public ICommand EditPictureCommand => new RelayCommand(OnEditFile);
        public async void OnEditFile(int checkListId)
        {
            var result = await FilePickerService.OpenImagePickerAsync();
            if (result != null)
            {
               
                var item = CheckList.Where(x => x.CheckListId == checkListId).First();
                if (item.Documents == null)
                    item.Documents = new ObservableCollection<PropertyCheckListDocumentsModel>();
                foreach (var file in result)
                {
                    item.Documents.Add(new PropertyCheckListDocumentsModel { 
                    FileCategoryId=file.FileCategoryId,
                    FileName=file.FileName,
                    Size=file.Size,
                    ContentType=file.ContentType,
                    ImageBytes=file.ImageBytes,
                    DueDate=DateTimeOffset.Now,
                    ActualCompletionDate=DateTimeOffset.Now
                    });
                }
                for (int i = 0; i < item.Documents.Count; i++)
                {
                    //item.Documents[i].Identity = i + 1;
                    item.Documents[i].DeleteAt =checkListId+"_"+ i + 1;

                }
                var temp = CheckList;
                CheckList = null;
                CheckList = temp;
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

        public async Task DeleteDocument(int checkListId,int docInx)
        {
           
                StartStatusMessage("Deleting Property Documents...");
                var item = CheckList.Where(x => x.CheckListId == checkListId).First();
            var doc = item.Documents[docInx - 1];
                if (doc.PropertyCheckListBlobId > 0)
                {
                    PropertyCheckListViewModel.ShowProgressRing();
                      await PropertyCheckListService.DeletePropertyDocumentAsync(doc);
                    PropertyCheckListViewModel.HideProgressRing();
                }
            item.Documents.Remove(doc);
            for (int i = 0; i < item.Documents.Count; i++)
            {
                item.Documents[i].DeleteAt = checkListId + "_" + i + 1;

            }
            EndStatusMessage("Property Documents deleted");
           
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
                model.HobliId = SelectedHobli;
                model.VillageId = SelectedVillage;
                if (model.PropertyCheckListId <= 0)
                    id = await PropertyCheckListService.AddPropertyCheckListAsync(model);
                else
                    id = await PropertyCheckListService.UpdatePropertyCheckListAsync(model);
                model = new PropertyCheckListModel() { PropertyCheckListId = -1, PropertyTypeId = "0", CompanyID = "0", TalukId = "0", HobliId = "0", VillageId = "0", DocumentTypeId = "0" };
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
            //var docList = new ObservableCollection<PropertyCheckListDocumentsModel>();
            //if (DocList != null)
            //{
            //    foreach (var obj in DocList)
            //    {
            //        if (obj.PropertyCheckListBlobId == 0)
            //        {
            //            docList.Add(obj);
            //        }
            //    }
            //    model.PropertyCheckListDocuments = docList;
            //}
            model.PropertyCheckListVendors = VendorList;
            //var vendorList = new ObservableCollection<PropertyCheckListVendorModel>();
            //if (VendorList != null)
            //{
            //    foreach (var obj in VendorList)
            //    {
            //        if (obj.CheckListVendorId == 0)
            //        {
            //            vendorList.Add(obj);
            //        }
            //    }
            //    model.PropertyCheckListVendors = vendorList;
            //}

            //var checklist = new ObservableCollection<CheckListOfPropertyModel>();
            //if (CheckList != null)
            //{
            //    foreach (var obj in CheckList)
            //    {
            //        if (obj.CheckListPropertyId == 0 && obj.IsSelected)
            //            checklist.Add(obj);
            //        else if (obj.CheckListPropertyId > 0 && obj.IsSelected)
            //            checklist.Add(obj);
            //        else if (obj.CheckListPropertyId > 0 && !obj.IsSelected)
            //        {
            //            obj.Delete = true;
            //            checklist.Add(obj);
            //        }
            //    }
            //    model.CheckListOfProperties = checklist;
            //}
            model.CheckListOfProperties = CheckList;
        }


        public void GetPropertyVendors(int id) {
            PropertyCheckListViewModel.ShowProgressRing();
            var items= PropertyCheckListService.GetPropertyCheckListVendors(id);
            PropertyCheckListViewModel.HideProgressRing();
            if(items==null)
            VendorList = new ObservableCollection<PropertyCheckListVendorModel>(items);            
        }

        protected override void ClearItem()
        {
            Item = new PropertyCheckListModel() { PropertyCheckListId = -1, PropertyTypeId = "0", CompanyID = "0", TalukId = "0", HobliId = "0", VillageId = "0", DocumentTypeId = "0" };
            VendorSearchQuery = "";
            VendorOptions = null;
            VendorList = null;
            if (DocList != null)
                DocList.Clear();
            CheckList = new ObservableCollection<CheckListOfPropertyModel>();

            ResetCompanyOption();
            ResetTalukOption();
            ResetHobliOption(null);
            ResetVillageOption(null);
            ResetDocumentTypeOption();
            SelectedHobli = "0";
            // ResetCheckList();
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
            yield return new ValidationConstraint<PropertyCheckListModel>("Company must be selected", m =>Convert.ToInt32( m.CompanyID??"0") > 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Taluk must be selected", m => Convert.ToInt32(m.TalukId??"0") > 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Hobli must be selected", m => Convert.ToInt32(_selectedHobli??"0" )> 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Village must be selected", m => Convert.ToInt32(SelectedVillage ?? "0" )> 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Document Type must be selected", m => Convert.ToInt32(m.DocumentTypeId??"0") > 0);
            yield return new ValidationConstraint<PropertyCheckListModel>("Property Type must be selected", m => Convert.ToInt32(m.PropertyTypeId??"0") > 0);
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
