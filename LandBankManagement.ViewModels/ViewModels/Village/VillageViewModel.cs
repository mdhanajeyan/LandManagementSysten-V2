using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class VillageViewModel : ViewModelBase
    {
       

        IVillageService VillageService { get; }
        public VillageListViewModel VillageList { get; set; }

        public VillageDetailsViewModel VillageDetials { get; set; }
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
        public VillageViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IVillageService villageService) : base(commonServices)
        {
            VillageService = villageService;
            VillageList = new VillageListViewModel(villageService, commonServices,this);
            VillageDetials = new VillageDetailsViewModel(dropDownService, villageService, filePickerService, commonServices, VillageList,this);
        }

        public async Task LoadAsync(VillageListArgs args)
        {
            await VillageDetials.LoadAsync();
            await VillageList.LoadAsync(args);
        }
        public void Unload()
        {
            VillageList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<VillageListViewModel>(this, OnMessage);
            VillageList.Subscribe();
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
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            VillageList.Unsubscribe();

        }

        private async void OnMessage(VillageListViewModel viewModel, string message, object args)
        {
            if (viewModel == VillageList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = VillageList.SelectedItem;
            if (!VillageList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        public async Task PopulateDetails(VillageModel selected)
        {
            try
            {
                ShowProgressRing();
                VillageDetials.ResetTalukOption();
                VillageDetials.ResetHobliOption(null);
                var model = await VillageService.GetVillageAsync(selected.VillageId);
                VillageDetials.ChangeTalukOptions(model.TalukId);
               // VillageDetials.ChangeHobliOptions( model.HobliId);
                selected.Merge(model);
                VillageDetials.Item = model;
                VillageDetials.SelectedHobli = model.HobliId;
            }
            catch (Exception ex)
            {
                LogException("Village", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }
    }
}
