﻿using LandBankManagement.Data;
using LandBankManagement.Enums;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LandBankManagement;

namespace LandBankManagement.ViewModels
{
    public class MainShellViewModel : ShellViewModel
    {
        private UserInfoModel _userInfo;
        private readonly NavigationItem DashboardItem = new NavigationItem(0xE80F, "Dashboard", typeof(DashboardViewModel));

        private readonly NavigationItem AppLogsItem = new NavigationItem("Activity Logs")
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xE7BA, "View Log", typeof(AppLogsViewModel)){IconColor = "Red",Screen=NavigationScreen.ViewLogs}
            }
        };
        private readonly NavigationItem ReportItem = new NavigationItem("Report")
        {
            Children = new ObservableCollection<NavigationItem>
            {
                 new NavigationItem(0xE9F9, "Report", typeof(CompanyReportViewModel))
            }
        };
        private readonly NavigationItem SetupItem = new NavigationItem("Set-up", 0xF0AD)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                 new NavigationItem(0xE9F9, "Comapny", typeof(CompanyViewModel)),
                 new NavigationItem(0xE731, "Vendor", typeof(VendorViewModel)),
                 new NavigationItem(0xE716, "Party", typeof(PartyViewModel)),
                 new NavigationItem(0xE825, "Bank", typeof(BankAccountViewModel)),
                 new NavigationItem(0xF584, "Cash", typeof(CashAccountViewModel)),
                 new NavigationItem(0xE912, "ExpenseHead", typeof(ExpenseHeadViewModel)),
                 new NavigationItem(0xE759, "Taluk", typeof(TalukViewModel)),
                 new NavigationItem(0xE802, "Hobli", typeof(HobliViewModel)),
                 new NavigationItem(0xF156, "Village", typeof(VillageViewModel)),
                 new NavigationItem(0xF0B5, "Property CheckList Master", typeof(CheckListViewModel)),
                  new NavigationItem(0xF97C, "Property Type", typeof(PropertyTypeViewModel))
            }
        };

        private readonly NavigationItem PropertyItem = new NavigationItem("Property")
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xE8C7, "Payments", typeof(PaymentsViewModel)),
                 new NavigationItem(0xE912, "Fund Transfer", typeof(FundTransferViewModel))
            }
        };

        public MainShellViewModel(ILoginService loginService, ICommonServices commonServices) : base(loginService, commonServices)
        {

        }

        private void SetMenuPermissions()
        {
            AppLogsItem.Children.ToList().ForEach(x => x.HasPermission = _userInfo.Permission.Any(item => (NavigationScreen)item.ScreenId == x.Screen));
            SetupItem.Children.ToList().ForEach(x => x.HasPermission = _userInfo.Permission.Any(item => (NavigationScreen)item.ScreenId == x.Screen));
            ReportItem.Children.ToList().ForEach(x => x.HasPermission = _userInfo.Permission.Any(item => (NavigationScreen)item.ScreenId == x.Screen));

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
            _userInfo = args.UserInfo;
            SetMenuPermissions();
        }


        public async void NavigateTo(Type viewModel)
        {
            switch (viewModel.Name)
            {
                case "DashboardViewModel":
                    NavigationService.Navigate(viewModel);
                    break;
                case "CompanyViewModel":
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
                case "VendorViewModel":
                    NavigationService.Navigate(viewModel, new VendorListArgs());
                    break;
                case "PartyViewModel":
                    NavigationService.Navigate(viewModel, new PartyListArgs());
                    break;
                case "ExpenseHeadViewModel":
                    NavigationService.Navigate(viewModel, new ExpenseHeadListArgs());
                    break;
                case "TalukViewModel":
                    NavigationService.Navigate(viewModel, new TalukListArgs());
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
                case "BankAccountViewModel":
                    NavigationService.Navigate(viewModel, new BankAccountListArgs());
                    break;
                case "DocumentTypeViewModel":
                    NavigationService.Navigate(viewModel, new DocumentTypeListArgs());
                    break;
                case "CheckListViewModel":
                    NavigationService.Navigate(viewModel, new CheckListListArgs());
                    break;
                case "PropertyTypeViewModel":
                    NavigationService.Navigate(viewModel, new PropertyTypeListArgs());
                    break;
                case "CompanyReportViewModel":
                    NavigationService.Navigate(viewModel, new CompanyReportArgs());
                    break;
                case "PaymentsViewModel":
                    NavigationService.Navigate(viewModel, new PaymentsListArgs());
                    break;
                case "FundTransferViewModel":
                    NavigationService.Navigate(viewModel, new FundTransferListArgs());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<NavigationItem> GetItems()
        {
            yield return SetupItem;
            yield return PropertyItem;
            yield return ReportItem;
            yield return AppLogsItem;
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
