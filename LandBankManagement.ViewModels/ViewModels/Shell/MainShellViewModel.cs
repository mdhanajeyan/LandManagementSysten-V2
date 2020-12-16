using LandBankManagement.Data;
using LandBankManagement.Enums;
using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LandBankManagement.ViewModels
{
    public class MainShellViewModel : ShellViewModel
    {
        private UserInfoModel _userInfo;
        private readonly NavigationItem DashboardItem = new NavigationItem(0xE80F, "Dashboard", typeof(DashboardViewModel));



        private readonly NavigationItem SetupItem = new NavigationItem("Set-up", 0xF0AD)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                 new NavigationItem(0xf1ad, "Company", typeof(CompanyViewModel)){Screen=NavigationScreen.Company},
                 new NavigationItem(0xf21d, "Vendor", typeof(VendorViewModel)){Screen=NavigationScreen.Vendor},
                 new NavigationItem(0xf263, "Party", typeof(PartyViewModel)){Screen=NavigationScreen.Party},
                 new NavigationItem(0xf19c, "Bank", typeof(BankAccountViewModel)){Screen=NavigationScreen.Bank},
                 new NavigationItem(0xf156, "Cash", typeof(CashAccountViewModel)){Screen=NavigationScreen.Cash},
                 new NavigationItem(0xf19d, "ExpenseHead", typeof(ExpenseHeadViewModel)){Screen=NavigationScreen.ExpenseHead},
                 new NavigationItem(0xf279, "Taluk", typeof(TalukViewModel)){Screen=NavigationScreen.Taluk},
                 new NavigationItem(0xf018, "Hobli", typeof(HobliViewModel)){Screen=NavigationScreen.Hobli},
                 new NavigationItem(0xf1bb, "Village", typeof(VillageViewModel)){Screen=NavigationScreen.Village},
                 new NavigationItem(0xf0cb, "Property CheckList Master", typeof(CheckListViewModel)){Screen=NavigationScreen.PropertyCheckList},
                 new NavigationItem(0xf035, "Property Type", typeof(PropertyTypeViewModel)){Screen=NavigationScreen.PropertyType},
                 new NavigationItem(0xf035, "Property Check List", typeof(PropertyCheckListViewModel)){Screen=NavigationScreen.PropertyCheckList},
            }
        };

        private readonly NavigationItem TransactionItem = new NavigationItem("Transaction", 0xf218)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xf1ed, "Payments", typeof(PaymentsViewModel)){Screen=NavigationScreen.Payments},
                 new NavigationItem(0xf101, "Fund Transfer", typeof(FundTransferViewModel)){Screen=NavigationScreen.FundTransfer},
            }
        };

        private readonly NavigationItem ReportItem = new NavigationItem("Report", 0xf201)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                 new NavigationItem(0xE9F9, "Report", typeof(CompanyReportViewModel))
            }
        };

        private readonly NavigationItem AdminItem = new NavigationItem("Admin", 0xf170)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xf243, "View Log", typeof(AppLogsViewModel)){IconColor = "Red",Screen=NavigationScreen.ViewLogs},
                new NavigationItem(0xf044, "Role", typeof(RoleViewModel)){Screen=NavigationScreen.Role},
                  new NavigationItem(0xf044, "Role Permission", typeof(RolePermissionViewModel)){Screen=NavigationScreen.RolePermission},
                new NavigationItem(0xf2bb, "User", typeof(UserViewModel)){Screen=NavigationScreen.UserInfo},
            }
        };

        private readonly NavigationItem PropertyItem = new NavigationItem("Property", 0xf231)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xf041, "Property", typeof(PropertyViewModel)){ Screen=NavigationScreen.Property}

            }
        };

        public MainShellViewModel(ILoginService loginService, ICommonServices commonServices) : base(loginService, commonServices)
        {

        }

        private void SetMenuPermissions()
        {
            SetupItem.Children.ToList().ForEach(x => x.HasPermission = _userInfo.Permission.Any(item => (NavigationScreen)item.ScreenId == x.Screen));
            TransactionItem.Children.ToList().ForEach(x => x.HasPermission = _userInfo.Permission.Any(item => (NavigationScreen)item.ScreenId == x.Screen));
            ReportItem.Children.ToList().ForEach(x => x.HasPermission = _userInfo.Permission.Any(item => (NavigationScreen)item.ScreenId == x.Screen));
            AdminItem.Children.ToList().ForEach(x => x.HasPermission = _userInfo.Permission.Any(item => (NavigationScreen)item.ScreenId == x.Screen));
            PropertyItem.Children.ToList().ForEach(x => x.HasPermission = _userInfo.Permission.Any(items => (NavigationScreen)items.ScreenId == x.Screen));
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
            HideProgressRing();
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
                case "ReceiptsViewModel":
                    NavigationService.Navigate(viewModel, new ReceiptsListArgs());
                    break;
                case "RoleViewModel":
                    NavigationService.Navigate(viewModel, new RoleListArgs());
                    break;
                case "UserViewModel":
                    NavigationService.Navigate(viewModel, new UserListArgs());
                    break;
                case "PropertyViewModel":
                    NavigationService.Navigate(viewModel, new PropertyListArgs());
                    break;
                case "RolePermissionViewModel":
                    NavigationService.Navigate(viewModel);
                    break;
                case "PropertyCheckListViewModel":
                    NavigationService.Navigate(viewModel, new PropertyCheckListArgs());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<NavigationItem> GetItems()
        {
            yield return SetupItem;
            yield return PropertyItem;
            yield return TransactionItem;
            yield return ReportItem;
            yield return AdminItem;
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
            AdminItem.Badge = count > 0 ? count.ToString() : null;
        }
    }
}
