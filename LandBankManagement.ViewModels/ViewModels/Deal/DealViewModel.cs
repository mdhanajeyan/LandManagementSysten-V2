using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class DealViewModel : ViewModelBase
    {
        IDealService DealService { get; }
        public DealListViewModel DealList { get; set; }

        public DealDetailsViewModel DealDetails { get; set; }
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
        public DealViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IDealService dealService) : base(commonServices)
        {
            DealService = dealService;

            DealDetails = new DealDetailsViewModel(dropDownService, dealService, filePickerService, commonServices, this);
            DealList = new DealListViewModel(dealService, commonServices, this);
        }

        public async Task LoadAsync(DealListArgs args)
        {
            await DealDetails.LoadAsync();
            await DealList.LoadAsync(args);
        }
        public void Unload()
        {
            DealList.Unload();
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
            MessageService.Subscribe<DealListViewModel>(this, OnMessage);
            DealList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            DealList.Unsubscribe();

        }

        private async void OnMessage(DealListViewModel viewModel, string message, object args)
        {
            if (viewModel == DealList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = DealList.SelectedItem;
            if (!DealList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        public async Task PopulateDetails(DealModel selected)
        {
            try
            {
                SelectedPivotIndex = 1;
                ShowProgressRing();
                var model = await DealService.GetDealAsync(selected.DealId);
                selected.Merge(model);
                DealDetails.Item = model;
                DealDetails.Sale1 = model.SaleValue1.ToString();
                DealDetails.Sale2 = model.SaleValue2.ToString();
                DealDetails.SaleTotal = (model.SaleValue1 + model.SaleValue2).ToString();
                DealDetails.DealPartyList = model.DealParties;
                for (int i = 0; i < model.DealPaySchedules.Count; i++)
                {
                    model.DealPaySchedules[i].Identity = i + 1;
                }
                decimal amt1=0;
                decimal amt2=0;
                foreach (var obj in model.DealPaySchedules) {
                    amt1 += obj.Amount1;
                    amt2 += obj.Amount2;
                    obj.Total = obj.Amount1 + obj.Amount2;
                }
                DealDetails.TotalAmount1 = amt1.ToString();
                DealDetails.TotalAmount2 = amt2.ToString();
                DealDetails.FinalAmount = (amt1+amt2).ToString();

                DealDetails.ScheduleList = model.DealPaySchedules;
               
            }
            catch (Exception ex)
            {
                LogException("Deal", "Load Details", ex);
            }
            finally
            {
                HideProgressRing();
            }
        }       
    }
}
