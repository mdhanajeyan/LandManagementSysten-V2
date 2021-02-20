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
        private ObservableCollection<ComboBoxOptions> _allTalukOptions = null;
        public ObservableCollection<ComboBoxOptions> AllTalukOptions
        {
            get => _allTalukOptions;
            set => Set(ref _allTalukOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _activeTalukOptions = null;
        public ObservableCollection<ComboBoxOptions> ActiveTalukOptions
        {
            get => _activeTalukOptions;
            set => Set(ref _activeTalukOptions, value);
        }

        private bool _showTaluk = true;
        public bool ShowActiveTaluk
        {
            get => _showTaluk;
            set => Set(ref _showTaluk, value);
        }

        private bool _hideTaluk = false;
        public bool ChangeTaluk
        {
            get => _hideTaluk;
            set => Set(ref _hideTaluk, value);
        }


        private ObservableCollection<ComboBoxOptions> _hobliOptions = null;
        public ObservableCollection<ComboBoxOptions> HobliOptions
        {
            get => _hobliOptions;
            set => Set(ref _hobliOptions, value);
        } 
        private ObservableCollection<ComboBoxOptions> _allHobliOptions = null;
        public ObservableCollection<ComboBoxOptions> AllHobliOptions
        {
            get => _allHobliOptions;
            set => Set(ref _allHobliOptions, value);
        }
        private ObservableCollection<ComboBoxOptions> _activeHobliOptions = null;
        public ObservableCollection<ComboBoxOptions> ActiveHobliOptions
        {
            get => _activeHobliOptions;
            set => Set(ref _activeHobliOptions, value);
        }

        private bool _showHobli = true;
        public bool ShowActiveHobli
        {
            get => _showHobli;
            set => Set(ref _showHobli, value);
        }

        private bool _hideHobli = false;
        public bool ChangeHobli
        {
            get => _hideHobli;
            set => Set(ref _hideHobli, value);
        }
        private string _selectedHobli = "0";
        public string SelectedHobli
        {
            get => _selectedHobli;
            set => Set(ref _selectedHobli, value);
        }

        private VillageViewModel VillageViewModel { get; set; }
        private bool IsProcessing = false;
        public VillageDetailsViewModel(IDropDownService dropDownService, IVillageService villageService,IFilePickerService filePickerService, ICommonServices commonServices, VillageListViewModel villageListViewModel, VillageViewModel villageViewModel) : base(commonServices)
        {
            DropDownService = dropDownService;
            FilePickerService = filePickerService;
            VillageService = villageService;
            VillageListViewModel = villageListViewModel;
            VillageViewModel = villageViewModel;
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
            VillageViewModel.ShowProgressRing();
            await GetDropdowns();
            VillageViewModel.HideProgressRing();
            ResetTalukOption();
         //   ResetHobliOption(null);
            
        }
        private async Task GetDropdowns()
        {
            ActiveTalukOptions = await DropDownService.GetTalukOptions();
            AllTalukOptions = await DropDownService.GetAllTalukOptions();
            ActiveHobliOptions = await DropDownService.GetHobliOptions();
            AllHobliOptions = await DropDownService.GetAllHobliOptions();
        }
       

        public void ChangeHobliOptions(string hobliId)
        {
            var comp = ActiveHobliOptions.Where(x =>x.Id == hobliId).FirstOrDefault();
            if (comp != null|| hobliId==null)
            {
                ResetHobliOption(hobliId);
                return;
            }
            HobliOptions = AllHobliOptions;
            SelectedHobli = "0";
            SelectedHobli = hobliId;
            ShowActiveHobli = false;
            ChangeHobli = true;
           
        }

        public void ResetHobliOption(string hobliId)
        {
            HobliOptions = ActiveHobliOptions;
            ShowActiveHobli = true;
            ChangeHobli = false;
            if(hobliId!=null)
                SelectedHobli = hobliId;
        }

        public void ChangeTalukOptions(string talukId)
        {
            var comp = ActiveTalukOptions.Where(x => x.Id == talukId).FirstOrDefault();
            if (comp != null)
            {
                ResetTalukOption();
                return;
            }
            TalukOptions = AllTalukOptions;
            ShowActiveTaluk = false;
            ChangeTaluk = true;
        }

        public void ResetTalukOption()
        {
            TalukOptions = ActiveTalukOptions;
            ShowActiveTaluk = true;
            ChangeTaluk = false;
        }
        public async Task LoadHobli()
        {
            var hobliId = Item.HobliId;
            int id = Convert.ToInt32(Item.TalukId);
            HobliOptions = await DropDownService.GetHobliOptionsByTaluk(id);
            if (HobliOptions.Count <= 1)
            {
                ChangeHobliOptions(Item.HobliId);
            }
            else
            {
                SelectedHobli = "0";
                SelectedHobli = hobliId;
            }
           // 
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
                if (IsProcessing)
                    return false;
                IsProcessing = true;
                StartStatusMessage("Saving Village...");
                VillageViewModel.ShowProgressRing();
                model.HobliId = SelectedHobli;
                if (model.VillageId <= 0)
                    await VillageService.AddVillageAsync(model);
                else
                    await VillageService.UpdateVillageAsync(model);
                ClearItem();
                IsProcessing = false;
                await VillageListViewModel.RefreshAsync();
                ShowPopup("success", "Village is Saved");
                EndStatusMessage("Village saved");
                LogInformation("Village", "Save", "Village saved successfully", $"Village {model.VillageId} '{model.VillageName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                IsProcessing = false;
                ShowPopup("error", "Village is not Saved");
                StatusError($"Error saving Village: {ex.Message}");
                LogException("Village", "Save", ex);
                return false;
            }
            finally {
                VillageViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = new VillageModel() { TalukId = "0", HobliId = "0" ,VillageId=0,VillageIsActive=true};
            ResetTalukOption();
            ResetHobliOption(null);
            SelectedHobli = "0";
        }
        protected override async Task<bool> DeleteItemAsync(VillageModel model)
        {
            try
            {
                StartStatusMessage("Deleting Village...");
                VillageViewModel.ShowProgressRing();
                var result = await VillageService.DeleteVillageAsync(model);
                if (!result.IsOk)
                {
                    await DialogService.ShowAsync(result.Message, "");
                    EndStatusMessage(result.Message);
                    return true;
                }
                ShowPopup("success", "Village is deleted");
                ClearItem();
                await VillageListViewModel.RefreshAsync();
                EndStatusMessage("Village deleted");
                LogWarning("Village", "Delete", "Village deleted", $"Taluk {model.VillageId} '{model.VillageName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                ShowPopup("error", "Village is not deleted");
                StatusError($"Error deleting Village: {ex.Message}");
                LogException("Village", "Delete", ex);
                return false;
            }
            finally {
                VillageViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.VillageId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Village?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<VillageModel>> GetValidationConstraints(VillageModel model)
        {
            yield return new ValidationConstraint<VillageModel>("Taluk must be selected", m =>Convert.ToInt32( m.TalukId)>0);
            yield return new ValidationConstraint<VillageModel>("Hobli must be selected", m =>Convert.ToInt32( SelectedHobli)>0);
            yield return new RequiredConstraint<VillageModel>("Village Name", m => m.VillageName);           
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
