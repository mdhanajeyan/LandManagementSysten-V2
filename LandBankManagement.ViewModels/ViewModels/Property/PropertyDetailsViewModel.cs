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
            Item.DateOfExecution = DateTimeOffset.Now;
            IsEditMode = true;
            GetDropdowns();
        }

       public void loadAcres(Area area,string type) {

            if (type == "Area")
            {             
                   Item.LandAreaInAcres = Math.Round(area.Acres,2).ToString();
                   Item.LandAreaInGuntas =Math.Round( area.Guntas,2).ToString();
                   Item.LandAreaInSqft = Math.Round(area.SqFt,2).ToString();
                   Item.LandAreaInSqMts = Math.Round(area.SqMeters,2).ToString();
            }
            if (type == "AKarab")
            {
                    Item.AKarabAreaInAcres = Math.Round(area.Acres,2).ToString();
                    Item.AKarabAreaInGuntas = Math.Round(area.Guntas,2).ToString();
                    Item.AKarabAreaInSqft = Math.Round(area.SqFt,2).ToString();
                    Item.AKarabAreaInSqMts = Math.Round(area.SqMeters,2).ToString();
              
            }
            if (type == "BKarab")
            {
                    Item.BKarabAreaInAcres = Math.Round(area.Acres,2).ToString();
                    Item.BKarabAreaInGuntas = Math.Round(area.Guntas,2).ToString();
                    Item.BKarabAreaInSqft = Math.Round(area.SqFt,2).ToString();
                    Item.BKarabAreaInSqMts = Math.Round(area.SqMeters,2).ToString();
            }
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
        }

        public void GetParties() {

            PartyOptions = DropDownService.GetPartyOptions(PartySearchQuery);
        }

        public void PreparePartyList() {
            if (PartyOptions == null)
                return;

            foreach (var item in PartyOptions) {
                if (item.IsSelected) {
                    if (PartyList == null)
                        PartyList = new ObservableCollection<PropertyPartyModel>();
                    PartyList.Add(new PropertyPartyModel { 
                    PartyId=item.Id,
                    PartyName=item.Description
                    });
                }
            }
        }

        public async void LoadPropertyById(int id) {

            var model = PropertyList.First(x => x.PropertyId == id);
            Item = model;
            await GetPropertyParties(id);
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
               await PropertyService.DeletePropertyPartyAsync(model);
               await  GetPropertyParties(Item.PropertyId);
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

        public void DeleteDocument(int id)
        {
            if (id > 0)
            {
                if (DocList[id - 1].blobId > 0)
                {
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

            PartyList = null;
            DocList = null;

        }
       
        protected override async Task<bool> SaveItemAsync(PropertyModel model)
        {
            try
            {
                StartStatusMessage("Saving Property...");

                if (model.PropertyId <= 0)
                   model= await PropertyService.AddPropertyAsync(model, DocList);
                else
                    await PropertyService.UpdatePropertyAsync(model, DocList);
               
                  SaveParties(model);
              await GetPropertyParties(model.PropertyId);
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

        private async void SaveParties(PropertyModel property) {

            if (PartyList == null)
                return;
            var parties = new List<PropertyPartyModel>();
            foreach (var model in PartyList) {
                if (model.PropertyPartyId == 0)
                {
                    parties.Add(new PropertyPartyModel
                    {
                        PartyId=model.PartyId,
                        PropertyId= property.PropertyId,
                        PropertyGuid=property.PropertyGuid

                    });                    
                }
            }
            if (parties.Count > 0) {
               await PropertyService.AddPropertyPartyAsync(parties);
            }
        }

        public async  Task GetPropertyParties(int id) {
            PartyList =await PropertyService.GetPartiesOfProperty(id);
            
        }

        protected override void ClearItem()
        {
            Item = new PropertyModel() { PropertyId = -1,  PropertyTypeId = 0, CompanyID = 0, TalukId = 0, HobliId = 0, VillageId = 0, DocumentTypeId = 0,DateOfExecution=DateTimeOffset.Now};
            PartySearchQuery = "";
            PartyOptions = null;
            PartyList = null;
            if (DocList != null)
                DocList.Clear();
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
