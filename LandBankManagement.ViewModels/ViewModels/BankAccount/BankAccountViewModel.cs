using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class BankAccountViewModel : ViewModelBase
    {
        IBankAccountService BankAccountService { get; }
        public BankAccountListViewModel BankAccountList { get; set; }

        public BankAccountDetailsViewModel BankAccountDetials { get; set; }
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

        public BankAccountViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IBankAccountService bankAccountService, IDropDownService dropDownService) : base(commonServices)
        {
            BankAccountService = bankAccountService;
            BankAccountList = new BankAccountListViewModel(bankAccountService, commonServices,this);
            BankAccountDetials = new BankAccountDetailsViewModel(bankAccountService, filePickerService, commonServices, dropDownService, BankAccountList,this);
        }

        public async Task LoadAsync(BankAccountListArgs args)
        {
            BankAccountDetials.Load();
            await BankAccountList.LoadAsync(args);
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
            BankAccountList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<BankAccountListViewModel>(this, OnMessage);
            BankAccountList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            BankAccountList.Unsubscribe();

        }

        private async void OnMessage(BankAccountListViewModel viewModel, string message, object args)
        {
            if (viewModel == BankAccountList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = BankAccountList.SelectedItem;
            if (!BankAccountList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(BankAccountModel selected)
        {
            try
            {
                ShowProgressRing();
                var model = await BankAccountService.GetBankAccountAsync(selected.BankAccountId);
                selected.Merge(model);
                BankAccountDetials.Item = model;
                HideProgressRing();
            }
            catch (Exception ex)
            {
                LogException("BankAccount", "Load Details", ex);
            }
        }
    }
}
