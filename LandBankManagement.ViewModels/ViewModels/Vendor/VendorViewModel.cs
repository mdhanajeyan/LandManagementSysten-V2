using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class VendorViewModel : ViewModelBase
    {
        public IVendorService VendorService { get; }

        public VendorListViewModel VendorList { get; set; }
        public VendorDetailsViewModel VendorDetails { get; set; }
        public VendorViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IVendorService vendorService) : base(commonServices) {

            VendorService = vendorService;
            VendorList = new VendorListViewModel(vendorService, commonServices);
            VendorDetails = new VendorDetailsViewModel(vendorService, filePickerService, commonServices);
        }

        public async Task LoadAsync(VendorListArgs args)
        {
           // await VendorList.LoadAsync(args);
        }
        public void Unload()
        {
            VendorList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<VendorListViewModel>(this, OnMessage);
            VendorList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            VendorList.Unsubscribe();
        }

        private async void OnMessage(VendorListViewModel viewModel, string message, object args)
        {
            if (viewModel == VendorList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            var selected = VendorList.SelectedItem;
            if (!VendorList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            //if (VendorDetails.IsEditMode)
            //{
            //    StatusReady();
            //    VendorDetails.CancelEdit();
            //}

            //var selected = VendorList.SelectedItem;
            //if (!VendorList.IsMultipleSelection)
            //{
            //    if (selected != null && !selected.IsEmpty)
            //    {
            //        await PopulateDetails(selected);

            //    }
            //}
            //VendorDetails.Item = selected;
        }

        private async Task PopulateDetails(VendorModel selected)
        {
            try
            {
                var model = await VendorService.GetVendorAsync(selected.VendorId);
                selected.Merge(model);
                VendorDetails.Item = model;
                VendorDetails.DocList = model.VendorDocuments;
                if (model.VendorDocuments != null)
                {
                    for (int i = 0; i < VendorDetails.DocList.Count; i++)
                    {
                        VendorDetails.DocList[i].Identity = i + 1;
                    }
                }
                SelectedPivotIndex = 1;
            }
            catch (Exception ex)
            {
                LogException("Vendors", "Load Details", ex);
            }
        }

    }
}
