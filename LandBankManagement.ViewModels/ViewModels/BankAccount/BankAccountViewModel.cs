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

        public BankAccountViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IBankAccountService bankAccountService, IDropDownService dropDownService) : base(commonServices)
        {
            BankAccountService = bankAccountService;
            BankAccountList = new BankAccountListViewModel(bankAccountService, commonServices);
            BankAccountDetials = new BankAccountDetailsViewModel(bankAccountService, filePickerService, commonServices, dropDownService, BankAccountList);
        }

        public async Task LoadAsync(BankAccountListArgs args)
        {
            BankAccountDetials.Load();
            await BankAccountList.LoadAsync(args);
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
                var model = await BankAccountService.GetBankAccountAsync(selected.BankAccountId);
                selected.Merge(model);
                BankAccountDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("BankAccount", "Load Details", ex);
            }
        }
    }
}
