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
    public class HobliDetailsViewModel : GenericDetailsViewModel<HobliModel>
    {
        private ObservableCollection<ComboBoxOptions> _talukOptions = null;
        public ObservableCollection<ComboBoxOptions> TalukOptions
        {
            get => _talukOptions;
            set => Set(ref _talukOptions, value);
        }
        public IHobliService HobliService { get; }
        public IFilePickerService FilePickerService { get; }
      public IDropDownService DropDownService { get; }
        public HobliListViewModel HobliListViewModel { get; }
        private HobliViewModel HobliViewModel { get; set; }
        public HobliDetailsViewModel(IHobliService hobliService, IFilePickerService filePickerService, ICommonServices commonServices, IDropDownService dropDownService,HobliListViewModel hobliListViewModel, HobliViewModel hobliViewModel) : base(commonServices)
        {
            HobliService = hobliService;
            FilePickerService = filePickerService;
            DropDownService = dropDownService;
            HobliListViewModel = hobliListViewModel;
            HobliViewModel = hobliViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Hobli" : TitleEdit;
        public string TitleEdit => Item == null ? "Hobli" : $"{Item.HobliName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

       // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public void Load() {
          
            Item =new HobliModel();
             IsEditMode=true;
           // TalukOptions = new ObservableCollection<ComboBoxOptions>();
            //TalukOptions.Add(new ComboBoxOptions { Id = 1, Description = "test1" });
             GetTaluks();
        }

        private async void GetTaluks() {
            HobliViewModel.ShowProgressRing();
            var models =await DropDownService.GetTalukOptions();
            HobliViewModel.HideProgressRing();
            TalukOptions = models;            
        }       

        public void Subscribe()
        {
            MessageService.Subscribe<HobliDetailsViewModel, HobliModel>(this, OnDetailsMessage);
            MessageService.Subscribe<HobliListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        protected override async Task<bool> SaveItemAsync(HobliModel model)
        {
            try
            {
                StartStatusMessage("Saving Hobli...");
                HobliViewModel.ShowProgressRing();
                if (model.HobliId <= 0)
                {
                    await HobliService.AddHobliAsync(model);
                }
                else
                    await HobliService.UpdateHobliAsync(model);
                await HobliListViewModel.RefreshAsync();
                ClearItem();
                EndStatusMessage("Hobli saved");
                LogInformation("Hobli", "Save", "Hobli saved successfully", $"Hobli {model.HobliName} '{model.HobliName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Hobli: {ex.Message}");
                LogException("Hobli", "Save", ex);
                return false;
            }
            finally {
                HobliViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = HobliModel.CreateEmpty();
        }
        protected override async Task<bool> DeleteItemAsync(HobliModel model)
        {
            try
            {
                StartStatusMessage("Deleting Hobli...");
                HobliViewModel.ShowProgressRing();
                var result = await HobliService.DeleteHobliAsync(model);
                if (!result.IsOk)
                {
                    await DialogService.ShowAsync(result.Message, "");
                    EndStatusMessage(result.Message);
                    return true;
                }
                await HobliListViewModel.RefreshAsync();
                ClearItem();
                EndStatusMessage("Hobli deleted");
                LogWarning("Hobli", "Delete", "Hobli deleted", $"Hobli {model.HobliId} '{model.HobliName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Hobli: {ex.Message}");
                LogException("Hobli", "Delete", ex);
                return false;
            }
            finally { HobliViewModel.HideProgressRing(); }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Hobli?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<HobliModel>> GetValidationConstraints(HobliModel model)
        {
            yield return new RequiredConstraint<HobliModel>("Name", m => m.HobliName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(HobliDetailsViewModel sender, string message, HobliModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.HobliId == current?.HobliId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await HobliService.GetHobliAsync(current.HobliId);
                                    item = item ?? new HobliModel { HobliId = current.HobliId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Hobli has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Hobli", "Handle Changes", ex);
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

        private async void OnListMessage(HobliListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<HobliModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.HobliId == current.HobliId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await HobliService.GetHobliAsync(current.HobliId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Hobli", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This Hobli has been deleted externally");
            });
        }
    }
}
