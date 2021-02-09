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

    public class VendorDetailsViewModel : GenericDetailsViewModel<VendorModel>
    {
        private ObservableCollection<ImagePickerResult> _docList = null;
        public ObservableCollection<ImagePickerResult> DocList
        {
            get => _docList;
            set => Set(ref _docList, value);
        }

        private ObservableCollection<ComboBoxOptions> _solutationOptions = null;
        public ObservableCollection<ComboBoxOptions> SolutationOptions
        {
            get => _solutationOptions;
            set => Set(ref _solutationOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _groupOptions = null;
        public ObservableCollection<ComboBoxOptions> GroupOptions
        {
            get => _groupOptions;
            set => Set(ref _groupOptions, value);
        }

        public IVendorService VendorService { get; }
        public IDropDownService DropdownService { get; }
        public IFilePickerService FilePickerService { get; }
        private VendorViewModel VendorViewModel { get; set; }
        public VendorDetailsViewModel(IVendorService vendorService, IFilePickerService filePickerService, ICommonServices commonServices, IDropDownService dropdownService, VendorViewModel vendorViewModel) : base(commonServices)
        {
            VendorService = vendorService;
            FilePickerService = filePickerService;
            VendorViewModel = vendorViewModel;
            DropdownService = dropdownService;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Vendor" : TitleEdit;
        public string TitleEdit => Item == null ? "Vendor" : $"{Item.VendorName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        public async Task LoadAsync()
        {
            Item = new VendorModel { IsVendorActive = true, SalutationType ="1"};
            Item.IsVendorActive = true;
            IsEditMode = true;
            SolutationOptions = DropdownService.GetSalutationOptions();
            GroupOptions = await DropdownService.GetGroupsOptions();
        }
        public void Unload()
        {

        }

        public void Subscribe()
        {
            MessageService.Subscribe<VendorDetailsViewModel, VendorModel>(this, OnDetailsMessage);
            MessageService.Subscribe<VendorListViewModel>(this, OnListMessage);
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
            if (Item.VendorId > 0)
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
                    StartStatusMessage("Saving Vendor Documents...");
                    VendorViewModel.ShowProgressRing();
                    await VendorService.UploadVendorDocumentsAsync(docs, Item.VendorGuid);
                    DocList = await VendorService.GetDocuments(Item.VendorGuid);
                    for (int i = 0; i < DocList.Count; i++)
                    {
                        DocList[i].Identity = i + 1;
                    }
                    VendorViewModel.HideProgressRing();
                    EndStatusMessage(" Vendor Document saved");
                }
            }

        }

        public async void DeleteDocument(int id)
        {
            if (id > 0)
            {
                StartStatusMessage("Deleting Vendor Documents...");
                if (DocList[id - 1].blobId > 0)
                {
                    VendorViewModel.ShowProgressRing();
                      await VendorService.DeleteVendorDocumentAsync(DocList[id - 1]);
                }
                DocList.RemoveAt(id - 1);
                var newlist = DocList;
                for (int i = 0; i < newlist.Count; i++)
                {
                    newlist[i].Identity = i + 1;
                }
                DocList = null;
                DocList = newlist;
                VendorViewModel.HideProgressRing();
                EndStatusMessage(" Vendor Document deleted");
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
        protected override async Task<bool> SaveItemAsync(VendorModel model)
        {
            try
            {
                StartStatusMessage("Saving Vendor...");
                VendorViewModel.ShowProgressRing();
                if (model.VendorId <= 0)
                    await VendorService.AddVendorAsync(model, DocList);
                else
                    await VendorService.UpdateVendorAsync(model, DocList);
                DocList = model.VendorDocuments;
                ShowPopup("success", "Vendor is Saved");
                EndStatusMessage("Vendor saved");
                LogInformation("Vendor", "Save", "Vendor saved successfully", $"Vendor {model.VendorId} '{model.VendorName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "Vendor is not Saved");
                StatusError($"Error saving Vendor: {ex.Message}");
                LogException("Vendor", "Save", ex);
                return false;
            }
            finally {
                VendorViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = new VendorModel() { IsVendorActive=true,SalutationType="1"};
            if (DocList != null)
                DocList.Clear();
        }
        protected override async Task<bool> DeleteItemAsync(VendorModel model)
        {
            try
            {
                StartStatusMessage("Deleting Vendor...");
                VendorViewModel.ShowProgressRing();
                await VendorService.DeleteVendorAsync(model);
                ClearItem();
                ShowPopup("success", "Vendor is deleted");
                EndStatusMessage("Vendor deleted");
                LogWarning("Vendor", "Delete", "Vendor deleted", $"Vendor {model.VendorId} '{model.VendorName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "Vendor is not deleted");
                StatusError($"Error deleting Vendor: {ex.Message}");
                LogException("Vendor", "Delete", ex);
                return false;
            }
            finally {
                VendorViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.VendorId == 0)
                return false;

            return await DialogService.ShowAsync("Confirm Delete", "Are you sure  to delete this Vendor?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<VendorModel>> GetValidationConstraints(VendorModel model)
        {
            yield return new RequiredConstraint<VendorModel>("Name", m => m.VendorName);
            yield return new ValidationConstraint<VendorModel>("PAN Number is not Valid", x => ValidatePanNumber(x));
            yield return new ValidationConstraint<VendorModel>("Aadhar Number is not Valid", x => ValidateAadhar(x));
            yield return new ValidationConstraint<VendorModel>("Email is not Valid", x => ValidateEmail(x));
            yield return new ValidationConstraint<VendorModel>("Phone Number is not Valid", x => ValidatePhone(x));
            yield return new ValidationConstraint<VendorModel>("Pin Code is not Valid", x => ValidatePinCode(x));
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }
        private bool ValidatePanNumber(VendorModel model)
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

        private bool ValidateEmail(VendorModel model)
        {
            if (string.IsNullOrEmpty(model.email))
                return true;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(model.email.Trim()))
            {
                return false;
            }
            return true;
        }

        private bool ValidatePhone(VendorModel model)
        {
            if (string.IsNullOrEmpty(model.PhoneNo))
                return true;
            if (model.PhoneNo.Length < 10)
                return false;
            Regex regex = new Regex(@"^[0-9]+$");
            if (!regex.IsMatch(model.PhoneNo.Trim()))
            {
                return false;
            }
            return true;
        }

        private bool ValidateAadhar(VendorModel model)
        {
            if (string.IsNullOrEmpty(model.AadharNo))
                return true;
            if (model.AadharNo.Length < 10)
                return false;
            Regex regex = new Regex(@"^(\d{12}|\d{16})$");
            if (!regex.IsMatch(model.AadharNo.Trim()))
            {
                return false;
            }
            return true;
        }

        private bool ValidatePinCode(VendorModel model)
        {
            if (string.IsNullOrEmpty(model.PinCode))
                return true;

            Regex regex = new Regex(@"^[0-9]+$");
            if (!regex.IsMatch(model.PinCode.Trim()))
            {
                return false;
            }
            return true;
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(VendorDetailsViewModel sender, string message, VendorModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.VendorId == current?.VendorId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await VendorService.GetVendorAsync(current.VendorId);
                                    item = item ?? new VendorModel { VendorId = current.VendorId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Vendor has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Vendor", "Handle Changes", ex);
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

        private async void OnListMessage(VendorListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<VendorModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.VendorId == current.VendorId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await VendorService.GetVendorAsync(current.VendorId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Vendor", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This Vendor has been deleted externally");
            });
        }
    }
}
