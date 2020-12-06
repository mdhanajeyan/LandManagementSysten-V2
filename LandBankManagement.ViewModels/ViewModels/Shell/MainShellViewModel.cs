using LandBankManagement.Data;
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
        private readonly NavigationItem DashboardItem = new NavigationItem(0xE80F, "Dashboard", typeof(DashboardViewModel));

        private readonly NavigationItem AppLogsItem = new NavigationItem("Activity Logs")
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xE7BA, "View Log", typeof(AppLogsViewModel)){IconColor = "Red"}
            }
        };
        private readonly NavigationItem ReportItem = new NavigationItem( "Report")
        {
            Children = new ObservableCollection<NavigationItem>
            {
                 new NavigationItem(0xE9F9, "Report", typeof(CompanyReportViewModel))
            }
        };
        private readonly NavigationItem SetupItem = new NavigationItem("Set-up", 0xE9F9)
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

        private readonly NavigationItem PropertyItem = new NavigationItem( "Property")
        {
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem(0xE8C7, "Payments", typeof(PaymentsViewModel)),
                 new NavigationItem(0xE912, "Fund Transfer", typeof(FundTransferViewModel))
            }
        };
        private readonly NavigationItem VendorItem = new NavigationItem(0xE731, "Vendor", typeof(VendorViewModel));
        private readonly NavigationItem PartyItem = new NavigationItem(0xE716, "Party", typeof(PartyViewModel));
        private readonly NavigationItem ExpenseHeadItem = new NavigationItem(0xE912, "ExpenseHead", typeof(ExpenseHeadViewModel));
        private readonly NavigationItem TalukItem = new NavigationItem(0xE759, "Taluk", typeof(TalukViewModel));
        private readonly NavigationItem HobliItem = new NavigationItem(0xE802, "Hobli", typeof(HobliViewModel));
        private readonly NavigationItem VillageItem = new NavigationItem(0xF156, "Village", typeof(VillageViewModel));
        private readonly NavigationItem CashAccountItem = new NavigationItem(0xF584, "Cash Account", typeof(CashAccountViewModel));
        private readonly NavigationItem BankAccountItem = new NavigationItem(0xE825, "Bank Account", typeof(BankAccountViewModel));
        private readonly NavigationItem DocumentTypeItem = new NavigationItem(0xF8A5, "Document Type", typeof(DocumentTypeViewModel));
        private readonly NavigationItem CheckListItem = new NavigationItem(0xF0B5, "CheckList", typeof(CheckListViewModel));
        private readonly NavigationItem PropertyTypeItem = new NavigationItem(0xF97C, "Property Type", typeof(PropertyTypeViewModel));

        private readonly NavigationItem PaymentsItem = new NavigationItem(0xE8C7, "Payments", typeof(PaymentsViewModel));
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
            //yield return AppLogsItem;
            //yield return CompanyItem;
            //yield return VendorItem;
            //yield return PartyItem;
            //yield return CashAccountItem;
            //yield return BankAccountItem;
            //yield return ExpenseHeadItem;
            //yield return TalukItem;
            //yield return HobliItem;
            //yield return VillageItem;
            //yield return DocumentTypeItem;
            //yield return CheckListItem;
            //yield return PropertyTypeItem;
            //yield return PaymentsItem;
            //yield return DashboardItem;

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
