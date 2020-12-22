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
        public CompanyDetailsViewModel(ICompanyService companyService, IFilePickerService filePickerService, ICommonServices commonServices) : base(commonServices)
        {
            CompanyService = companyService;
            FilePickerService = filePickerService;
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
                    await CompanyService.UploadCompanyDocumentsAsync(docs, Item.CompanyGuid);
                    DocList = await CompanyService.GetDocuments(Item.CompanyGuid);
                    EndStatusMessage(" Company Document saved");
                }
            }

        }

        public void DeleteDocument(int id) {
            try
            {
                if (id > 0)
                {
                    StartStatusMessage("Deleting  Company Documents...");
                    if (DocList[id - 1].blobId > 0)
                    {
                        ShowProgressRing();
                        CompanyService.DeleteCompanyDocumentAsync(DocList[id - 1]);
                        HideProgressRing();
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
                HideProgressRing();
            }
        }

        public async void DownloadDocument(int id)
        {
            if (id > 0)
            {
               await FilePickerService.DownloadFile(DocList[id - 1].FileName, DocList[id - 1].ImageBytes);
              
            }
        }

        protected override async Task<bool> SaveItemAsync(CompanyModel model)
        {
            try
            {               
                StartStatusMessage("Saving Company...");
                ShowProgressRing();
                              
                if (model.CompanyID <= 0)
                    await CompanyService.AddCompanyAsync(model, DocList);
                else
                    await CompanyService.UpdateCompanyAsync(model, DocList);

                DocList = model.CompanyDocuments;
                HideProgressRing();              
                EndStatusMessage("Company saved");                             
                LogInformation("Company", "Save", "Company saved successfully", $"Company {model.CompanyID} '{model.Name}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                HideProgressRing();
                StatusError($"Error saving Company: {ex.Message}");
                LogException("Company", "Save", ex);
                return false;
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
                StartStatusMessage("Deleting Company...");
                ShowProgressRing();
                
               var result= await CompanyService.DeleteCompanyAsync(model);
                if (!result.IsOk) {
                   await DialogService.ShowAsync(result.Message,"");
                    EndStatusMessage(result.Message);
                    return true;
                }
                ClearItem();
                HideProgressRing();
                EndStatusMessage("Company deleted");
                LogWarning("Company", "Delete", "Company deleted", $"Company {model.CompanyID} '{model.Name}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                HideProgressRing();
                StatusError($"Error deleting Company: {ex.Message}");
                LogException("Company", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete this Company?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<CompanyModel>> GetValidationConstraints(CompanyModel model)
        {
            yield return new RequiredConstraint<CompanyModel>("Name", m => m.Name);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

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
