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
   public class VillageDetailsViewModel : GenericDetailsViewModel<VillageModel>
    {
       public IDropDownService DropDownService { get; }
        public IVillageService VillageService { get; }
        public IFilePickerService FilePickerService { get; }
        public VillageListViewModel VillageListViewModel { get; }
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
        public VillageDetailsViewModel(IDropDownService dropDownService, IVillageService villageService,IFilePickerService filePickerService, ICommonServices commonServices, VillageListViewModel villageListViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            VillageService = villageService;
            VillageListViewModel = villageListViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Village" : TitleEdit;
        public string TitleEdit => Item == null ? "Village" : $"{Item.VillageName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new VillageModel();
            Item.VillageIsActive = true;
            IsEditMode = true;
            GetTaluks();
            GetHobli();
        }
        private void GetTaluks()
        {
            var models = DropDownService.GetTalukOptions();
            TalukOptions = models;

        }
        private void GetHobli()
        {
            var models = DropDownService.GetHobliOptions();
            HobliOptions = models;

        }

        public void Subscribe()
        {
            MessageService.Subscribe<VillageDetailsViewModel, VillageModel>(this, OnDetailsMessage);
            MessageService.Subscribe<VillageListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        //public ExpenseHeadDetailsArgs CreateArgs()
        //{
        //    return new ExpenseHeadDetailsArgs
        //    {
        //        ExpenseHeadId = Item?.ExpenseHeadId ?? 0
        //    };
        //}


        protected override async Task<bool> SaveItemAsync(VillageModel model)
        {
            try
            {
                StartStatusMessage("Saving Village...");
                
                if (model.VillageId <= 0)
                    await VillageService.AddVillageAsync(model);
                else
                    await VillageService.UpdateVillageAsync(model);
                ClearItem();
                await VillageListViewModel.RefreshAsync();
                EndStatusMessage("Village saved");
                LogInformation("Village", "Save", "Village saved successfully", $"Village {model.VillageId} '{model.VillageName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Village: {ex.Message}");
                LogException("Village", "Save", ex);
                return false;
            }
        }
        protected override void ClearItem()
        {
            Item = new VillageModel() { TalukId = 0, HobliId = 0 ,VillageId=0,VillageIsActive=true};
        }
        protected override async Task<bool> DeleteItemAsync(VillageModel model)
        {
            try
            {
                StartStatusMessage("Deleting Village...");
                
                await VillageService.DeleteVillageAsync(model);
                ClearItem();
                 await VillageListViewModel.RefreshAsync();
                EndStatusMessage("Village deleted");
                LogWarning("Village", "Delete", "Village deleted", $"Taluk {model.VillageId} '{model.VillageName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Village: {ex.Message}");
                LogException("Village", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Village?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<VillageModel>> GetValidationConstraints(VillageModel model)
        {
            yield return new RequiredConstraint<VillageModel>("Name", m => m.VillageName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(VillageDetailsViewModel sender, string message, VillageModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.VillageId == current?.VillageId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await VillageService.GetVillageAsync(current.VillageId);
                                    item = item ?? new VillageModel { VillageId = current.VillageId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Village has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Village", "Handle Changes", ex);
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

        private async void OnListMessage(VillageListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<VillageModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.VillageId == current.VillageId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await VillageService.GetVillageAsync(current.VillageId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Village", "Handle Ranges Deleted", ex);
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
