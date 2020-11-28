using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
   public class VillageDetailsViewModel : GenericDetailsViewModel<VillageModel>
    {
        public ITalukService TalukService { get; }
        public IHobliService HobliService { get; }
        public IVillageService VillageService { get; }
        public IFilePickerService FilePickerService { get; }
        public List<ComboBoxOptions> TalukOptions { get; set; }
        public List<ComboBoxOptions> HobliOptions { get; set; }
        public VillageDetailsViewModel(ITalukService talukService, IHobliService hobliService, IVillageService villageService,IFilePickerService filePickerService, ICommonServices commonServices) : base(commonServices)
        {
            TalukService = talukService;
            HobliService= hobliService;
            FilePickerService = filePickerService;
            VillageService = villageService;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Village" : TitleEdit;
        public string TitleEdit => Item == null ? "Village" : $"{Item.VillageName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync()
        {
            Item = new VillageModel();
            IsEditMode = true;
            GetTaluks();
            GetHobli();
        }
        private void GetTaluks()
        {
            var models = TalukService.GetTaluksOptions();
            TalukOptions = models;

        }
        private void GetHobli()
        {
            var models = HobliService.GetHobliOptions();
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
                await Task.Delay(100);
                if (model.VillageId <= 0)
                    await VillageService.AddVillageAsync(model);
                else
                    await VillageService.UpdateVillageAsync(model);
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
            Item = new VillageModel();
        }
        protected override async Task<bool> DeleteItemAsync(VillageModel model)
        {
            try
            {
                StartStatusMessage("Deleting Village...");
                await Task.Delay(100);
                await VillageService.DeleteVillageAsync(model);
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
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current Village?", "Ok", "Cancel");
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
                            var model = await TalukService.GetTalukAsync(current.VillageId);
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
