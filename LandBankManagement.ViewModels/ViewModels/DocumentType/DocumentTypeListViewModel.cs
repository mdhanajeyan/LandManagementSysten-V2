using LandBankManagement.Data;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;


namespace LandBankManagement.ViewModels
{
    public class DocumentTypeListArgs
    {
        static public DocumentTypeListArgs CreateEmpty() => new DocumentTypeListArgs { IsEmpty = true };

        public DocumentTypeListArgs()
        {
            OrderBy = r => r.DocumentTypeName;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<Data.DocumentType, object>> OrderBy { get; set; }
        public Expression<Func<Data.DocumentType, object>> OrderByDesc { get; set; }
    }
    public class DocumentTypeListViewModel : GenericListViewModel<DocumentTypeModel>
    {
        public IDocumentTypeService DocumentTypeService { get; }
        public DocumentTypeListArgs ViewModelArgs { get; private set; }

        public DocumentTypeListViewModel(IDocumentTypeService documentTypeService, ICommonServices commonServices) : base(commonServices)
        {
            DocumentTypeService = documentTypeService;
        }
        public async Task LoadAsync(DocumentTypeListArgs args)
        {
            ViewModelArgs = args ?? DocumentTypeListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Loading Document Type...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Document Type loaded");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<DocumentTypeListViewModel>(this, OnMessage);

        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public DocumentTypeListArgs CreateArgs()
        {
            return new DocumentTypeListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            Items = null;
            ItemsCount = 0;
            SelectedItem = null;

            try
            {
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<DocumentTypeModel>();
                StatusError($"Error loading Document Type: {ex.Message}");
                LogException("Document Type", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                // SelectedItem = Items.FirstOrDefault(); // Note : Avoid Auto selection
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<DocumentTypeModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<Data.DocumentType> request = BuildDataRequest();
                return await DocumentTypeService.GetDocumentTypesAsync(request);
            }
            return new List<DocumentTypeModel>();
        }

        //public ICommand OpenInNewViewCommand => new RelayCommand(OnOpenInNewView);
        //private async void OnOpenInNewView()
        //{
        //    if (SelectedItem != null)
        //    {
        //        await NavigationService.CreateNewViewAsync<DocumentTypeViewModel>(new PartyDetailsArgs { PartyId = SelectedItem.DocumentTypeId });
        //    }
        //}

        protected override async void OnNew()
        {

            // await NavigationService.CreateNewViewAsync<DocumentTypeViewModel>(new DocumentTypeArgs());

            StatusReady();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Loading DocumentType...");
            if (await RefreshAsync())
            {
                EndStatusMessage("DocumentType loaded");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected DocumentType?", "Ok", "Cancel"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Deleting {count} DocumentType...");
                        // await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, "ItemRangesDeleted", SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Deleting {count} DocumentType...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, "ItemsDeleted", SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Error deleting {count} DocumentType: {ex.Message}");
                    LogException("DocumentTypes", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} DocumentType deleted");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<DocumentTypeModel> models)
        {
            foreach (var model in models)
            {
                await DocumentTypeService.DeleteDocumentTypeAsync(model);
            }
        }

        //private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        //{
        //    DataRequest<Vendor> request = BuildDataRequest();
        //    foreach (var range in ranges)
        //    {
        //        await VendorService.DeleteVendorRangeAsync(range.Index, range.Length, request);
        //    }
        //}

        private DataRequest<Data.DocumentType> BuildDataRequest()
        {
            return new DataRequest<Data.DocumentType>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            switch (message)
            {
                case "NewItemSaved":
                case "ItemDeleted":
                case "ItemsDeleted":
                case "ItemRangesDeleted":
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }
    }
}
