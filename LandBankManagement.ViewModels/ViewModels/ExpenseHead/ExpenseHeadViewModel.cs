using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System.Windows.Input;

namespace LandBankManagement.ViewModels
{
   
    public class ExpenseHeadViewModel :  ViewModelBase
    {
        IExpenseHeadService ExpenseHeadService { get; }
        public ExpenseHeadListViewModel ExpenseHeadList { get; set; }

        public ExpenseHeadDetailsViewModel ExpenseHeadDetials { get; set; }

        public ExpenseHeadViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IExpenseHeadService expenseHeadService) : base(commonServices)
        {
            ExpenseHeadService = expenseHeadService;
            ExpenseHeadList = new ExpenseHeadListViewModel(expenseHeadService, commonServices);
            ExpenseHeadDetials = new ExpenseHeadDetailsViewModel(expenseHeadService, filePickerService, commonServices, ExpenseHeadList);
        }

        public async Task LoadAsync(ExpenseHeadListArgs args)
        {
            await ExpenseHeadDetials.LoadAsync();
               await ExpenseHeadList.LoadAsync(args);
        }
        public void Unload()
        {
            ExpenseHeadList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<ExpenseHeadListViewModel>(this, OnMessage);
            ExpenseHeadList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            ExpenseHeadList.Unsubscribe();

        }

        private async void OnMessage(ExpenseHeadListViewModel viewModel, string message, object args)
        {
            if (viewModel == ExpenseHeadList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = ExpenseHeadList.SelectedItem;
            if (!ExpenseHeadList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(ExpenseHeadModel selected)
        {
            try
            {
                var model = await ExpenseHeadService.GetExpenseHeadAsync(selected.ExpenseHeadId);
                selected.Merge(model);
                ExpenseHeadDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("ExpenseHead", "Load Details", ex);
            }
        }
    }

}

