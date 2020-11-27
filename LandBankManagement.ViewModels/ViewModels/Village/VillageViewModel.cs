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

        public VillageViewModel(ITalukService talukService, IHobliService hobliService, ICommonServices commonServices, IFilePickerService filePickerService, IVillageService villageService) : base(commonServices)
        {
            VillageService = villageService;
            VillageList = new VillageListViewModel(villageService, commonServices);
            VillageDetials = new VillageDetailsViewModel(talukService, hobliService,villageService, filePickerService, commonServices);
        }

        public async Task LoadAsync(VillageListArgs args)
        {
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

        private async Task PopulateDetails(VillageModel selected)
        {
            try
            {
                var model = await VillageService.GetVillageAsync(selected.VillageId);
                selected.Merge(model);
                VillageDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("Village", "Load Details", ex);
            }
        }
    }
}
