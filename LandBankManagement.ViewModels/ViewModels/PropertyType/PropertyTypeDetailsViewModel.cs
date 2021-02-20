using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
   public class PropertyTypeDetailsViewModel : GenericDetailsViewModel<PropertyTypeModel>
    {
        public IPropertyTypeService PropertyTypeService { get; }
        public IFilePickerService FilePickerService { get; }
        public PropertyTypeListViewModel PropertyTypeList { get;  }
        private PropertyTypeViewModel PropertyTypeViewModel { get; set; }
        public PropertyTypeDetailsViewModel(IPropertyTypeService propertyTypeService, IFilePickerService filePickerService, ICommonServices commonServices, PropertyTypeListViewModel propertyTypeList, PropertyTypeViewModel propertyTypeViewModel) : base(commonServices)
        {
            PropertyTypeService = propertyTypeService;
            FilePickerService = filePickerService;
            PropertyTypeList = propertyTypeList;
            PropertyTypeViewModel = propertyTypeViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New PropertyType" : TitleEdit;
        public string TitleEdit => Item == null ? "PropertyType" : $"{Item.PropertyTypeText}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        private bool IsProcessing = false;
        public async Task LoadAsync()
        {
            Item = new PropertyTypeModel();
            IsEditMode = true;
        }
        public void Unload()
        {

        }

        public void Subscribe()
        {
            MessageService.Subscribe<PropertyTypeDetailsViewModel, PropertyTypeModel>(this, OnDetailsMessage);
            MessageService.Subscribe<PropertyTypeListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
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

               // NewPictureSource = result.ImageSource;
            }
            else
            {
                NewPictureSource = null;
            }
        }

        protected override async Task<bool> SaveItemAsync(PropertyTypeModel model)
        {
            try
            {
                if (IsProcessing)
                    return false;
                IsProcessing = true;
                StartStatusMessage("Saving PropertyType...");
                PropertyTypeViewModel.ShowProgressRing();
                if (model.PropertyTypeId <= 0)
                    await PropertyTypeService.AddPropertyTypeAsync(model);
                else
                    await PropertyTypeService.UpdatePropertyTypeAsync(model);
                ShowPopup("success", "Property Type is Saved");
                await PropertyTypeList.RefreshAsync();
                ClearItem();
                IsProcessing = false;
                EndStatusMessage("PropertyType saved");
                LogInformation("PropertyType", "Save", "PropertyType saved successfully", $"PropertyType {model.PropertyTypeId} '{model.PropertyTypeText}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                IsProcessing = false;
                ShowPopup("error", "Property Type is not Saved");
                StatusError($"Error saving PropertyType: {ex.Message}");
                LogException("PropertyType", "Save", ex);
                return false;
            }
            finally {
                PropertyTypeViewModel.HideProgressRing();
            }
        }

        protected override void ClearItem()
        {
            Item = new PropertyTypeModel();
        }
        protected override async Task<bool> DeleteItemAsync(PropertyTypeModel model)
        {
            try
            {
                StartStatusMessage("Deleting PropertyType...");
                PropertyTypeViewModel.ShowProgressRing();
                await PropertyTypeService.DeletePropertyTypeAsync(model);
                ShowPopup("success", "Property Type is deleted");
                await PropertyTypeList.RefreshAsync();
                ClearItem();
                EndStatusMessage("PropertyType deleted");
                LogWarning("PropertyType", "Delete", "PropertyType deleted", $"PropertyType {model.PropertyTypeId} '{model.PropertyTypeText}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "Property Type is not Deleted");
                StatusError($"Error deleting PropertyType: {ex.Message}");
                LogException("PropertyType", "Delete", ex);
                return false;
            }
            finally {
                PropertyTypeViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.PropertyTypeId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current PropertyType?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<PropertyTypeModel>> GetValidationConstraints(PropertyTypeModel model)
        {
            yield return new RequiredConstraint<PropertyTypeModel>("Property Type", m => m.PropertyTypeText);
         
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(PropertyTypeDetailsViewModel sender, string message, PropertyTypeModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.PropertyTypeId == current?.PropertyTypeId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await PropertyTypeService.GetPropertyTypeAsync(current.PropertyTypeId);
                                    item = item ?? new PropertyTypeModel { PropertyTypeId = current.PropertyTypeId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This PropertyType has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("PropertyType", "Handle Changes", ex);
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

        private async void OnListMessage(PropertyTypeListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<PropertyTypeModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.PropertyTypeId == current.PropertyTypeId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await PropertyTypeService.GetPropertyTypeAsync(current.PropertyTypeId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("PropertyType", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This PropertyType has been deleted externally");
            });
        }
    }
}
