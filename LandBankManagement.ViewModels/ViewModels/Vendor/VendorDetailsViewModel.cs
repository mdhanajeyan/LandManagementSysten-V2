using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class VendorDetailsArgs
    {
        static public VendorDetailsArgs CreateDefault() => new VendorDetailsArgs();

        public int VendorId { get; set; }

        public bool IsNew => VendorId <= 0;
    }
    public class VendorDetailsViewModel : GenericDetailsViewModel<VendorModel>
    {
        public IVendorService VendorService { get; }
        public IFilePickerService FilePickerService { get; }
        public VendorDetailsViewModel(IVendorService vendorService, IFilePickerService filePickerService, ICommonServices commonServices) : base(commonServices)
        {
            VendorService = vendorService;
            FilePickerService = filePickerService;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Vendor" : TitleEdit;
        public string TitleEdit => Item == null ? "Vendor" : $"{Item.VendorName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        public VendorDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(VendorDetailsArgs args)
        {
            ViewModelArgs = args ?? VendorDetailsArgs.CreateDefault();

            if (ViewModelArgs.IsNew)
            {
                Item = new VendorModel();
                IsEditMode = true;
            }
            else
            {
                try
                {
                    var item = await VendorService.GetVendorAsync(ViewModelArgs.VendorId);
                    Item = item ?? new VendorModel { VendorId = ViewModelArgs.VendorId, IsEmpty = true };
                }
                catch (Exception ex)
                {
                    LogException("Vendor", "Load", ex);
                }
            }
        }
        public void Unload()
        {
            ViewModelArgs.VendorId = Item?.VendorId ?? 0;
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

        public VendorDetailsArgs CreateArgs()
        {
            return new VendorDetailsArgs
            {
                VendorId = Item?.VendorId ?? 0
            };
        }

        private object _newPictureSource = null;
        public object NewPictureSource
        {
            get => _newPictureSource;
            set => Set(ref _newPictureSource, value);
        }

        public override void BeginEdit()
        {
            NewPictureSource = null;
            base.BeginEdit();
        }

        public ICommand EditPictureCommand => new RelayCommand(OnEditFile);
        private async void OnEditFile()
        {
            NewPictureSource = null;
            var result = await FilePickerService.OpenImagePickerAsync();
            if (result != null)
            {

             //   NewPictureSource = result.ImageSource;
            }
            else
            {
                NewPictureSource = null;
            }
        }

        protected override async Task<bool> SaveItemAsync(VendorModel model)
        {
            try
            {
                StartStatusMessage("Saving Vendor...");
                await Task.Delay(100);
                if (model.VendorId <= 0)
                    await VendorService.AddVendorAsync(model);
                else
                    await VendorService.UpdateVendorAsync(model);
                EndStatusMessage("Vendor saved");
                LogInformation("Vendor", "Save", "Vendor saved successfully", $"Vendor {model.VendorId} '{model.VendorName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Vendor: {ex.Message}");
                LogException("Vendor", "Save", ex);
                return false;
            }
        }
        protected override void ClearItem()
        {
            Item = new VendorModel();
        }
        protected override async Task<bool> DeleteItemAsync(VendorModel model)
        {
            try
            {
                StartStatusMessage("Deleting Vendor...");
                await Task.Delay(100);
                await VendorService.DeleteVendorAsync(model);
                EndStatusMessage("Vendor deleted");
                LogWarning("Vendor", "Delete", "Vendor deleted", $"Vendor {model.VendorId} '{model.VendorName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Vendor: {ex.Message}");
                LogException("Vendor", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current Vendor?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<VendorModel>> GetValidationConstraints(VendorModel model)
        {
            yield return new RequiredConstraint<VendorModel>("Name", m => m.VendorName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

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
