using System;
using System.Threading.Tasks;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
    public class VendorsViewModel : ViewModelBase
    {
        public IVendorService VendorService { get; }

        public VendorListViewModel VendorList { get; set; }
        public VendorDetailsViewModel VendorDetails { get; set; }
        public VendorsViewModel(ICommonServices commonServices, IFilePickerService filePickerService, IVendorService vendorService) : base(commonServices) {

            VendorService = vendorService;
            VendorList = new VendorListViewModel(vendorService, commonServices);
            VendorDetails = new VendorDetailsViewModel(vendorService, filePickerService, commonServices);
        }

        public async Task LoadAsync(VendorListArgs args)
        {
            await VendorList.LoadAsync(args);
        }
        public void Unload()
        {
            VendorDetails.CancelEdit();
            VendorList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<VendorListViewModel>(this, OnMessage);
            VendorList.Subscribe();
            VendorDetails.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            VendorList.Unsubscribe();
            VendorDetails.Unsubscribe();

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
            if (VendorDetails.IsEditMode)
            {
                StatusReady();
                VendorDetails.CancelEdit();
            }

            var selected = VendorList.SelectedItem;
            if (!VendorList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);

                }
            }
            VendorDetails.Item = selected;
        }

        private async Task PopulateDetails(VendorModel selected)
        {
            try
            {
                var model = await VendorService.GetVendorAsync(selected.VendorId);
                selected.Merge(model);
            }
            catch (Exception ex)
            {
                LogException("Vendors", "Load Details", ex);
            }
        }

    }
}
