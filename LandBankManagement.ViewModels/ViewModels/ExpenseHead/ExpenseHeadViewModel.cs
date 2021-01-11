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
        public ExpenseHeadViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IExpenseHeadService expenseHeadService) : base(commonServices)
        {
            ExpenseHeadService = expenseHeadService;
            ExpenseHeadList = new ExpenseHeadListViewModel(expenseHeadService, commonServices,this);
            ExpenseHeadDetials = new ExpenseHeadDetailsViewModel(expenseHeadService, filePickerService, commonServices, ExpenseHeadList,this);
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
                ShowProgressRing();
                var model = await ExpenseHeadService.GetExpenseHeadAsync(selected.ExpenseHeadId);
                selected.Merge(model);
                ExpenseHeadDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("ExpenseHead", "Load Details", ex);
            }
            finally {
                HideProgressRing();
            }
        }
    }

}

