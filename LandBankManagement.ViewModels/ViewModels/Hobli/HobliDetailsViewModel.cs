using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class HobliDetailsViewModel : GenericDetailsViewModel<HobliModel>
    {
    
        public List<ComboBoxOptions> TalukOptions { get; set; }
        public IHobliService HobliService { get; }
        public IFilePickerService FilePickerService { get; }
       public ITalukService TalukService { get; }
        public HobliDetailsViewModel(IHobliService hobliService, IFilePickerService filePickerService, ICommonServices commonServices, ITalukService talukService) : base(commonServices)
        {
            HobliService = hobliService;
            FilePickerService = filePickerService;
            TalukService = talukService;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Hobli" : TitleEdit;
        public string TitleEdit => Item == null ? "Hobli" : $"{Item.HobliName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

       // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public void Load() {
          
            Item = new HobliModel();
            IsEditMode=true;
            GetTaluks();
        }

        private void GetTaluks() {
            var models = TalukService.GetTaluksOptions();
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
                await Task.Delay(100);
                if (model.HobliId <= 0)
                {
                    model.TalukId = 1;
                    await HobliService.AddHobliAsync(model);
                }
                else
                    await HobliService.UpdateHobliAsync(model);
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
        }

        protected override async Task<bool> DeleteItemAsync(HobliModel model)
        {
            try
            {
                StartStatusMessage("Deleting Hobli...");
                await Task.Delay(100);
                await HobliService.DeleteHobliAsync(model);
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
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current Hobli?", "Ok", "Cancel");
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
