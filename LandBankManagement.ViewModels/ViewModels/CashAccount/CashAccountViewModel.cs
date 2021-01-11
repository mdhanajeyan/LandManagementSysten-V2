using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;


namespace LandBankManagement.ViewModels
{
    public class CashAccountViewModel : ViewModelBase
    {
        ICashAccountService CashAccountService { get; }
        public CashAccountListViewModel CashAccountList { get; set; }

        public CashAccountDetailsViewModel CashAccountDetials { get; set; }
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

        public CashAccountViewModel(ICommonServices commonServices, IFilePickerService filePickerService, ICashAccountService cashAccountService, IDropDownService dropDownService) : base(commonServices)
        {
            CashAccountService = cashAccountService;
            CashAccountList = new CashAccountListViewModel(cashAccountService, commonServices,this);
            CashAccountDetials = new CashAccountDetailsViewModel(cashAccountService, filePickerService, commonServices, dropDownService, CashAccountList,this);
        }

        public async Task LoadAsync(CashAccountListArgs args)
        {
            CashAccountDetials.Load();
               await CashAccountList.LoadAsync(args);
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
        public void Unload()
        {
            CashAccountList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CashAccountListViewModel>(this, OnMessage);
            CashAccountList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            CashAccountList.Unsubscribe();

        }

        private async void OnMessage(CashAccountListViewModel viewModel, string message, object args)
        {
            if (viewModel == CashAccountList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = CashAccountList.SelectedItem;
            if (!CashAccountList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(CashAccountModel selected)
        {
            try
            {
                ShowProgressRing();
                var model = await CashAccountService.GetCashAccountAsync(selected.CashAccountId);
                selected.Merge(model);
                CashAccountDetials.Item = model;
                HideProgressRing();
            }
            catch (Exception ex)
            {
                LogException("CashAccount", "Load Details", ex);
            }
        }
    }
}
