﻿using LandBankManagement.Data;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandBankManagement.ViewModels
{
    public class MainShellViewModel : ShellViewModel
    {
        private readonly NavigationItem DashboardItem = new NavigationItem(0xE80F, "Dashboard", typeof(DashboardViewModel));
        private readonly NavigationItem AppLogsItem = new NavigationItem(0xE7BA, "Activity Log", typeof(AppLogsViewModel));
        private readonly NavigationItem CompanyItem = new NavigationItem(0xEC0B, "Company", typeof(CompaniesViewModel));
        private readonly NavigationItem VendorItem = new NavigationItem(0xE731, "Vendor", typeof(VendorsViewModel));
        private readonly NavigationItem PartyItem = new NavigationItem(0xE716, "Party", typeof(PartiesViewModel));
        private readonly NavigationItem ExpenseHeadItem = new NavigationItem(0xE912, "ExpenseHead", typeof(ExpenseHeadViewModel));
        private readonly NavigationItem TalukItem = new NavigationItem(0xE759, "Taluk", typeof(TalukViewModel));
        private readonly NavigationItem HobliItem = new NavigationItem(0xE802, "Hobli", typeof(HobliViewModel));
        private readonly NavigationItem VillageItem = new NavigationItem(0xF156, "Village", typeof(VillageViewModel));
        private readonly NavigationItem CashAccountItem = new NavigationItem(0xF584, "Cash Account", typeof(CashAccountViewModel));
        private readonly NavigationItem DocumentTypeItem = new NavigationItem(0xF8A5, "Document Type", typeof(DocumentTypeViewModel));
        public MainShellViewModel(ILoginService loginService, ICommonServices commonServices) : base(loginService, commonServices)
        {
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private bool _isPaneOpen = true;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => Set(ref _isPaneOpen, value);
        }

        private IEnumerable<NavigationItem> _items;
        public IEnumerable<NavigationItem> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        public override async Task LoadAsync(ShellArgs args)
        {
            Items = GetItems().ToArray();
            await UpdateAppLogBadge();
            await base.LoadAsync(args);
        }


        public async void NavigateTo(Type viewModel)
        {
            switch (viewModel.Name)
            {
                case "DashboardViewModel":
                    NavigationService.Navigate(viewModel);
                    break;
                case "CompaniesViewModel":
                    NavigationService.Navigate(viewModel, new CompanyListArgs());
                    break;
                case "AppLogsViewModel":
                    NavigationService.Navigate(viewModel, new AppLogListArgs());
                    await LogService.MarkAllAsReadAsync();
                    await UpdateAppLogBadge();
                    break;
                case "SettingsViewModel":
                  //  NavigationService.Navigate(viewModel, new SettingsArgs());
                    break;
                case "VendorsViewModel":
                    NavigationService.Navigate(viewModel,new VendorListArgs());
                    break;
                case "PartiesViewModel":
                    NavigationService.Navigate(viewModel, new PartyListArgs());
                    break;
                case "ExpenseHeadViewModel":
                    NavigationService.Navigate(viewModel,new ExpenseHeadArgs());
                    break;
                case "TalukViewModel":
                    NavigationService.Navigate(viewModel,new TalukListArgs());
                    break;
                case "HobliViewModel":
                    NavigationService.Navigate(viewModel, new HobliListArgs());
                    break;
                case "VillageViewModel":
                    NavigationService.Navigate(viewModel, new VillageListArgs());
                    break;
                case "CashAccountViewModel":
                    NavigationService.Navigate(viewModel, new CashAccountListArgs());
                    break;
                case "DocumentTypeViewModel":
                    NavigationService.Navigate(viewModel, new DocumentTypeListArgs());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<NavigationItem> GetItems()
        {
            yield return AppLogsItem;
            yield return CompanyItem;
            yield return VendorItem;
            yield return PartyItem;
            yield return ExpenseHeadItem;
            yield return TalukItem;
            yield return HobliItem;
            yield return VillageItem;
            yield return CashAccountItem;
            yield return DocumentTypeItem;
            yield return DashboardItem;
           
        }

        override public void Subscribe()
        {
            MessageService.Subscribe<ILogService, AppLog>(this, OnLogServiceMessage);
            base.Subscribe();
        }

        override public void Unsubscribe()
        {
            base.Unsubscribe();
        }

        public override void Unload()
        {
            base.Unload();
        }

        private async void OnLogServiceMessage(ILogService logService, string message, AppLog log)
        {
            if (message == "LogAdded")
            {
                await ContextService.RunAsync(async () =>
                {
                    await UpdateAppLogBadge();
                });
            }
        }

        private async Task UpdateAppLogBadge()
        {
            int count = await LogService.GetLogsCountAsync(new DataRequest<AppLog> { Where = r => !r.IsRead });
            AppLogsItem.Badge = count > 0 ? count.ToString() : null;
        }
    }
}
