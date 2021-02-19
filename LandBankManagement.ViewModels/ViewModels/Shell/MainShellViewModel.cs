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
        private readonly NavigationItem DashboardItem = new NavigationItem(0xf135, "Dashboard",false,true, typeof(DashboardViewModel)) { Screen = NavigationScreen.Default };



        private readonly NavigationItem SetupItem = new NavigationItem("Set-up", 0xF0AD)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                 new NavigationItem(0xf1ad, "Company",true,false, typeof(CompanyViewModel)){Screen=NavigationScreen.Company},
                 new NavigationItem(0xf0c0, "Groups",true,false, typeof(GroupsViewModel)){Screen=NavigationScreen.Groups},
                 new NavigationItem(0xf21d, "Vendor",true,false, typeof(VendorViewModel)){Screen=NavigationScreen.Vendor},
                 new NavigationItem(0xf263, "Party",true,false, typeof(PartyViewModel)){Screen=NavigationScreen.Party},
                 new NavigationItem(0xf19c, "Bank",true,false, typeof(BankAccountViewModel)){Screen=NavigationScreen.Bank},
                 new NavigationItem(0xf156, "Cash",true,false, typeof(CashAccountViewModel)){Screen=NavigationScreen.Cash},
                 new NavigationItem(0xf19d, "ExpenseHead",true,false, typeof(ExpenseHeadViewModel)){Screen=NavigationScreen.ExpenseHead},
                 new NavigationItem(0xf279, "Taluk",true,false, typeof(TalukViewModel)){Screen=NavigationScreen.Taluk},
                 new NavigationItem(0xf018, "Hobli",true,false, typeof(HobliViewModel)){Screen=NavigationScreen.Hobli},
                 new NavigationItem(0xf1bb, "Village", true,false,typeof(VillageViewModel)){Screen=NavigationScreen.Village},
                 new NavigationItem(0xf0cb, "Property CheckList Master",true,false, typeof(CheckListViewModel)){Screen=NavigationScreen.PropertyCheckList},
                 new NavigationItem(0xf035, "Property Type",true,false, typeof(PropertyTypeViewModel)){Screen=NavigationScreen.PropertyType},
                 new NavigationItem(0xf24a, "Document Type",true,false, typeof(DocumentTypeViewModel)){Screen=NavigationScreen.DocumentType}

            }
        };

        private readonly NavigationItem TransactionItem = new NavigationItem("Transaction", 0xf218)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xf1ed, "Payments", true,false,typeof(PaymentsViewModel)){Screen=NavigationScreen.Payments},
                new NavigationItem(0xf0d6, "Fund Transfer",true,false, typeof(FundTransferViewModel)){Screen=NavigationScreen.FundTransfer},
                new NavigationItem(0xf044, "Receipts", true,false,typeof(ReceiptsViewModel)){Screen=NavigationScreen.Receipt},
            }
        };

        private readonly NavigationItem ReportItem = new NavigationItem("Report", 0xf201)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                 new NavigationItem(0xE9F9, "Company Report",true,false, typeof(CompanyReportViewModel)),
                 new NavigationItem(0xf035, "Deal Report", true,false,typeof(DealReportViewModel)),
                 new NavigationItem(0xf035, "Property CheckList Report",true,false, typeof(PropertyCheckListReportViewModel))
            }
        };

        private readonly NavigationItem AdminItem = new NavigationItem("Admin", 0xf170)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xf243, "View Log",true,false, typeof(AppLogsViewModel)){IconColor = "Red",Screen=NavigationScreen.ViewLogs},
                new NavigationItem(0xf044, "Role", true,false,typeof(RoleViewModel)){Screen=NavigationScreen.Role},
                new NavigationItem(0xf044, "Role Permission",true,false, typeof(RolePermissionViewModel)){Screen=NavigationScreen.RolePermission},
                new NavigationItem(0xf2bb, "User", true,false,typeof(UserViewModel)){Screen=NavigationScreen.UserInfo}
            }
        };

        private readonly NavigationItem PropertyItem = new NavigationItem("Property", 0xf231)
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xf035, "Property Check List",true,false, typeof(PropertyCheckListViewModel)){Screen=NavigationScreen.PropertyCheckList},
                new NavigationItem(0xf041, "Property",true,false, typeof(PropertyViewModel)){ Screen=NavigationScreen.Property},
                new NavigationItem(0xf12e, "Proposals",true,false, typeof(PropertyMergeViewModel)){Screen=NavigationScreen.MergeProperties},
                new NavigationItem(0xf2b5, "Deal",true,false, typeof(DealViewModel)){Screen=NavigationScreen.PropertyDeals}
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

            AdminItem.Children.Where(x => x.Screen == NavigationScreen.Default).ToList().ForEach(x => x.HasPermission = true);
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
            // HideProgressRing();
        }

        public void ClosePopup()
        {
            ShowSuccessPopupMessage = false;
            ShowErrorPopupMessage = false;
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
                    NavigationService.Navigate(viewModel, new SettingsArgs());
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
                    NavigationService.Navigate(viewModel, new PropertyCheckListListArgs());
                    break;
                case "PropertyMergeViewModel":
                    NavigationService.Navigate(viewModel, new PropertyMergeListArgs());
                    break;
                case "DealViewModel":
                    NavigationService.Navigate(viewModel, new DealListArgs());
                    break;
                case "DealReportViewModel":
                    NavigationService.Navigate(viewModel, new DealReportArgs());
                    break;
                case "PropertyCheckListReportViewModel":
                    NavigationService.Navigate(viewModel, new PropertyCheckListReportArgs());
                    break;
                case "GroupsViewModel":
                    NavigationService.Navigate(viewModel, new GroupsListArgs());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<NavigationItem> GetItems()
        {
            yield return DashboardItem;
            yield return SetupItem;
            yield return PropertyItem;
            yield return TransactionItem;
            //  yield return ReportItem;
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
