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
        public PropertyViewModel PropertyView { get; set; }
               
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

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(bool fromParty)
        {
            Item = new PropertyModel();
            Item.DateOfExecution = DateTimeOffset.Now;
            IsEditMode = true;
            await GetDropdowns();
            if(fromParty)
            GetStoredItem();
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
            CompanyOptions = await DropDownService.GetCompanyOptions();
            HobliOptions = await DropDownService.GetHobliOptions();
            TalukOptions = await DropDownService.GetTalukOptions();
            VillageOptions = await DropDownService.GetVillageOptions();
            DocumentTypeOptions = await DropDownService.GetDocumentTypeOptions();
            PropertyTypeOptions = await DropDownService.GetPropertyTypeOptions();
            PropertyView.HideProgressRing();
        }

        public async void GetParties() {
            PropertyView.ShowProgressRing();
            PartyOptions =await DropDownService.GetPartyOptions(PartySearchQuery);
            PropertyView.HideProgressRing();
        }

        public void PreparePartyList() {
            if (PartyOptions == null)
                return;

            foreach (var item in PartyOptions) {

                if (item.IsSelected) {
                    if (PartyList != null)
                    {
                        var existParty = PartyList.Where(x => x.PartyId == item.Id).FirstOrDefault();
                        if (existParty != null)
                            continue;
                    }
                    if (PartyList == null)
                        PartyList = new ObservableCollection<PropertyPartyModel>();
                    PartyList.Add(new PropertyPartyModel { 
                    PartyId=item.Id,
                    PartyName=item.Description
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
        public async void LoadPropertyById(int id) {

            var model = PropertyList.First(x => x.PropertyId == id);
            Item = model;
            PropertyView.ShowProgressRing();
            await GetPropertyParties(id);
            PropertyView.HideProgressRing();
            DocList = model.PropertyDocuments;
            if (model.PropertyDocuments != null)
            {
                for (int i = 0; i < DocList.Count; i++)
                {
                    DocList[i].Identity = i + 1;
                }
            }
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

        public ICommand EditPictureCommand => new RelayCommand(OnEditFile);
        private async void OnEditFile()
        {
            var result = await FilePickerService.OpenImagePickerAsync();
            if (result != null)
            {
                if (DocList == null)
                    DocList = new ObservableCollection<ImagePickerResult>();

                foreach (var file in result)
                {
                    DocList.Add(file);
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
                    await PropertyService.SaveDocuments(docs,Item.PropertyGuid);
                    DocList = await PropertyService.GetProeprtyDocuments(Item.PropertyGuid);
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

            Item = null;
            Item = newItem;

            DocList = null;

        }
       
        protected override async Task<bool> SaveItemAsync(PropertyModel model)
        {
            try
            {
                StartStatusMessage("Saving Property...");
                PropertyView.ShowProgressRing();
                if (model.PropertyId <= 0)
                    model = await PropertyService.AddPropertyAsync(model, DocList);
                else
                    await PropertyService.UpdatePropertyAsync(model, DocList);

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

        private async void ReloadProperty(Guid guid,int propertId) {
            PropertyView.ShowProgressRing();
             PropertyList = await PropertyService.GetPropertyByGroupGuidAsync(guid);
            
            var model = PropertyList.Where(x=>x.PropertyId==propertId).FirstOrDefault();
            
            Item = model;
            await GetPropertyParties(model.PropertyId);
            DocList = model.PropertyDocuments;
            if (model.PropertyDocuments != null)
            {
                for (int i = 0; i < DocList.Count; i++)
                {
                    DocList[i].Identity = i + 1;
                }
            }
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
            Item = new PropertyModel() { PropertyId = -1,  PropertyTypeId = 0, CompanyID = 0, TalukId = 0, HobliId = 0, VillageId = 0, DocumentTypeId = 0,DateOfExecution=DateTimeOffset.Now};
            PartySearchQuery = "";
            PartyOptions = null;
            PartyList = null;
            if (DocList != null)
                DocList.Clear();
            PropertyList = null;
        }
        protected override async Task<bool> DeleteItemAsync(PropertyModel model)
        {
            try
            {
                StartStatusMessage("Deleting Property...");
                PropertyView.ShowProgressRing();
                var isDeleted = await PropertyService.DeletePropertyAsync(model);
                if (isDeleted == 0)
                {
                    await DialogService.ShowAsync("", "This property is in use ", "Ok");
                    StatusError($"This property is not deleted ");
                    return false;
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
            yield return new ValidationConstraint<PropertyModel>("Company must be selected", m => m.CompanyID > 0);
            yield return new ValidationConstraint<PropertyModel>("Taluk must be selected", m => m.TalukId > 0);
            yield return new ValidationConstraint<PropertyModel>("Hobli must be selected", m => m.HobliId > 0);
            yield return new ValidationConstraint<PropertyModel>("Village must be selected", m => m.VillageId > 0);
            yield return new ValidationConstraint<PropertyModel>("Document Type must be selected", m => m.DocumentTypeId > 0);
            yield return new RequiredConstraint<PropertyModel>("Document No must be entered", m => m.DocumentNo);
            yield return new ValidationConstraint<PropertyModel>("Property Type must be selected", m => m.PropertyTypeId > 0);
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
