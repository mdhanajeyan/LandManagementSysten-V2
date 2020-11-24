using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class PartyDetailsArgs
    {
        static public PartyDetailsArgs CreateDefault() => new PartyDetailsArgs();

        public int PartyId { get; set; }

        public bool IsNew => PartyId <= 0;
    }
    public class PartyDetailsViewModel : GenericDetailsViewModel<PartyModel>
    {
        public IPartyService PartyService { get; }
        public IFilePickerService FilePickerService { get; }
        public PartyDetailsViewModel(IPartyService partyService, IFilePickerService filePickerService, ICommonServices commonServices) : base(commonServices)
        {
            PartyService = partyService;
            FilePickerService = filePickerService;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Party" : TitleEdit;
        public string TitleEdit => Item == null ? "Party" : $"{Item.PartyName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        public PartyDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(PartyDetailsArgs args)
        {
            ViewModelArgs = args ?? PartyDetailsArgs.CreateDefault();

            if (ViewModelArgs.IsNew)
            {
                Item = new PartyModel();
                IsEditMode = true;
            }
            else
            {
                try
                {
                    var item = await PartyService.GetPartyAsync(ViewModelArgs.PartyId);
                    Item = item ?? new PartyModel { PartyId = ViewModelArgs.PartyId, IsEmpty = true };
                }
                catch (Exception ex)
                {
                    LogException("Party", "Load", ex);
                }
            }
        }
        public void Unload()
        {
            ViewModelArgs.PartyId = Item?.PartyId ?? 0;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<PartyDetailsViewModel, PartyModel>(this, OnDetailsMessage);
            MessageService.Subscribe<PartyListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public PartyDetailsArgs CreateArgs()
        {
            return new PartyDetailsArgs
            {
                PartyId = Item?.PartyId ?? 0
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

                NewPictureSource = result.ImageSource;
            }
            else
            {
                NewPictureSource = null;
            }
        }

        protected override async Task<bool> SaveItemAsync(PartyModel model)
        {
            try
            {
                StartStatusMessage("Saving Party...");
                await Task.Delay(100);
                if (model.PartyId <= 0)
                    await PartyService.AddPartyAsync(model);
                else
                    await PartyService.UpdatePartyAsync(model);
                EndStatusMessage("Party saved");
                LogInformation("Party", "Save", "Party saved successfully", $"Party {model.PartyId} '{model.PartyName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Party: {ex.Message}");
                LogException("Party", "Save", ex);
                return false;
            }
        }

        protected override async Task<bool> DeleteItemAsync(PartyModel model)
        {
            try
            {
                StartStatusMessage("Deleting Party...");
                await Task.Delay(100);
                await PartyService.DeletePartyAsync(model);
                EndStatusMessage("Party deleted");
                LogWarning("Party", "Delete", "Party deleted", $"Party {model.PartyId} '{model.PartyName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Party: {ex.Message}");
                LogException("Party", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current Party?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<PartyModel>> GetValidationConstraints(PartyModel model)
        {
            yield return new RequiredConstraint<PartyModel>("Name", m => m.PartyName);
            //yield return new RequiredConstraint<CompanyModel>("Email", m => m.Email);
            //yield return new RequiredConstraint<CompanyModel>("Phone Number", m => m.PhoneNo);

        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(PartyDetailsViewModel sender, string message, PartyModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.PartyId == current?.PartyId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await PartyService.GetPartyAsync(current.PartyId);
                                    item = item ?? new PartyModel { PartyId = current.PartyId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Party has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Party", "Handle Changes", ex);
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

        private async void OnListMessage(PartyListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<PartyModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.PartyId == current.PartyId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await PartyService.GetPartyAsync(current.PartyId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Party", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This Party has been deleted externally");
            });
        }
    }
}
