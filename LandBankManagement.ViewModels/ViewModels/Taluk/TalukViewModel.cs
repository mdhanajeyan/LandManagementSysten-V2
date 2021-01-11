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
        public TalukViewModel(ICommonServices commonServices, IFilePickerService filePickerService, ITalukService talukService) : base(commonServices)
        {
            TalukService = talukService;
            TalukList = new TalukListViewModel(talukService, commonServices,this);
            TalukDetials = new TalukDetailsViewModel(talukService, filePickerService, commonServices, TalukList,this);
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
                ShowProgressRing();
                var model = await TalukService.GetTalukAsync(selected.TalukId);
                selected.Merge(model);
                TalukDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("Taluk", "Load Details", ex);
            }
            HideProgressRing();
        }
    }
}
