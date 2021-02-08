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
    public class PropertyDetailsViewModel : GenericDetailsViewModel<PropertyModel>
    {
        public IDropDownService DropDownService { get; }
        public IPropertyService PropertyService { get; }
        public IFilePickerService FilePickerService { get; }
        public PropertyListViewModel PropertyListViewModel { get; }

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

        private ObservableCollection<ComboBoxOptions> _partyOptions = null;
        public ObservableCollection<ComboBoxOptions> PartyOptions
        {
            get => _partyOptions;
            set => Set(ref _partyOptions, value);
        }

        private ObservableCollection<PropertyPartyModel> _addedParty = null;
        public ObservableCollection<PropertyPartyModel> PartyList
        {
            get => _addedParty;
            set => Set(ref _addedParty, value);
        }


        public string _partySearchQuery = null;
        public string PartySearchQuery {
            get => _partySearchQuery;
            set => Set(ref _partySearchQuery, value);
        }

        private ObservableCollection<ImagePickerResult> _docList = null;
        public ObservableCollection<ImagePickerResult> DocList
        {
            get => _docList;
            set => Set(ref _docList, value);
        }

        private ObservableCollection<PropertyModel> _propertyList = null;
        public ObservableCollection<PropertyModel> PropertyList
        {
            get => _propertyList;
            set => Set(ref _propertyList, value);
        }

        private ObservableCollection<PropertyDocumentTypeModel> _propertyDocumentTypeList = null;
        public ObservableCollection<PropertyDocumentTypeModel> PropertyDocumentTypeList
        {
            get => _propertyDocumentTypeList;
            set => Set(ref _propertyDocumentTypeList, value);
        }

        private PropertyDocumentTypeModel _currentDocumentType = null;
        public PropertyDocumentTypeModel CurrentDocumentType
        {
            get => _currentDocumentType;
            set => Set(ref _currentDocumentType, value);
        }
        public PropertyViewModel PropertyView { get; set; }

        private bool _enableDocType = false;
        public bool EnableDocType
        {
            get => _enableDocType;
            set => Set(ref _enableDocType, value);
        }

        private bool _enablePropertyName = false;
        public bool EnablePropertyName
        {
            get => _enablePropertyName;
            set => Set(ref _enablePropertyName, value);
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

        private bool _showParties = false;
        public bool ShowParties
        {
            get => _showParties;
            set => Set(ref _showParties, value);
        }

        public PropertyDetailsViewModel(IDropDownService dropDownService, IPropertyService propertyService, IFilePickerService filePickerService, ICommonServices commonServices, PropertyListViewModel propertyListViewModel, PropertyViewModel propertyView) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            PropertyService = propertyService;
            PropertyListViewModel = propertyListViewModel;
            PropertyView = propertyView;   
            
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Property" : TitleEdit;
        public string TitleEdit => Item == null ? "Property" : $"{Item.PropertyName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;
              
        public async Task LoadAsync(bool fromParty)
        {
            EnableDocType = true;
            EnablePropertyName = true;
               Item = new PropertyModel();
            Item.DateOfExecution = DateTimeOffset.Now;
            IsEditMode = true;
            await GetDropdowns();
            if(fromParty)
            GetStoredItem();

            ResetCompanyOption();
            ResetTalukOption();
            ResetHobliOption(null);
            ResetVillageOption(null);
            ResetDocumentTypeOption();
        }
        public void GetStoredItem() {
            var prop = PropertyService.GetStoredItems();
            if (prop != null) {
                Item = prop.Item;
                DocList = prop.DocList;
                PartyList = prop.PartyList;
                PropertyList = prop.PropertyList;
            }
        }
        public void storeItems() {
            var property = new PropertyContainer
            {
                Item = Item,
                DocList=DocList,
                PartyList=PartyList,
                PropertyList=PropertyList
            };
            PropertyService.StoreItems(property);
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
            PropertyView.ShowProgressRing();
            ActiveCompanyOptions = await DropDownService.GetCompanyOptions();
            AllCompanyOptions = await DropDownService.GetAllCompanyOptions();
            ActiveTalukOptions = await DropDownService.GetTalukOptions();
            AllTalukOptions = await DropDownService.GetAllTalukOptions();
            ActiveDocumentTypeOptions = await DropDownService.GetDocumentTypeOptions();
            AllDocumentTypeOptions = await DropDownService.GetAllDocumentTypeOptions();
            AllHobliOptions= await DropDownService.GetAllHobliOptions();
            ActiveHobliOptions= await DropDownService.GetHobliOptions();
            AllVillageOptions= await DropDownService.GetAllVillageOptions();
            ActiveVillageOptions= await DropDownService.GetVillageOptions();
            PropertyTypeOptions = await DropDownService.GetPropertyTypeOptions();
            PropertyView.HideProgressRing();
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
            if (comp != null || villageId == null)
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

        public async Task LoadHobli() {
            var hobliId = Item.HobliId;
            int id = Convert.ToInt32(Item.TalukId);
            HobliOptions = await DropDownService.GetHobliOptionsByTaluk(id);
            if (HobliOptions.Count == 1 &&( hobliId == "0"|| hobliId==null))
                return;
            var isExist = HobliOptions.Where(x => x.Id == hobliId).FirstOrDefault();
            var isValid = hobliId == null ? null : isExist;
            if (HobliOptions.Count <= 1 || isValid == null)
            {
                ChangeHobliOptions(hobliId);
            }
            else
            {
                SelectedHobli = "0";
                SelectedHobli = hobliId;
            }
        }
        public async Task LoadVillage() {
            var villageId = Item.VillageId;
            VillageOptions = await DropDownService.GetVillageOptionsByHobli(Convert.ToInt32(SelectedHobli));
            if (VillageOptions.Count == 1 && (villageId == "0" || villageId == null))
                return;
            var isExist = VillageOptions.Where(x => x.Id == villageId).FirstOrDefault();
            var isValid = villageId == null ? null : isExist;
            if (VillageOptions.Count <= 1 && isValid == null)
            {
                ChangeVillageOptions(villageId);
            }
            else
            {
                SelectedVillage = "0";
                SelectedVillage = villageId;
            }
        }

        public async void GetParties() {
            PropertyView.ShowProgressRing();
            PartyOptions =await DropDownService.GetPartyOptions(PartySearchQuery);
            if (PartyOptions == null|| PartyOptions.Count==0)
            {
                ShowParties = false;
                NoRecords = true;
            }
            else
            {
                ShowParties = true;
                NoRecords = false;
            }
            PopupOpened = true;
            PropertyView.HideProgressRing();
        }

        public void PreparePartyList() {
            PopupOpened = false;
            if (PartyOptions == null)
                return;

            foreach (var item in PartyOptions) {

                if (item.IsSelected) {
                    if (PartyList != null)
                    {
                        var existParty = PartyList.Where(x => x.PartyId.ToString() == item.Id).FirstOrDefault();
                        if (existParty != null)
                            continue;
                    }
                    if (PartyList == null)
                        PartyList = new ObservableCollection<PropertyPartyModel>();
                    PartyList.Add(new PropertyPartyModel { 
                    PartyId=Convert.ToInt32( item.Id),
                    PartyName=item.Description
                    });
                }
            }
            if (PartyList.Count == 1)
                PartyList[0].IsPrimaryParty = true;
            PartyOptions = null;
            PartySearchQuery = "";
        }

        public void PreparePropertyName() {
            if (string.IsNullOrEmpty(Item.PropertyName))
            {
                var partyName = "";
                if (PartyList!=null) {
                    if (PartyList.Count > 1)
                        partyName = PartyList.Where(x => x.IsPrimaryParty == true).FirstOrDefault().PartyName;
                    else
                        partyName = PartyList[0].PartyName;
                }

                var village = VillageOptions.Where(x => x.Id == SelectedVillage).FirstOrDefault().Description;
                Item.PropertyName = village + " - " + Item.SurveyNo;

                if (partyName!="" && !Item.PropertyName.Contains(partyName))
                    Item.PropertyName =partyName +" - "+ village + " - " + Item.SurveyNo;

                RestartItem();
            }
        }

        public void UpdatePropertyName(string party) {
            if (Item.PropertyName == null || Item.PropertyName == "")
                return;
            var array = Item.PropertyName.Split('-');
            var propertyName = "";
            if (array.Length == 3)
                 propertyName = party + " - " + array[1] + " - " + array[2];
            else
                propertyName = party + " - " + array[0] + " - " + array[1];
            Item.PropertyName = propertyName;
            RestartItem();
        }

        public async void LoadPropertyById(int id) {

            var model = PropertyList.First(x => x.PropertyId == id);
            foreach (var propDocument in model.PropertyDocumentType)
            {
                propDocument.DocumentType = DocumentTypeOptions.Where(x => x.Id == propDocument.DocumentTypeId.ToString()).First().Description;
                if (propDocument.PropertyDocuments != null)
                {
                    for (int i = 0; i < propDocument.PropertyDocuments.Count; i++)
                    {
                        propDocument.PropertyDocuments[i].Identity = i + 1;
                    }
                }
            }

            PropertyDocumentTypeList = model.PropertyDocumentType;
            CurrentDocumentType = model.PropertyDocumentType[0];
            Item = model;
            UpdateItemValues();
            PropertyView.ShowProgressRing();
            await GetPropertyParties(id);
            PropertyView.HideProgressRing();
            //DocList = model.PropertyDocuments;
            //if (model.PropertyDocuments != null)
            //{
            //    for (int i = 0; i < DocList.Count; i++)
            //    {
            //        DocList[i].Identity = i + 1;
            //    }
            //}
        }

        public async void RemoveParty(int id) {
            var model = PartyList.First(x => x.PartyId == id);
            if (model.PropertyPartyId > 0) {
                PropertyView.ShowProgressRing();
               await PropertyService.DeletePropertyPartyAsync(model);
               await  GetPropertyParties(Item.PropertyId);
                PropertyView.HideProgressRing();
            }
            else
            PartyList.Remove(model);
        }

        public void SetCurrentDocumentType(int documentTypeId) {
            if (documentTypeId == 0)
                return;
            if (PropertyDocumentTypeList != null)
            {
                var isExist = PropertyDocumentTypeList.Where(x => x.DocumentTypeId == documentTypeId).FirstOrDefault();
                if (isExist != null)
                {
                    EditableItem.DocumentTypeId = CurrentDocumentType.DocumentTypeId.ToString();                    
                    return;
                }
            }

            CurrentDocumentType = new PropertyDocumentTypeModel();

            CurrentDocumentType.DocumentTypeId = documentTypeId;
            CurrentDocumentType.DocumentType = DocumentTypeOptions.Where(x => x.Id == documentTypeId.ToString()).First().Description;
            if (PropertyDocumentTypeList == null)
                PropertyDocumentTypeList = new ObservableCollection<PropertyDocumentTypeModel>();
            PropertyDocumentTypeList.Add(CurrentDocumentType);
        }

        public void ShiftDocumentType(int documentTypeId) {
            CurrentDocumentType = PropertyDocumentTypeList.Where(x => x.DocumentTypeId == documentTypeId).First();
            UpdateItemValues();
        }

        public ICommand EditPictureCommand => new RelayCommand(OnEditFile);
        public async void OnEditFile()
        {
            var result = await FilePickerService.OpenImagePickerAsync();
            if (result != null)
            {
                //if (DocList == null)
                //    DocList = new ObservableCollection<ImagePickerResult>();

                if (CurrentDocumentType.PropertyDocuments == null)
                    CurrentDocumentType.PropertyDocuments = new ObservableCollection<ImagePickerResult>();
                DocList = CurrentDocumentType.PropertyDocuments;

                foreach (var file in result)
                {
                    DocList.Add(file);
                }
                for (int i = 0; i < DocList.Count; i++)
                {
                    DocList[i].Identity = i + 1;
                }

                CurrentDocumentType.PropertyDocuments = DocList;
                var item = PropertyDocumentTypeList;
                PropertyDocumentTypeList = null;
                PropertyDocumentTypeList = item;


            }

        }

        public ICommand SavePictureCommand => new RelayCommand(OnSaveFile);
        private async void OnSaveFile()
        {
            if (Item.PropertyId > 0) {

                List<ImagePickerResult> docs = new List<ImagePickerResult>();
                foreach (var doc in DocList) {
                    if (doc.blobId == 0) {
                        docs.Add(doc);
                    }
                }

                if (docs.Count > 0)
                {
                    StartStatusMessage("Saving Property Documents...");
                    PropertyView.ShowProgressRing();
                    await PropertyService.SaveDocuments(docs, Item.PropertyGuid,CurrentDocumentType.PropertyDocumentTypeId);
                    DocList = await PropertyService.GetProeprtyDocuments(CurrentDocumentType.PropertyDocumentTypeId);
                    for (int i = 0; i < DocList.Count; i++)
                    {
                        DocList[i].Identity = i + 1;
                    }
                    PropertyView.HideProgressRing();
                    EndStatusMessage("Property Documents saved");
                }
            }

        }

        public void DeleteDocument(int id)
        {
            if (id > 0)
            {
                StartStatusMessage("Deleteing Property Documents...");
                if (DocList[id - 1].blobId > 0)
                {
                    PropertyView.ShowProgressRing();
                    PropertyService.DeletePropertyDocumentAsync(DocList[id - 1]);
                }
                DocList.RemoveAt(id - 1);
                var newlist = DocList;
                for (int i = 0; i < newlist.Count; i++)
                {
                    newlist[i].Identity = i + 1;
                }
                DocList = null;
                DocList = newlist;
                PropertyView.HideProgressRing();
                EndStatusMessage("Property Documents deleted");
            }
        }


        public async void DownloadDocument(int id)
        {
            if (id > 0)
            {
                StartStatusMessage("Start downloading...");
                var result = await FilePickerService.DownloadFile(DocList[id - 1].FileName, DocList[id - 1].ImageBytes, DocList[id - 1].ContentType);
                if (result)
                    StartStatusMessage("File downloaded...");
                else
                    EndStatusMessage("Download failed");
            }
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

        public void CloneProperty() {
           
            var newItem = new PropertyModel();
            newItem.CompanyID = Item.CompanyID;
            newItem.TalukId = Item.TalukId;
            newItem.HobliId = Item.HobliId;
            newItem.VillageId = Item.VillageId;
            newItem.GroupGuid = Item.GroupGuid;
            newItem.DateOfExecution = DateTimeOffset.Now;
            EnableDocType = true;
            EnablePropertyName = true;
            PropertyDocumentTypeList = null;
            Item = null;
            Item = newItem;
            DocList = null;

            foreach (var partyItem in PartyList) {
                partyItem.PropertyPartyId = 0;
            }
        }
       
        protected override async Task<bool> SaveItemAsync(PropertyModel model)
        {
            try
            {
                StartStatusMessage("Saving Property...");
                PropertyView.ShowProgressRing();
                UpdateCurrentDocumentType();
                if (PropertyDocumentTypeList == null) {
                    PropertyDocumentTypeList = new ObservableCollection<PropertyDocumentTypeModel>();
                }

                //if (PropertyDocumentTypeList.Count == 0)
                //{
                //    PropertyDocumentTypeList.Add(CurrentDocumentType);
                //}
                //else {
                //    var inx = PropertyDocumentTypeList.ToList().FindIndex(x=>x.PropertyDocumentTypeId==CurrentDocumentType.PropertyDocumentTypeId);
                //    PropertyDocumentTypeList[inx] = CurrentDocumentType;
                //}

                model.PropertyDocumentType = PropertyDocumentTypeList;
                model.HobliId = SelectedHobli;
                model.VillageId = SelectedVillage;
                if (model.PropertyId <= 0)
                    model = await PropertyService.AddPropertyAsync(model,PropertyDocumentTypeList, DocList);
                else
                    await PropertyService.UpdatePropertyAsync(model, PropertyDocumentTypeList, DocList);

                SaveParties(model);
                ReloadProperty(model.GroupGuid.Value, model.PropertyId);
                // await GetPropertyParties(model.PropertyId);
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
            finally {
                PropertyView.HideProgressRing();
            }
        }

        private void UpdateCurrentDocumentType() {
            CurrentDocumentType.LandAreaInputAcres = Item.LandAreaInputAcres;
            CurrentDocumentType.LandAreaInputGuntas = Item.LandAreaInputGuntas;
            CurrentDocumentType.LandAreaInAcres = Item.LandAreaInAcres;
            CurrentDocumentType.LandAreaInGuntas = Item.LandAreaInGuntas;
            CurrentDocumentType.LandAreaInputAanas = Item.LandAreaInputAanas;
            CurrentDocumentType.LandAreaInSqft = Item.LandAreaInSqft;
            CurrentDocumentType.LandAreaInSqMts = Item.LandAreaInSqMts;
            CurrentDocumentType.AKarabAreaInputAcres = Item.AKarabAreaInputAcres;
            CurrentDocumentType.AKarabAreaInputGuntas = Item.AKarabAreaInputGuntas;
            CurrentDocumentType.AKarabAreaInAcres = Item.AKarabAreaInAcres;
            CurrentDocumentType.AKarabAreaInGuntas = Item.AKarabAreaInGuntas;
            CurrentDocumentType.AKarabAreaInputAanas = Item.AKarabAreaInputAanas;
            CurrentDocumentType.AKarabAreaInSqft = Item.AKarabAreaInSqft;
            CurrentDocumentType.AKarabAreaInSqMts = Item.AKarabAreaInSqMts;
            CurrentDocumentType.BKarabAreaInputAcres = Item.BKarabAreaInputAcres;
            CurrentDocumentType.BKarabAreaInputGuntas = Item.BKarabAreaInputGuntas;
            CurrentDocumentType.BKarabAreaInAcres = Item.BKarabAreaInAcres;
            CurrentDocumentType.BKarabAreaInGuntas = Item.BKarabAreaInGuntas;
            CurrentDocumentType.BKarabAreaInputAanas = Item.BKarabAreaInputAanas;
            CurrentDocumentType.BKarabAreaInSqft = Item.BKarabAreaInSqft;
            CurrentDocumentType.BKarabAreaInSqMts = Item.BKarabAreaInSqMts;
        }

        private void UpdateItemValues()
        {
            EditableItem.DocumentTypeId = CurrentDocumentType.DocumentTypeId.ToString();
            EditableItem.LandAreaInputAcres = CurrentDocumentType.LandAreaInputAcres;
            EditableItem.LandAreaInputGuntas = CurrentDocumentType.LandAreaInputGuntas;
            EditableItem.LandAreaInAcres = CurrentDocumentType.LandAreaInAcres;
            EditableItem.LandAreaInGuntas = CurrentDocumentType.LandAreaInGuntas;
            EditableItem.LandAreaInputAanas = CurrentDocumentType.LandAreaInputAanas;
            EditableItem.LandAreaInSqft = CurrentDocumentType.LandAreaInSqft;
            EditableItem.LandAreaInSqMts = CurrentDocumentType.LandAreaInSqMts;
            EditableItem.AKarabAreaInputAcres = CurrentDocumentType.AKarabAreaInputAcres;
            EditableItem.AKarabAreaInputGuntas = CurrentDocumentType.AKarabAreaInputGuntas;
            EditableItem.AKarabAreaInAcres = CurrentDocumentType.AKarabAreaInAcres;
            EditableItem.AKarabAreaInGuntas = CurrentDocumentType.AKarabAreaInGuntas;
            EditableItem.AKarabAreaInputAanas = CurrentDocumentType.AKarabAreaInputAanas;
            EditableItem.AKarabAreaInSqft = CurrentDocumentType.AKarabAreaInSqft;
            EditableItem.AKarabAreaInSqMts = CurrentDocumentType.AKarabAreaInSqMts;
            EditableItem.BKarabAreaInputAcres = CurrentDocumentType.BKarabAreaInputAcres;
            EditableItem.BKarabAreaInputGuntas = CurrentDocumentType.BKarabAreaInputGuntas;
            EditableItem.BKarabAreaInAcres = CurrentDocumentType.BKarabAreaInAcres;
            EditableItem.BKarabAreaInGuntas = CurrentDocumentType.BKarabAreaInGuntas;
            EditableItem.BKarabAreaInputAanas = CurrentDocumentType.BKarabAreaInputAanas;
            EditableItem.BKarabAreaInSqft = CurrentDocumentType.BKarabAreaInSqft;
            EditableItem.BKarabAreaInSqMts = CurrentDocumentType.BKarabAreaInSqMts;

            var temp = EditableItem;
            EditableItem = null;
            EditableItem = temp;
        }

        private async Task ReloadProperty(Guid guid,int propertId) {
            PropertyView.ShowProgressRing();
             PropertyList = await PropertyService.GetPropertyByGroupGuidAsync(guid);
            
            var model = PropertyList.Where(x=>x.PropertyId==propertId).FirstOrDefault();          
          

            foreach (var propDocument in model.PropertyDocumentType)
            {
                propDocument.DocumentType = DocumentTypeOptions.Where(x => x.Id == propDocument.DocumentTypeId.ToString()).First().Description;
                if (propDocument.PropertyDocuments != null)
                {
                    for (int i = 0; i < propDocument.PropertyDocuments.Count; i++)
                    {
                        propDocument.PropertyDocuments[i].Identity = i + 1;
                    }
                }
            }

            PropertyDocumentTypeList = model.PropertyDocumentType;
            CurrentDocumentType = model.PropertyDocumentType[0];
            Item = model;
            UpdateItemValues();
            await GetPropertyParties(model.PropertyId);
            EnableDocType = false;
            //DocList = model.PropertyDocuments;
            //if (model.PropertyDocuments != null)
            //{
            //    for (int i = 0; i < DocList.Count; i++)
            //    {
            //        DocList[i].Identity = i + 1;
            //    }
            //}
            PropertyView.HideProgressRing();
        }

        private async void SaveParties(PropertyModel property) {

            if (PartyList == null)
                return;
            var parties = new List<PropertyPartyModel>();
            foreach (var model in PartyList) {                
                    parties.Add(new PropertyPartyModel
                    {
                        PartyId=model.PartyId,
                        PropertyId= property.PropertyId,
                        PropertyGuid=property.PropertyGuid,
                        PropertyPartyId=model.PropertyPartyId,
                        IsPrimaryParty=model.IsPrimaryParty==null?false: model.IsPrimaryParty
                    });                    
                
            }
            if (parties.Count > 0) {
                if (parties.Count == 1)
                    parties[0].IsPrimaryParty = true;
                PropertyView.ShowProgressRing();
               await PropertyService.AddPropertyPartyAsync(parties);
                PropertyView.HideProgressRing();
            }
        }

        public async  Task GetPropertyParties(int id) {
            PropertyView.ShowProgressRing();
            PartyList =await PropertyService.GetPartiesOfProperty(id);
            PropertyView.HideProgressRing();
        }

        protected override void ClearItem()
        {
            EnableDocType = true;
            EnablePropertyName = true;
            Item = new PropertyModel() { PropertyId = -1,  PropertyTypeId = "0", CompanyID = "0", TalukId = "0", HobliId = "0", VillageId = "0", DocumentTypeId = "0",DateOfExecution=DateTimeOffset.Now};
            PartySearchQuery = "";
            PartyOptions = null;
            PartyList = null;
            if (DocList != null)
                DocList.Clear();
            PropertyList = null;
            PropertyDocumentTypeList = null;
            ResetCompanyOption();
            ResetTalukOption();
            ResetHobliOption(null);
            ResetVillageOption(null);
            ResetDocumentTypeOption();
        }
        protected override async Task<bool> DeleteItemAsync(PropertyModel model)
        {
            try
            {
                Guid groupGuid= PropertyList[0].GroupGuid.Value;
                StartStatusMessage("Deleting Property...");
                PropertyView.ShowProgressRing();
                var isDeleted = await PropertyService.DeletePropertyAsync(model);
                if (isDeleted == 0)
                {
                    await DialogService.ShowAsync("", "This property is in use ", "Ok");
                    StatusError($"This property is not deleted ");
                    return false;
                }

                if (PropertyList.Count > 1) {
                  var  propList = await PropertyService.GetPropertyByGroupGuidAsync(groupGuid);
                    if (propList != null) {
                        await ReloadProperty(propList[0].GroupGuid.Value, propList[0].PropertyId);
                        EndStatusMessage("Property deleted");
                        return true;
                    }
                }

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
            finally {
                PropertyView.HideProgressRing();
            }
        }

        public async Task ValidationMeassge(string message) {
             await DialogService.ShowAsync("Validation Error", message, "Ok");
        }
        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.PropertyId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Property?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<PropertyModel>> GetValidationConstraints(PropertyModel model)
        {
            yield return new ValidationConstraint<PropertyModel>("Company must be selected", m =>Convert.ToInt32( m.CompanyID) > 0);
            yield return new ValidationConstraint<PropertyModel>("Taluk must be selected", m =>Convert.ToInt32( m.TalukId) > 0);
            yield return new ValidationConstraint<PropertyModel>("Hobli must be selected", m =>Convert.ToInt32(SelectedHobli) > 0);
            yield return new ValidationConstraint<PropertyModel>("Village must be selected", m =>Convert.ToInt32(SelectedVillage) > 0);
            yield return new ValidationConstraint<PropertyModel>("Document Type must be selected", m =>Convert.ToInt32( m.DocumentTypeId) > 0);
            yield return new RequiredConstraint<PropertyModel>("Document No must be entered", m =>Convert.ToInt32( m.DocumentNo));
            yield return new ValidationConstraint<PropertyModel>("Property Type must be selected", m =>Convert.ToInt32( m.PropertyTypeId) > 0);
            yield return new RequiredConstraint<PropertyModel>("Survey No", m => m.SurveyNo);          
            yield return new RequiredConstraint<PropertyModel>("Property Name", m => m.PropertyName);
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
