using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class TalukDetailsViewModel : GenericDetailsViewModel<TalukModel>
    {

        public ITalukService TalukService { get; }
        public IFilePickerService FilePickerService { get; }
        public TalukListViewModel TalukListViewModel { get; }
        private TalukViewModel TalukViewModel { get; set; }
        public TalukDetailsViewModel(ITalukService talukService, IFilePickerService filePickerService, ICommonServices commonServices, TalukListViewModel talukListViewModel, TalukViewModel talukViewModel) : base(commonServices)
        {
            TalukService = talukService;
            FilePickerService = filePickerService;
            TalukListViewModel = talukListViewModel;
            TalukViewModel = talukViewModel;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Taluk" : TitleEdit;
        public string TitleEdit => Item == null ? "Taluk" : $"{Item.TalukName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

       // public ExpenseHeadDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync() {
            Item = new TalukModel();
            IsEditMode=true;
        }

        //public async Task LoadAsync(ExpenseHeadDetailsArgs args)
        //{
        //    ViewModelArgs = args ?? ExpenseHeadDetailsArgs.CreateDefault();

        //    if (ViewModelArgs.IsNew)
        //    {
        //        Item = new TalukModel();
        //        IsEditMode = true;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            var item = await TalukService.GetTalukAsync(ViewModelArgs.ExpenseHeadId);
        //            Item = item ?? new ExpenseHeadModel { ExpenseHeadId = ViewModelArgs.ExpenseHeadId, IsEmpty = true };
        //        }
        //        catch (Exception ex)
        //        {
        //            LogException("ExpenseHead", "Load", ex);
        //        }
        //    }
        //}
        //public void Unload()
        //{
        //    ViewModelArgs.ExpenseHeadId = Item?.ExpenseHeadId ?? 0;
        //}

        public void Subscribe()
        {
            MessageService.Subscribe<TalukDetailsViewModel, TalukModel>(this, OnDetailsMessage);
            MessageService.Subscribe<TalukListViewModel>(this, OnListMessage);
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

        
        

        protected override async Task<bool> SaveItemAsync(TalukModel model)
        {
            try
            {
                StartStatusMessage("Saving Taluk...");
                TalukViewModel.ShowProgressRing();
                if (model.TalukId <= 0)
                    await TalukService.AddTalukAsync(model);
                else
                    await TalukService.UpdateTalukAsync(model);
                ClearItem();
                EndStatusMessage("Taluk saved");
                LogInformation("Taluk", "Save", "Taluk saved successfully", $"Taluk {model.TalukName} '{model.TalukName}' was saved successfully.");
                await TalukListViewModel.RefreshAsync();
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Taluk: {ex.Message}");
                LogException("Taluk", "Save", ex);
                return false;
            }
            finally {
                TalukViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = new TalukModel();
        }
        protected override async Task<bool> DeleteItemAsync(TalukModel model)
        {
            try
            {
                StartStatusMessage("Deleting Taluk...");
                TalukViewModel.ShowProgressRing();
                var result = await TalukService.DeleteTalukAsync(model);
                if (!result.IsOk)
                {
                    await DialogService.ShowAsync(result.Message, "");
                    EndStatusMessage("Taluk is deleted");
                    return true;
                }
                ClearItem();
                await TalukListViewModel.RefreshAsync();
                EndStatusMessage("Taluk deleted");
                LogWarning("Taluk", "Delete", "Taluk deleted", $"Taluk {model.TalukId} '{model.TalukName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Taluk: {ex.Message}");
                LogException("Taluk", "Delete", ex);
                return false;
            }
            finally {
                TalukViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.TalukId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Taluk?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<TalukModel>> GetValidationConstraints(TalukModel model)
        {
            yield return new RequiredConstraint<TalukModel>(" Taluk Name", m => m.TalukName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(TalukDetailsViewModel sender, string message, TalukModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.TalukId == current?.TalukId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await TalukService.GetTalukAsync(current.TalukId);
                                    item = item ?? new TalukModel { TalukId = current.TalukId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Taluk has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Taluk", "Handle Changes", ex);
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

        private async void OnListMessage(TalukListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<TalukModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.TalukId == current.TalukId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await TalukService.GetTalukAsync(current.TalukId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Taluk", "Handle Ranges Deleted", ex);
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
