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
            Item = new CompanyModel();
            Item.IsActive = true;
            IsEditMode = true;
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

        public void DeleteDocument(int id) {
            if (id > 0) {
                if (DocList[id - 1].blobId > 0) {
                    CompanyService.DeleteCompanyDocumentAsync(DocList[id - 1]);
                }
                DocList.RemoveAt(id-1);
                for (int i = 0; i < DocList.Count; i++)
                {
                    DocList[i].Identity = i + 1;
                }
            }
        }

        protected override async Task<bool> SaveItemAsync(CompanyModel model)
        {
            try
            {               
                StartStatusMessage("Saving Company...");
                await Task.Delay(100);
                if (model.CompanyID <= 0)
                    await CompanyService.AddCompanyAsync(model, DocList);
                else
                    await CompanyService.UpdateCompanyAsync(model, DocList);
                //if (DocList.Count > 0)
                //{
                //    foreach (var doc in DocList)
                //    {
                //        if (doc.guid == null || doc.guid == Guid.Empty)
                //            doc.guid = model.CompanyGuid;
                //    }
                //    await CompanyService.UploadCompanyDocumentsAsync(DocList.ToList());
                //}
                EndStatusMessage("Company saved");
                LogInformation("Company", "Save", "Company saved successfully", $"Company {model.CompanyID} '{model.Name}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Company: {ex.Message}");
                LogException("Company", "Save", ex);
                return false;
            }
        }

        protected override void ClearItem()
        {
            Item = new CompanyModel();
        }
        protected override async Task<bool> DeleteItemAsync(CompanyModel model)
        {
            try
            {
                StartStatusMessage("Deleting Company...");
                await Task.Delay(100);
                await CompanyService.DeleteCompanyAsync(model);
                EndStatusMessage("Company deleted");
                LogWarning("Company", "Delete", "Company deleted", $"Company {model.CompanyID} '{model.Name}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Company: {ex.Message}");
                LogException("Company", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current Company?", "Ok", "Cancel");
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
