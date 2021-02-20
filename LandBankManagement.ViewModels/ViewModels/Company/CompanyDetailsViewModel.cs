using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class CompanyDetailsViewModel : GenericDetailsViewModel<CompanyModel>
    {
        private ObservableCollection<ImagePickerResult> _docList = null;
        public ObservableCollection<ImagePickerResult> DocList
        {
            get => _docList;
            set => Set(ref _docList, value);
        }         

        public ICompanyService CompanyService { get; }
        public IFilePickerService FilePickerService { get; }
        public CompanyViewModel CompanyViewModel { get; set; }
        private bool IsProcessing = false;
        public CompanyDetailsViewModel(ICompanyService companyService, IFilePickerService filePickerService, ICommonServices commonServices, CompanyViewModel companyViewModel) : base(commonServices)
        {
            CompanyService = companyService;
            FilePickerService = filePickerService;
            CompanyViewModel = companyViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Company" : TitleEdit;
        public string TitleEdit => Item == null ? "Company" : $"{Item.Name}";

        public override bool ItemIsNew => Item?.IsNew ?? true;


        public async Task LoadAsync()
        {
            Item = new CompanyModel() { IsActive = true };
        }
        public void Unload()
        {

        }

       
        public void Subscribe()
        {
            MessageService.Subscribe<CompanyDetailsViewModel, CompanyModel>(this, OnDetailsMessage);
            MessageService.Subscribe<CompanyListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

       

        public override void BeginEdit()
        {
            base.BeginEdit();
        }

        public ICommand EditPictureCommand => new RelayCommand(OnEditFile);
        private async void OnEditFile()
        {
            var result = await FilePickerService.OpenImagePickerAsync();
            if (result != null)
            {
                if (DocList == null)
                    DocList =new ObservableCollection<ImagePickerResult>();

                foreach (var file in result)
                {
                    DocList.Add(file);
                }
                for (int i = 0; i < DocList.Count; i++) {
                    DocList[i].Identity = i+1;
                }
            }
           
        }

        public ICommand SavePictureCommand => new RelayCommand(OnSaveFile);
        private async void OnSaveFile()
        {
            if (Item.CompanyID > 0)
            {
                if (DocList == null)
                    return;

                List<ImagePickerResult> docs = new List<ImagePickerResult>();
                foreach (var doc in DocList)
                {
                    if (doc.blobId == 0)
                    {
                        docs.Add(doc);
                    }
                }

                if (docs.Count > 0)
                {
                    StartStatusMessage("Saving Company Documents...");
                    CompanyViewModel.ShowProgressRing();
                    await CompanyService.UploadCompanyDocumentsAsync(docs, Item.CompanyGuid);
                    DocList = await CompanyService.GetDocuments(Item.CompanyGuid);
                    for (int i = 0; i < DocList.Count; i++)
                    {
                        DocList[i].Identity = i + 1;
                    }
                    CompanyViewModel.HideProgressRing();
                    EndStatusMessage(" Company Document saved");
                }
            }

        }

        public async void DeleteDocument(int id) {
            try
            {
                if (id > 0)
                {
                    StartStatusMessage("Deleting  Company Documents...");
                    if (DocList[id - 1].blobId > 0)
                    {
                        CompanyViewModel.ShowProgressRing();
                        await CompanyService.DeleteCompanyDocumentAsync(DocList[id - 1]);
                        CompanyViewModel.HideProgressRing();
                    }
                    DocList.RemoveAt(id - 1);
                    var newlist = DocList;
                    for (int i = 0; i < newlist.Count; i++)
                    {
                        newlist[i].Identity = i + 1;
                    }
                    DocList = null;
                    DocList = newlist;
                    EndStatusMessage(" Company Document delted");
                }
            }
            catch (Exception ) {
                CompanyViewModel.HideProgressRing();
            }
        }

        public async void DownloadDocument(int id)
        {
            if (id > 0)
            {
                StartStatusMessage("Start downloading...");
                var result=  await FilePickerService.DownloadFile(DocList[id - 1].FileName, DocList[id - 1].ImageBytes, DocList[id - 1].ContentType);
              if(result)
                    StartStatusMessage("File downloaded...");
              else
                    EndStatusMessage("Download failed");
            }
        }

        protected override async Task<bool> SaveItemAsync(CompanyModel model)
        {
            try
            {
                if (IsProcessing)
                    return false;
                IsProcessing = true;
                StartStatusMessage("Saving Company...");
                CompanyViewModel.ShowProgressRing();

                if (model.CompanyID <= 0)
                    await CompanyService.AddCompanyAsync(model, DocList);
                else
                    await CompanyService.UpdateCompanyAsync(model, DocList);

                DocList = model.CompanyDocuments;
                if (DocList != null)
                {
                    for (int i = 0; i < DocList.Count; i++)
                    {
                        DocList[i].Identity = i + 1;
                    }
                }
                IsProcessing = false;
                ShowPopup("success", "Company saved");
                EndStatusMessage("Company saved");                             
                LogInformation("Company", "Save", "Company saved successfully", $"Company {model.CompanyID} '{model.Name}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                IsProcessing = false;
                ShowPopup("error", "Unable to save company");
                StatusError($"Error saving Company: {ex.Message}");
                LogException("Company", "Save", ex);
                return false;
            }
            finally{
                CompanyViewModel.HideProgressRing();
            }
        }

        protected override void ClearItem()
        {           
            Item = new CompanyModel() { IsActive = true };
            if (DocList!=null)
            DocList.Clear();
        }
        protected override async Task<bool> DeleteItemAsync(CompanyModel model)
        {
            try
            {
                if (model.CompanyID == 0)
                    return false;
                StartStatusMessage("Deleting Company...");
                CompanyViewModel.ShowProgressRing();

                var result = await CompanyService.DeleteCompanyAsync(model);
                if (!result.IsOk)
                {
                    await DialogService.ShowAsync(result.Message, "");
                    EndStatusMessage(result.Message);
                    return true;
                }
                ClearItem();
                ShowPopup("success", "Company is deleted");
                EndStatusMessage("Company deleted");
                LogWarning("Company", "Delete", "Company deleted", $"Company {model.CompanyID} '{model.Name}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "Company is not deleted");
                StatusError($"Error deleting Company: {ex.Message}");
                LogException("Company", "Delete", ex);
                return false;
            }
            finally {
                CompanyViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.CompanyID == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete this Company?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<CompanyModel>> GetValidationConstraints(CompanyModel model)
        {
            yield return new RequiredConstraint<CompanyModel>("Name", m => m.Name);
            yield return new ValidationConstraint<CompanyModel>("PAN Number is not Valid", x => ValidatePanNumber(x));
            yield return new ValidationConstraint<CompanyModel>("Email is not Valid", x => ValidateEmail(x));
            yield return new ValidationConstraint<CompanyModel>("Phone Number is not Valid", x => ValidatePhone(x));
            yield return new ValidationConstraint<CompanyModel>("Pin Code is not Valid", x => ValidatePinCode(x));
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }
        private bool ValidatePanNumber(CompanyModel model)
        {
            if (string.IsNullOrEmpty(model.PAN))
                return true;
            Regex regex = new Regex("([A-Z]){5}([0-9]){4}([A-Z]){1}$");
            if (!regex.IsMatch(model.PAN.Trim()))
            {
                return false;
            }
            return true;
        }

        private bool ValidateEmail(CompanyModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return true;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(model.Email.Trim()))
            {
                return false;
            }
            return true;
        }

        private bool ValidatePhone(CompanyModel model)
        {
            if (string.IsNullOrEmpty(model.PhoneNo))
                return true;
            if (model.PhoneNo.Length<10)
                return false;
            Regex regex = new Regex(@"^[0-9]+$");
            if (!regex.IsMatch(model.PhoneNo.Trim()))
            {
                return false;
            }
            return true;
        }

        private bool ValidatePinCode(CompanyModel model)
        {
            if (string.IsNullOrEmpty(model.Pincode))
                return true;
           
            Regex regex = new Regex(@"^[0-9]+$");
            if (!regex.IsMatch(model.Pincode.Trim()))
            {
                return false;
            }
            return true;
        }



        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(CompanyDetailsViewModel sender, string message, CompanyModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.CompanyID == current?.CompanyID)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await CompanyService.GetCompanyAsync(current.CompanyID);
                                    item = item ?? new CompanyModel { CompanyID = current.CompanyID, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Company has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Company", "Handle Changes", ex);
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

        private async void OnListMessage(CompanyListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<CompanyModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.CompanyID == current.CompanyID))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await CompanyService.GetCompanyAsync(current.CompanyID);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Company", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This Company has been deleted externally");
            });
        }
    }
}
