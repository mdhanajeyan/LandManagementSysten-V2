using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LandBankManagement.Models;
using LandBankManagement.Services;

namespace LandBankManagement.ViewModels
{
   public class CheckListViewModel : ViewModelBase
    {
        ICheckListService CheckListService { get; }
        public CheckListListViewModel CheckListList { get; set; }

        public CheckListDetailsViewModel CheckListDetials { get; set; }

        public CheckListViewModel(ICommonServices commonServices, IFilePickerService filePickerService, ICheckListService checkListService) : base(commonServices)
        {
            CheckListService = checkListService;
            CheckListList = new CheckListListViewModel(checkListService, commonServices);
            CheckListDetials = new CheckListDetailsViewModel(checkListService, filePickerService, commonServices);
        }

        public async Task LoadAsync(CheckListListArgs args)
        {
            await CheckListList.LoadAsync(args);
        }
        public void Unload()
        {
            CheckListList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CheckListListViewModel>(this, OnMessage);
            CheckListList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            CheckListList.Unsubscribe();

        }

        private async void OnMessage(CheckListListViewModel viewModel, string message, object args)
        {
            if (viewModel == CheckListList && message == "ItemSelected")
            {
                await ContextService.RunAsync(() =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {

            var selected = CheckListList.SelectedItem;
            if (!CheckListList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
        }

        private async Task PopulateDetails(CheckListModel selected)
        {
            try
            {
                var model = await CheckListService.GetCheckListAsync(selected.CheckListId);
                selected.Merge(model);
                CheckListDetials.Item = model;
            }
            catch (Exception ex)
            {
                LogException("CheckList", "Load Details", ex);
            }
        }

    }
}
