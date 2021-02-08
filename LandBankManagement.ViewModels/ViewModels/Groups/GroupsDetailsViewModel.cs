using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LandBankManagement.ViewModels
{
    public class GroupsDetailsViewModel : GenericDetailsViewModel<GroupsModel>
    {
        public IDropDownService DropDownService { get; }
        public IGroupsService GroupsService { get; }
        public IFilePickerService FilePickerService { get; }
        public GroupsListViewModel GroupsListViewModel { get; }
        public GroupsViewModel GroupsViewModel { get; set; }
        private ObservableCollection<ComboBoxOptions> _groupsOptions = null;
        public ObservableCollection<ComboBoxOptions> GroupsOptions

        {
            get => _groupsOptions;
            set => Set(ref _groupsOptions, value);
        }

        public GroupsDetailsViewModel(IGroupsService groupsService, IFilePickerService filePickerService, ICommonServices commonServices, GroupsListViewModel groupsListViewModel, GroupsViewModel groupsViewModel, IDropDownService dropDownService) : base(commonServices)
        {
            GroupsService = groupsService;
            FilePickerService = filePickerService;
            GroupsListViewModel = groupsListViewModel;
            GroupsViewModel = groupsViewModel;
            DropDownService = dropDownService;
        }

        override public string Title => (Item?.IsNew ?? true) ? "New Groups" : TitleEdit;
        public string TitleEdit => Item == null ? "Groups" : $"{Item.GroupName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;


        public async Task LoadAsync()
        {
            Item = new GroupsModel { IsActive = true };
            GroupsOptions = DropDownService.GetGroupsTypeOptions();
        }
        public void Unload()
        {

        }

        public void Subscribe()
        {
            MessageService.Subscribe<GroupsDetailsViewModel, GroupsModel>(this, OnDetailsMessage);
            MessageService.Subscribe<GroupsListViewModel>(this, OnListMessage);
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

        protected override async Task<bool> SaveItemAsync(GroupsModel model)
        {
            try
            {
                StartStatusMessage("Saving Groups...");
                GroupsViewModel.ShowProgressRing();
                if (model.GroupId <= 0)
                    await GroupsService.AddGroupsAsync(model);
                else
                    await GroupsService.UpdateGroupsAsync(model);

                await GroupsListViewModel.RefreshAsync();
                ClearItem();
                EndStatusMessage("Groups saved");
                LogInformation("Groups", "Save", "Groups saved successfully", $"Groups {model.GroupId} '{model.GroupName}' was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Party: {ex.Message}");
                LogException("Party", "Save", ex);
                return false;
            }
            finally
            {
                GroupsViewModel.HideProgressRing();
            }
        }
        protected override void ClearItem()
        {
            Item = new GroupsModel { IsActive = true };
        }
        protected override async Task<bool> DeleteItemAsync(GroupsModel model)
        {
            try
            {
                StartStatusMessage("Deleting Groups...");
                GroupsViewModel.ShowProgressRing();
                await GroupsService.DeleteGroupsAsync(model);
                ClearItem();
                await GroupsListViewModel.RefreshAsync();
                EndStatusMessage("Groups deleted");
                LogWarning("Groups", "Delete", "Groups deleted", $"Groups {model.GroupId} '{model.GroupName}' was deleted.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Groups: {ex.Message}");
                LogException("Groups", "Delete", ex);
                return false;
            }
            finally
            {
                GroupsViewModel.HideProgressRing();
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Item.GroupId == 0)
                return false;
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure to delete current Groups?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<GroupsModel>> GetValidationConstraints(GroupsModel model)
        {
            yield return new RequiredConstraint<GroupsModel>("Group Name", m => m.GroupName);
            yield return new ValidationConstraint<GroupsModel>("Group Type must be selected", m => Convert.ToInt32(m.GroupType ?? "0") > 0);
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(GroupsDetailsViewModel sender, string message, GroupsModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.GroupId == current?.GroupId)
                {
                    switch (message)
                    {
                        case "ItemChanged":
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await GroupsService.GetGroupsAsync(current.GroupId);
                                    item = item ?? new GroupsModel { GroupId = current.GroupId, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This Groups has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Groups", "Handle Changes", ex);
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

        private async void OnListMessage(GroupsListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case "ItemsDeleted":
                        if (args is IList<GroupsModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.GroupId == current.GroupId))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case "ItemRangesDeleted":
                        try
                        {
                            var model = await GroupsService.GetGroupsAsync(current.GroupId);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Groups", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This Groups has been deleted externally");
            });
        }

    }
}
