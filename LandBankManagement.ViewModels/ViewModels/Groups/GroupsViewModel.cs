using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
   public class GroupsViewModel : ViewModelBase
    {
        IGroupsService GroupsService { get; }
        public GroupsListViewModel GroupsList { get; set; }

        public GroupsDetailsViewModel GroupsDetials { get; set; }
        private bool _progressRingVisibility;
        public bool ProgressRingVisibility
        {
            get => _progressRingVisibility;
            set => Set(ref _progressRingVisibility, value);
        }

        private bool _progressRingActive;
        public bool ProgressRingActive
        {
            get => _progressRingActive;
            set => Set(ref _progressRingActive, value);
        }
        public GroupsViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IGroupsService groupsService, IDropDownService DropDownService) : base(commonServices)
        {
            GroupsService = groupsService;
            GroupsList = new GroupsListViewModel(groupsService, commonServices, this);
            GroupsDetials = new GroupsDetailsViewModel(groupsService, filePickerService, commonServices, GroupsList, this, DropDownService);
        }

        public async Task LoadAsync(GroupsListArgs args)
        {
            await GroupsDetials.LoadAsync();
            await GroupsList.LoadAsync(args);
        }
        public void Unload()
        {
            GroupsList.Unload();
        }
        int noOfApiCalls = 0;
        public void ShowProgressRing()
        {
            noOfApiCalls++;
            ProgressRingActive = true;
            ProgressRingVisibility = true;
        }
        public void HideProgressRing()
        {
            if (noOfApiCalls > 1)
            {
                noOfApiCalls--;
                return;
            }
            else
                noOfApiCalls--;
            ProgressRingActive = false;
            ProgressRingVisibility = false;
        }
        public void Subscribe()
        {
            MessageService.Subscribe<GroupsListViewModel>(this, OnMessage);
            GroupsList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            GroupsList.Unsubscribe();

        }

        private async void OnMessage(GroupsListViewModel viewModel, string message, object args)
        {
            if (viewModel == GroupsList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = GroupsList.SelectedItem;
            if (!GroupsList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        public async Task PopulateDetails(GroupsModel selected)
        {
            try
            {
                ShowProgressRing();
                var model = await GroupsService.GetGroupsAsync(selected.GroupId);
                selected.Merge(model);
                GroupsDetials.Item = model;
                HideProgressRing();
            }
            catch (Exception ex)
            {
                LogException("Groups", "Load Details", ex);
            }
        }
    }
}
