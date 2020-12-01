using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
   public class HobliViewModel : ViewModelBase
    {
        IHobliService HobliService { get; }
        public HobliListViewModel HobliList { get; set; }

        public HobliDetailsViewModel HobliDetials { get; set; }

        public HobliViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IHobliService hobliService,IDropDownService dropDownService) : base(commonServices)
        {
            HobliService = hobliService;
            HobliList = new HobliListViewModel(hobliService, commonServices);
            HobliDetials = new HobliDetailsViewModel(hobliService, filePickerService, commonServices, dropDownService);
        }

        public async Task LoadAsync(HobliListArgs args)
        {           
            await HobliList.LoadAsync(args);
        }
        public void Unload()
        {
            HobliList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<HobliListViewModel>(this, OnMessage);
            HobliList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            HobliList.Unsubscribe();

        }

        private async void OnMessage(HobliListViewModel viewModel, string message, object args)
        {
            if (viewModel == HobliList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = HobliList.SelectedItem;
            if (!HobliList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(HobliModel selected)
        {
            try
            {
                var model = await HobliService.GetHobliAsync(selected.HobliId);
                selected.Merge(model);
                model.HobliIsActive = true;
                HobliDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("Hobli", "Load Details", ex);
            }
        }
    }
}
