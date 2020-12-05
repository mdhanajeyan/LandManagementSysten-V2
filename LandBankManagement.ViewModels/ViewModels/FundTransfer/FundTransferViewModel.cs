﻿using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class FundTransferViewModel : ViewModelBase
    {
        IFundTransferService FundTransferService { get; }
        public FundTransferListViewModel FundTransferList { get; set; }

        public FundTransferDetailsViewModel FundTransferDetails { get; set; }

        public FundTransferViewModel(IDropDownService dropDownService, ICommonServices commonServices, IFilePickerService filePickerService, IFundTransferService fundTransferService) : base(commonServices)
        {
            FundTransferService = fundTransferService;
            FundTransferList = new FundTransferListViewModel(FundTransferService, commonServices);
            FundTransferDetails = new FundTransferDetailsViewModel(dropDownService, FundTransferService, filePickerService, commonServices);
        }

        public async Task LoadAsync(FundTransferListArgs args)
        {
            await FundTransferDetails.LoadAsync();
            await FundTransferList.LoadAsync(args);
        }
        public void Unload()
        {
            FundTransferList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<FundTransferListViewModel>(this, OnMessage);
            FundTransferList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            FundTransferList.Unsubscribe();

        }

        private async void OnMessage(FundTransferListViewModel viewModel, string message, object args)
        {
            if (viewModel == FundTransferList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = FundTransferList.SelectedItem;
            if (!FundTransferList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(FundTransferModel selected)
        {
            try
            {
                var model = await FundTransferService.GetFundTransferAsync(selected.FundTransferId);
                selected.Merge(model);
                FundTransferDetails.Item = model;
                FundTransferDetails.defaultSettings();
                SelectedPivotIndex = 0;
            }
            catch (Exception ex)
            {
                LogException("FundTransfer", "Load Details", ex);
            }
        }
    }

}