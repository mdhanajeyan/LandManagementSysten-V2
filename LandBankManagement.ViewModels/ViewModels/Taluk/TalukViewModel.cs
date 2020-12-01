using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
   public class TalukViewModel : ViewModelBase
    {
        ITalukService TalukService { get; }
        public TalukListViewModel TalukList { get; set; }

        public TalukDetailsViewModel TalukDetials { get; set; }

        public TalukViewModel(ICommonServices commonServices, IFilePickerService filePickerService, ITalukService talukService) : base(commonServices)
        {
            TalukService = talukService;
            TalukList = new TalukListViewModel(talukService, commonServices);
            TalukDetials = new TalukDetailsViewModel(talukService, filePickerService, commonServices, TalukList);
        }

        public async Task LoadAsync(TalukListArgs args)
        {
            await TalukDetials.LoadAsync();
            await TalukList.LoadAsync(args);
        }
        public void Unload()
        {
            TalukList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<TalukListViewModel>(this, OnMessage);
            TalukList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            TalukList.Unsubscribe();

        }

        private async void OnMessage(TalukListViewModel viewModel, string message, object args)
        {
            if (viewModel == TalukList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = TalukList.SelectedItem;
            if (!TalukList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(TalukModel selected)
        {
            try
            {
                var model = await TalukService.GetTalukAsync(selected.TalukId);
                selected.Merge(model);
                TalukDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("Taluk", "Load Details", ex);
            }
        }
    }
}
